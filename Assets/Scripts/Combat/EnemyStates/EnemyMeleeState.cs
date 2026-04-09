using UnityEngine;
using UnityEngine.AI;

public class EnemyMeleeState : IEnemyState
{
    private enum Phase
    {
        Chase,
        Windup,
        Lunge,
        Recover,
        Backstep
    }

    private readonly EnemyContext ctx;
    private readonly EnemyStateMachine fsm;

    private readonly Collider[] hitResults = new Collider[8];
    private readonly Collider[] separationHits = new Collider[16];

    private Phase phase;

    private float phaseTimer;
    private float repathTimer;
    private float dartDecisionTimer;

    private int dartDir;
    private bool didHitThisLunge;
    private Vector3 lungeDirection;
    private Vector3 backstepDirection;

    public string Name => "Melee";

    public EnemyMeleeState(EnemyContext ctx, EnemyStateMachine fsm)
    {
        this.ctx = ctx;
        this.fsm = fsm;
    }

    public void Enter()
    {
        phase = Phase.Chase;
        phaseTimer = 0f;
        repathTimer = 0f;
        dartDecisionTimer = Random.Range(ctx.stats.dartIntervalMin, ctx.stats.dartIntervalMax);
        dartDir = Random.value < 0.5f ? -1 : 1;
        didHitThisLunge = false;
        lungeDirection = ctx.self.forward;
        backstepDirection = -ctx.self.forward;

        if (ctx.agent)
        {
            ctx.agent.isStopped = false;
            ctx.agent.stoppingDistance = GetDesiredStopDistance();
            ctx.agent.ResetPath();
        }
    }

    public void Tick(float dt)
    {
        if (!ctx.HasTarget)
            return;

        EnemyCombatUtil.FaceTarget(ctx, dt);

        repathTimer -= dt;
        dartDecisionTimer -= dt;

        switch (phase)
        {
            case Phase.Chase:
                TickChase();
                break;

            case Phase.Windup:
                TickWindup(dt);
                break;

            case Phase.Lunge:
                TickLunge(dt);
                break;

            case Phase.Recover:
                TickRecover(dt);
                break;

            case Phase.Backstep:
                TickBackstep(dt);
                break;
        }
    }

    public void Exit()
    {
        if (ctx.agent)
        {
            ctx.agent.isStopped = false;
            ctx.agent.updatePosition = true;
            ctx.agent.updateRotation = true;

            if (ctx.agent.isOnNavMesh)
                ctx.agent.Warp(ctx.self.position);
        }
    }

    private void TickChase()
    {
        if (ShouldEnterMeleeAttack())
        {
            EnterWindup();
            return;
        }

        if (!ReadyToRepath())
            return;

        Vector3 targetPos = GetTargetGroundPoint(ctx.target);
        Vector3 toTarget = targetPos - ctx.self.position;
        Vector3 planarToTarget = Vector3.ProjectOnPlane(toTarget, Vector3.up);

        if (planarToTarget.sqrMagnitude < 0.001f)
            planarToTarget = ctx.self.forward;

        planarToTarget.Normalize();

        Vector3 desiredGoal = targetPos - planarToTarget * GetDesiredStopDistance();

        if (GetPlanarDistanceToTarget() > GetEngageRange() + ctx.stats.attackRange)
            desiredGoal += ComputeDartOffset(planarToTarget);

        desiredGoal += ComputeAllySeparation();

        SetAgentDestinationSafe(desiredGoal);
    }

    private bool ShouldEnterMeleeAttack()
    {
        float surfaceDistance = GetMeleeSurfaceDistance();
        float engageRange = GetEngageRange();

        
        if (surfaceDistance <= engageRange)
        {
            return true;
        }

        if (ctx.agent && ctx.agent.enabled && ctx.agent.isOnNavMesh)
        {
            bool arrived =
                !ctx.agent.pathPending &&
                ctx.agent.remainingDistance <= ctx.agent.stoppingDistance + 0.2f;

            bool nearlyStopped = ctx.agent.velocity.sqrMagnitude <= 0.05f;

            
            if ((arrived || nearlyStopped) && surfaceDistance <= engageRange + 0.5f)
            {
                return true;
            }
        }

        return false;
    }

    private float GetPlanarDistanceToTarget()
    {
        if (!ctx.HasTarget)
            return float.PositiveInfinity;

        Vector3 targetPoint = GetTargetGroundPoint(ctx.target);
        Vector3 selfPoint = ctx.self.position;

        targetPoint.y = 0f;
        selfPoint.y = 0f;

        return Vector3.Distance(selfPoint, targetPoint);
    }

    private static Vector3 GetTargetGroundPoint(Transform target)
    {
        Vector3 pos = target.position;

        if (target.TryGetComponent<CharacterController>(out var cc))
        {
            Vector3 center = target.TransformPoint(cc.center);
            center.y = target.position.y;
            return center;
        }

        if (target.TryGetComponent<Collider>(out var col))
        {
            Vector3 center = col.bounds.center;
            center.y = target.position.y;
            return center;
        }

        return pos;
    }

    private float GetMeleeSurfaceDistance()
    {
        if (!ctx.HasTarget)
            return float.PositiveInfinity;

        Collider selfCol = ctx.self.GetComponentInChildren<Collider>();
        Collider targetCol = ctx.target.GetComponentInChildren<Collider>();

        if (!selfCol || !targetCol)
            return GetPlanarDistanceToTarget();

        Vector3 selfClosest = selfCol.ClosestPoint(targetCol.bounds.center);
        Vector3 targetClosest = targetCol.ClosestPoint(selfCol.bounds.center);

        selfClosest.y = 0f;
        targetClosest.y = 0f;

        return Vector3.Distance(selfClosest, targetClosest);
    }

    private void TickWindup(float dt)
    {
        if (GetMeleeSurfaceDistance() > GetAttackCommitRange())
        {
            EnterChase();
            return;
        }

        if (IsTooCrampedForAttack())
        {
            EnterBackstep();
            return;
        }

        if (ctx.agent)
        {
            ctx.agent.isStopped = true;
            ctx.agent.ResetPath();
        }

        phaseTimer -= dt;
        if (phaseTimer <= 0f)
            EnterLunge();
    }

    private void TickLunge(float dt)
    {
        if (!ctx.HasTarget)
            return;

        phaseTimer -= dt;

        if (ctx.agent)
        {
            ctx.agent.isStopped = true;
            ctx.agent.updatePosition = false;
            ctx.agent.updateRotation = false;
        }

        float lungeSpeed = ctx.stats.meleeLungeDistance / Mathf.Max(0.01f, ctx.stats.meleeLungeTime);
        Vector3 move = lungeDirection * lungeSpeed * dt;
        ctx.self.position += move;

        if (ctx.agent && ctx.agent.enabled && ctx.agent.isOnNavMesh)
            ctx.agent.nextPosition = ctx.self.position;

        if (!didHitThisLunge && phaseTimer <= ctx.stats.meleeLungeHitTime)
        {
            ctx.OnAttack?.Invoke();
            didHitThisLunge = true;
            PerformMeleeHit();
        }

        if (phaseTimer <= 0f)
            EnterRecover();
    }

    private void TickRecover(float dt)
    {
        phaseTimer -= dt;

        if (GetMeleeSurfaceDistance() > GetAttackCommitRange())
        {
            EnterChase();
            return;
        }

        if (IsTooCrampedForAttack())
        {
            EnterBackstep();
            return;
        }

        if (ctx.agent)
        {
            ctx.agent.isStopped = true;
            ctx.agent.ResetPath();
        }

        if (phaseTimer <= 0f)
            EnterWindup();
    }

    private void TickBackstep(float dt)
    {
        phaseTimer -= dt;

        if (ctx.agent)
        {
            ctx.agent.isStopped = true;
            ctx.agent.updatePosition = false;
            ctx.agent.updateRotation = false;
        }

        float backstepSpeed = ctx.stats.meleeBackstepDistance / Mathf.Max(0.01f, ctx.stats.meleeBackstepTime);
        Vector3 move = backstepDirection * backstepSpeed * dt;
        ctx.self.position += move;

        if (phaseTimer <= 0f)
            EnterWindup();
    }

    private void EnterChase()
    {
        phase = Phase.Chase;
        didHitThisLunge = false;

        if (ctx.agent)
        {
            ctx.agent.updatePosition = true;
            ctx.agent.updateRotation = true;
            ctx.agent.isStopped = false;
            ctx.agent.stoppingDistance = GetDesiredStopDistance();

            if (ctx.agent.isOnNavMesh)
                ctx.agent.Warp(ctx.self.position);
        }
    }

    private void EnterWindup()
    {
        phase = Phase.Windup;
        phaseTimer = ctx.stats.meleeWindupTime;
        didHitThisLunge = false;

        Vector3 targetPoint = GetTargetAimPoint(ctx.target, ctx.stats.meleeAttackHeightOffset);
        Vector3 toTarget = targetPoint - ctx.self.position;
        Vector3 planar = Vector3.ProjectOnPlane(toTarget, Vector3.up);

        if (planar.sqrMagnitude < 0.001f)
            planar = ctx.self.forward;

        lungeDirection = planar.normalized;
        backstepDirection = -lungeDirection;

        if (ctx.agent)
        {
            ctx.agent.updatePosition = true;
            ctx.agent.updateRotation = true;
            ctx.agent.isStopped = true;
            ctx.agent.ResetPath();

            if (ctx.agent.isOnNavMesh)
                ctx.agent.Warp(ctx.self.position);
        }
    }

    private void EnterLunge()
    {
        phase = Phase.Lunge;
        phaseTimer = Mathf.Max(0.01f, ctx.stats.meleeLungeTime);
        didHitThisLunge = false;
    }

    private void EnterRecover()
    {
        phase = Phase.Recover;
        phaseTimer = ctx.stats.meleeCooldownTime * StatModManager.GetStatModifier(StatName.DOG_RECOVERY_SPEED);

        if (ctx.agent)
        {
            ctx.agent.updatePosition = true;
            ctx.agent.updateRotation = true;
            ctx.agent.isStopped = true;

            if (ctx.agent.isOnNavMesh)
                ctx.agent.Warp(ctx.self.position);
        }
    }

    private void EnterBackstep()
    {
        phase = Phase.Backstep;
        phaseTimer = Mathf.Max(0.01f, ctx.stats.meleeBackstepTime);

        Vector3 toTarget = ctx.target.position - ctx.self.position;
        Vector3 planar = Vector3.ProjectOnPlane(toTarget, Vector3.up);

        if (planar.sqrMagnitude < 0.001f)
            planar = ctx.self.forward;

        backstepDirection = -planar.normalized;

        if (ctx.agent)
        {
            ctx.agent.isStopped = true;
            ctx.agent.updatePosition = false;
            ctx.agent.updateRotation = true;

            if (ctx.agent.isOnNavMesh)
                ctx.agent.Warp(ctx.self.position);
        }
    }

    private bool IsTooCrampedForAttack()
    {
        return GetMeleeSurfaceDistance() < ctx.stats.meleeMinSeparationDistance;
    }

    private float GetDesiredStopDistance()
    {
        return Mathf.Max(0.1f, ctx.stats.attackRange);
    }

    private float GetEngageRange()
    {
        return Mathf.Max(ctx.stats.attackRange, ctx.stats.meleeLungeStartRange);
    }

    private float GetAttackCommitRange()
    {
        return Mathf.Max(
            ctx.stats.attackRange + ctx.stats.meleeCommitRangePadding,
            ctx.stats.meleeAttackRangeForward + ctx.stats.meleeAttackRadius
        );
    }

    private void PerformMeleeHit()
    {
        if (!ctx.HasTarget)
            return;

        Transform originTransform = ctx.self;

        Vector3 targetPoint = GetTargetAimPoint(ctx.target, ctx.stats.meleeAttackHeightOffset);
        Vector3 origin = originTransform.position;

        Vector3 attackDir = targetPoint - origin;
        if (attackDir.sqrMagnitude < 0.0001f)
            attackDir = originTransform.forward;
        else
            attackDir.Normalize();

        Vector3 capsuleStart = origin + Vector3.up * ctx.stats.meleeAttackHeightOffset;
        Vector3 capsuleEnd = capsuleStart + attackDir * ctx.stats.meleeAttackRangeForward;

        int count = Physics.OverlapCapsuleNonAlloc(
            capsuleStart,
            capsuleEnd,
            ctx.stats.meleeAttackRadius,
            hitResults,
            ctx.stats.meleeHitMask,
            QueryTriggerInteraction.Ignore
        );

        for (int i = 0; i < count; i++)
        {
            Collider col = hitResults[i];
            if (!col)
                continue;

            if (col.transform.IsChildOf(ctx.self))
                continue;

            Transform otherRoot = col.transform.root;
            if (otherRoot == ctx.self.root)
                continue;

            if (otherRoot.GetComponent<EnemyBrain>() != null)
                continue;

            IDamageable damageable =
                col.GetComponentInParent<IDamageable>() ??
                col.GetComponent<IDamageable>() ??
                otherRoot.GetComponentInChildren<IDamageable>();

            if (damageable == null)
                continue;

            Vector3 samplePoint = (capsuleStart + capsuleEnd) * 0.5f;
            Vector3 hitPoint = col.ClosestPoint(samplePoint);
            Vector3 normal =
                (hitPoint - samplePoint).sqrMagnitude > 0.0001f
                    ? (hitPoint - samplePoint).normalized
                    : -attackDir;

            damageable.TakeDamage((int)(ctx.stats.damage * StatModManager.GetStatModifier(StatName.DOG_DAMAGE)), hitPoint, normal);
            
            return;
        }
    }

    private Vector3 ComputeDartOffset(Vector3 towardTarget)
    {
        if (ctx.stats.dartStep <= 0f)
            return Vector3.zero;

        if (dartDecisionTimer <= 0f)
        {
            dartDecisionTimer = Random.Range(ctx.stats.dartIntervalMin, ctx.stats.dartIntervalMax);

            if (Random.value < ctx.stats.dartChance)
                dartDir = Random.value < 0.5f ? -1 : 1;
        }

        Vector3 side = Vector3.Cross(Vector3.up, towardTarget).normalized;
        return side * dartDir * ctx.stats.dartStep;
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

    private static Vector3 GetTargetAimPoint(Transform target, float fallbackHeight)
    {
        if (target.TryGetComponent<CharacterController>(out var cc))
            return target.TransformPoint(cc.center);

        if (target.TryGetComponent<Collider>(out var col))
            return col.bounds.center;

        return target.position + Vector3.up * fallbackHeight;
    }

    private void SetAgentDestinationSafe(Vector3 desiredWorldPos)
    {
        if (!ctx.agent || !ctx.agent.enabled || !ctx.agent.isOnNavMesh)
            return;

        ctx.agent.isStopped = false;

        float sampleRadius = Mathf.Max(0.5f, ctx.stats.navSampleDistance);

        if (NavMesh.SamplePosition(desiredWorldPos, out NavMeshHit hit, sampleRadius, NavMesh.AllAreas))
        {
            ctx.agent.SetDestination(hit.position);
            return;
        }

        if (ctx.HasTarget && NavMesh.SamplePosition(ctx.target.position, out NavMeshHit targetHit, sampleRadius * 2f, NavMesh.AllAreas))
        {
            ctx.agent.SetDestination(targetHit.position);
            return;
        }

        if (NavMesh.SamplePosition(ctx.self.position, out NavMeshHit selfHit, sampleRadius, NavMesh.AllAreas))
        {
            ctx.agent.SetDestination(selfHit.position);
        }
    }
}