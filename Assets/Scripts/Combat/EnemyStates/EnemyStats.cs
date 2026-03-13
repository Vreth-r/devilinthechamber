using UnityEngine;

[CreateAssetMenu(menuName = "Enemies/Enemy Stats")]
public class EnemyStats : ScriptableObject
{
    [Header("Core")]
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

    [Header("Melee")]
    public float attackRange = 2.0f;
    public float meleeWindupTime = 0.0f;
    public float meleeCooldownTime = 0.5f;
    public float meleeAttackMoveStoppingDistance = 0f;

    [Header("Ranged Combat")]
    public float fireRate = 3f;
    public float projectileSpeed = 20f;
    public float initialShotDelayMultiplier = 1f;

    [Header("Ranged Spacing")]
    public float preferredRange = 12f;
    public float rangeTolerance = 2f;

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
}