using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(Animator))]
public class EnemyMeleeAnimDriver : MonoBehaviour
{
    public NavMeshAgent agent;
    public string moveSpeedParam = "MoveSpeed";
    public float dampTime = 0.08f;

    Animator anim;
    int moveSpeedHash;

    void Awake()
    {
        anim = GetComponent<Animator>();
        moveSpeedHash = Animator.StringToHash(moveSpeedParam);
        if (!agent) agent = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        float speed = agent ? agent.velocity.magnitude : 0f;
        anim.SetFloat(moveSpeedHash, speed, dampTime, Time.deltaTime);
    }
}