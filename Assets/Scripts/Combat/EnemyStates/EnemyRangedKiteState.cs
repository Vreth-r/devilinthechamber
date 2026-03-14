using UnityEngine;
using UnityEngine.AI;

public class EnemyRangedKiteState : IEnemyState
{
    private readonly EnemyContext ctx;
    private readonly EnemyStateMachine fsm;

    private readonly Collider[] separationHits = new Collider[16];

    private float shotTimer;
    private float repathTimer;
    private float orbitDecisionTimer;
    private float dartDecisionTimer;

    private int orbitDir;
    private int dartDir;

    public string Name => "RangedKite";

    public EnemyRangedKiteState(EnemyContext ctx, EnemyStateMachine fsm)
    {
        this.ctx = ctx;
        this.fsm = fsm;
    }

    public void Enter()
    {
        if (ctx.agent)
        {
            ctx.agent.isStopped = false;
            ctx.agent.stoppingDistance = 0f;
            ctx.agent.ResetPath();
        }

        shotTimer = GetFireInterval() * Mathf.Max(0f, ctx.stats.initialShotDelayMultiplier);
        repathTimer = 0f;
        orbitDecisionTimer = 0f;
        dartDecisionTimer = Random.Range(
            ctx.stats.dartIntervalMin,
            ctx.stats.dartIntervalMax
        );

        orbitDir = Random.value < 0.5f ? -1 : 1;
        dartDir = Random.value < 0.5f ? -1 : 1;
    }

    public void Tick(float dt)
    {
        if (!ctx.HasTarget)
            return;

        EnemyCombatUtil.FaceTarget(ctx, dt);

        UpdateFire(dt);
        UpdateMovementTimers(dt);
        UpdateMovement();
    }

    public void Exit()
    {
        if (ctx.agent)
            ctx.agent.isStopped = false;
    }

    private void UpdateFire(float dt)
    {
        shotTimer -= dt;

        if (!IsTargetInFireRange())
            return;

        float fireInterval = GetFireInterval();

        while (shotTimer <= 0f)
        {
            FireProjectile();
            shotTimer += fireInterval;
        }
    }

    private void UpdateMovementTimers(float dt)
    {
        repathTimer -= dt;
        orbitDecisionTimer -= dt;
        dartDecisionTimer -= dt;
    }

    private void UpdateMovement()
    {
        if (!ctx.agent || !ctx.agent.enabled)
            return;

        float distanceToTarget = ctx.DistanceToTarget();

        float preferredMin = ctx.stats.preferredRange - ctx.stats.rangeTolerance;
        float preferredMax = ctx.stats.preferredRange + ctx.stats.rangeTolerance;

        if (distanceToTarget < preferredMin)
        {
            HandleRetreat();
            return;
        }

        if (distanceToTarget > preferredMax)
        {
            HandleApproach();
            return;
        }

        HandleOrbit(distanceToTarget, preferredMin, preferredMax);
    }

    private void HandleRetreat()
    {
        if (!ReadyToRepath())
            return;

        Vector3 awayFromTarget = GetFlatDirection(
            ctx.target.position,
            ctx.self.position,
            ctx.self.forward
        );

        Vector3 separationOffset = ComputeAllySeparation();
        Vector3 desiredOffset = awayFromTarget * ctx.stats.retreatStep + separationOffset;

        SetAgentDestinationSafe(ctx.self.position + desiredOffset);
    }

    private void HandleApproach()
    {
        if (!ReadyToRepath())
            return;

        Vector3 towardTarget = GetFlatDirection(
            ctx.self.position,
            ctx.target.position,
            ctx.self.forward
        );

        Vector3 separationOffset = ComputeAllySeparation();
        Vector3 desiredOffset = towardTarget * ctx.stats.approachStep + separationOffset;

        SetAgentDestinationSafe(ctx.self.position + desiredOffset);
    }

    private void HandleOrbit(float distanceToTarget, float preferredMin, float preferredMax)
    {
        if (!ReadyToRepath())
            return;

        if (orbitDecisionTimer <= 0f)
        {
            orbitDecisionTimer = 1f / Mathf.Max(0.01f, ctx.stats.orbitRecalcHz);

            if (Random.value < ctx.stats.orbitFlipChance)
                orbitDir *= -1;
        }

        if (dartDecisionTimer <= 0f)
        {
            dartDecisionTimer = Random.Range(
                ctx.stats.dartIntervalMin,
                ctx.stats.dartIntervalMax
            );

            if (Random.value < ctx.stats.dartChance)
                dartDir = Random.value < 0.5f ? -1 : 1;
        }

        Vector3 towardTarget = GetFlatDirection(
            ctx.self.position,
            ctx.target.position,
            ctx.self.forward
        );

        Vector3 side = Vector3.Cross(Vector3.up, towardTarget) * orbitDir;

        float normalizedBandPos = Mathf.InverseLerp(preferredMin, preferredMax, distanceToTarget);
        float radialBias = Mathf.Lerp(
            ctx.stats.orbitRadialBiasNear,
            ctx.stats.orbitRadialBiasFar,
            normalizedBandPos
        );

        Vector3 orbitOffset = side * ctx.stats.orbitStep;
        Vector3 dartOffset = side * dartDir * ctx.stats.dartStep;
        Vector3 radialOffset = towardTarget * radialBias;
        Vector3 separationOffset = ComputeAllySeparation();

        Vector3 desiredOffset =
            orbitOffset +
            dartOffset +
            radialOffset +
            separationOffset;

        SetAgentDestinationSafe(ctx.self.position + desiredOffset);
    }

    private bool IsTargetInFireRange()
    {
        return ctx.DistanceToTarget() <= ctx.stats.rangedFireMaxRange;
    }

    private float GetFireInterval()
    {
        return 1f / Mathf.Max(0.01f, ctx.stats.fireRate);
    }

    private void FireProjectile()
    {
        if (!ctx.firePoint || !ctx.projectilePrefab || !ctx.target)
            return;

        float speedMod = StatModManager.GetStatModifier(StatName.LADY_PROJECTILE_SPEED);
        float finalSpeed = ctx.stats.projectileSpeed * speedMod;

        Vector3 origin = ctx.firePoint.position;
        Vector3 aimPoint = GetAimPoint(ctx.target, ctx.stats.projectileAimHeight);
        Vector3 direction = aimPoint - origin;

        if (direction.sqrMagnitude < 0.0001f)
            direction = ctx.firePoint.forward;

        direction.Normalize();

        Projectile projectile = Object.Instantiate(ctx.projectilePrefab);
        IgnoreShooterCollisions(projectile.gameObject, ctx.self);

        projectile.damage = ctx.stats.damage;
        projectile.hitMask = ctx.projectileHitMask;
        projectile.Launch(
            origin,
            direction,
            finalSpeed,
            ctx.stats.projectileMaxTravelDistance,
            ctx.stats.projectileLifetimeSafetyBuffer
        );
    }

    private bool ReadyToRepath()
    {
        if (repathTimer > 0f)
            return false;

        repathTimer = 1f / Mathf.Max(1f, ctx.stats.repathRateHz);
        return true;
    }

    private Vector3 ComputeAllySeparation()
    {
        if (ctx.stats.allySeparationRadius <= 0f || ctx.stats.allySeparationStrength <= 0f)
            return Vector3.zero;

        int hitCount = Physics.OverlapSphereNonAlloc(
            ctx.self.position,
            ctx.stats.allySeparationRadius,
            separationHits,
            ctx.stats.allySeparationMask,
            QueryTriggerInteraction.Ignore
        );

        Vector3 totalSeparation = Vector3.zero;
        int contributors = 0;

        for (int i = 0; i < hitCount; i++)
        {
            Collider hit = separationHits[i];
            if (!hit)
                continue;

            Transform otherRoot = hit.transform.root;
            if (otherRoot == ctx.self.root)
                continue;

            EnemyBrain otherEnemy = otherRoot.GetComponent<EnemyBrain>();
            if (!otherEnemy)
                continue;

            Vector3 away = ctx.self.position - otherRoot.position;
            away.y = 0f;

            float sqrDistance = away.sqrMagnitude;
            if (sqrDistance < 0.0001f)
                continue;

            float distance = Mathf.Sqrt(sqrDistance);
            float weight = 1f - Mathf.Clamp01(distance / ctx.stats.allySeparationRadius);

            totalSeparation += away.normalized * weight;
            contributors++;
        }

        if (contributors == 0 || totalSeparation.sqrMagnitude < 0.0001f)
            return Vector3.zero;

        totalSeparation.Normalize();
        return totalSeparation * ctx.stats.allySeparationStrength;
    }

    private static Vector3 GetAimPoint(Transform target, float fallbackAimHeight)
    {
        if (target.TryGetComponent<CharacterController>(out var cc))
            return target.TransformPoint(cc.center);

        if (target.TryGetComponent<Collider>(out var col))
            return col.bounds.center;

        return target.position + Vector3.up * fallbackAimHeight;
    }

    private static Vector3 GetFlatDirection(Vector3 from, Vector3 to, Vector3 fallbackForward)
    {
        Vector3 dir = to - from;
        dir.y = 0f;

        if (dir.sqrMagnitude < 0.001f)
            dir = fallbackForward;

        return dir.normalized;
    }

    private void SetAgentDestinationSafe(Vector3 desiredWorldPos)
    {
        if (!ctx.agent || !ctx.agent.enabled)
            return;

        ctx.agent.isStopped = false;

        if (NavMesh.SamplePosition(
            desiredWorldPos,
            out NavMeshHit hit,
            ctx.stats.navSampleDistance,
            NavMesh.AllAreas))
        {
            ctx.agent.SetDestination(hit.position);
            return;
        }

        if (NavMesh.SamplePosition(
            ctx.self.position,
            out NavMeshHit fallbackHit,
            ctx.stats.navSampleDistance,
            NavMesh.AllAreas))
        {
            ctx.agent.SetDestination(fallbackHit.position);
        }
    }

    private static void IgnoreShooterCollisions(GameObject projectileGO, Transform shooterRoot)
    {
        Collider[] projectileColliders = projectileGO.GetComponentsInChildren<Collider>();
        Collider[] shooterColliders = shooterRoot.GetComponentsInChildren<Collider>();

        for (int i = 0; i < projectileColliders.Length; i++)
        {
            for (int j = 0; j < shooterColliders.Length; j++)
            {
                Physics.IgnoreCollision(projectileColliders[i], shooterColliders[j], true);
            }
        }
    }
}