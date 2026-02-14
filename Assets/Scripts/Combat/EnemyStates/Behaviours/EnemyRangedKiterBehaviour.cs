using UnityEngine;

[CreateAssetMenu(menuName = "Enemies/Behaviours/Ranged Kiter")]
public class EnemyRangedKiterBehaviour : EnemyBehaviour
{
    public override IEnemyState CreateInitialState(EnemyContext ctx, EnemyStateMachine fsm)
        => new EnemyIdleState(ctx, fsm, nextOnAggro: () => new EnemyRangedKiteState(ctx, fsm));
}
