using UnityEngine;

public enum EnemyType
{
    LADY,
    DOG
}

[CreateAssetMenu(menuName = "Enemies/Enemy Stats")]
public class EnemyStats : ScriptableObject
{
    [Header("Core")]
    public EnemyType type = EnemyType.LADY;
    public float maxHealth = 100f;
    public int damage = 10;

    [Header("Detection")]
    public float aggroRange = 25f;
    public float leashRange = 30f;

    [Header("Movement")]
    public float moveSpeed = 3.5f;
    public float stopRange = 1.8f;
    public float repathRateHz = 10f;

    [Header("Facing")]
    public bool faceTargetWhenStopped = true;
    public float faceTurnSpeed = 12f;

    [Header("Melee State Timing")]
    public float attackRange = 2.0f;
    public float meleeWindupTime = 0.0f;
    public float meleeCooldownTime = 0.5f;
    public float meleeAttackMoveStoppingDistance = 0f;

    [Header("Melee Hit")]
    public float meleeAttackRadius = 1.2f;
    public float meleeAttackRangeForward = 0.6f;
    public float meleeAttackHeightOffset = 1.0f;

    [Header("Melee Lunge")]
    public float meleeLungeStartRange = 1.8f;
    public float meleeLungeDistance = 1.2f;
    public float meleeLungeTime = 0.12f;
    public float meleeLungeHitTime = 0.06f;
    public float meleeCommitRangePadding = 0.6f;

    [Header("Melee Close Reposition")]
    public float meleeMinSeparationDistance = 0.9f;
    public float meleeBackstepDistance = 0.8f;
    public float meleeBackstepTime = 0.12f;

    [Header("Melee Hit Filtering")]
    public LayerMask meleeHitMask = ~0;

    [Header("Ranged Combat")]
    public float fireRate = 3f;
    public float projectileSpeed = 28f;
    public float initialShotDelayMultiplier = 1f;
    public float projectileAimHeight = 1.2f;
    public bool leadTarget = true;
    public float maxLeadTime = 0.75f;
    public float fallbackLeadBlend = 0.35f;
    public float velocitySmoothing = 12f;

    [Header("Ranged Fire Rules")]
    public float rangedFireMinRange = 6f;
    public float rangedFireMaxRange = 14f;

    [Header("Ranged Dodge / Dart")]
    public float dartStep = 4f;
    public float dartIntervalMin = 0.4f;
    public float dartIntervalMax = 1.1f;
    [Range(0f, 1f)] public float dartChance = 0.35f;

    [Header("Separation")]
    public float allySeparationRadius = 2.5f;
    public float allySeparationStrength = 2.5f;
    public LayerMask allySeparationMask = ~0;

    [Header("Ranged Spacing")]
    public float preferredRange = 12f;
    public float rangeTolerance = 2f;

    [Header("Projectile Lifetime")]
    public float projectileMaxTravelDistance = 30f;
    public float projectileLifetimeSafetyBuffer = 0.25f;

    [Header("Ranged Kiting")]
    public float orbitStep = 10f;
    public float retreatStep = 11f;
    public float approachStep = 6f;
    public float orbitRecalcHz = 6f;
    [Range(0f, 1f)] public float orbitFlipChance = 0.15f;
    public float orbitRadialBiasNear = 1.2f;
    public float orbitRadialBiasFar = -1.2f;

    [Header("Navigation Sampling")]
    public float navSampleDistance = 2.5f;

    [Header("Animation")]
    public string animParamAttackTrigger = "Attack";
    public string animParamWindupTrigger = "Windup";
    public string animParamRecoverTrigger = "Recover"; // fuckin, tutorials man
    public string animParamWinddownTrigger = "Winddown";
    public string animParamSpeed = "Speed";
    public bool useWindupAnimation = true;
    public bool useRecoverAnimation = false;
}