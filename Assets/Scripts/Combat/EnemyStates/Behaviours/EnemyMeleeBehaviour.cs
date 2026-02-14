using UnityEngine;

[CreateAssetMenu(menuName = "Enemies/Behaviours/Melee Chaser")]
public class EnemyMeleeBehaviour : EnemyBehaviour
{
    public override IEnemyState CreateInitialState(EnemyContext ctx, EnemyStateMachine fsm)
        => new EnemyIdleState(ctx, fsm, nextOnAggro: () => new EnemyMeleeChaseState(ctx, fsm));
}