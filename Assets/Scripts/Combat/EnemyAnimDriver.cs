using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(Animator))]
public class EnemyAnimDriver : MonoBehaviour
{
    public NavMeshAgent agent;
    public bool debug;

    Animator anim;

    static readonly int MoveSpeedHash = Animator.StringToHash("MoveSpeed");
    static readonly int IsFiringHash  = Animator.StringToHash("IsFiring");

    void Awake()
    {
        anim = GetComponent<Animator>();
        if (!agent) agent = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        float speed = agent ? agent.velocity.magnitude : 0f;
        anim.SetFloat(MoveSpeedHash, speed);
    }

    public void SetFiring(bool firing)
    {
        anim.SetBool(IsFiringHash, firing);
        if (debug) Debug.Log($"SetFiring({firing})");
    }
}
