using UnityEngine;

public class EnemyAnimDriver : MonoBehaviour
{
    private Animator animator;
    private EnemyBrain brain;
    private EnemyStats stats;

    private void Awake()
    {
        animator = GetComponentInChildren<Animator>();
        brain = GetComponent<EnemyBrain>();
        stats = brain ? brain.stats : null;
    }

    private void RefreshStats()
    {
        if (!brain) brain = GetComponent<EnemyBrain>();
        if (brain) stats = brain.stats;
    }

    public void PlayWindUp()
    {
        RefreshStats();
        if (!animator || stats == null) return;
        if (!string.IsNullOrEmpty(stats.animParamWindupTrigger))
            animator.SetTrigger(stats.animParamWindupTrigger);
    }

    public void PlayShoot()
    {
        RefreshStats();
        if (!animator || stats == null) return;
        if (!string.IsNullOrEmpty(stats.animParamAttackTrigger))
            animator.SetTrigger(stats.animParamAttackTrigger);
    }

    public void PlayWindDown()
    {
        RefreshStats();
        if (!animator || stats == null) return;
        if (!string.IsNullOrEmpty(stats.animParamWinddownTrigger))
            animator.SetTrigger(stats.animParamWinddownTrigger);
    }

    public void SetSpeed(float speed)
    {
        RefreshStats();
        if (!animator || stats == null) return;
        if (!string.IsNullOrEmpty(stats.animParamSpeed))
            animator.SetFloat(stats.animParamSpeed, speed);
    }
}