using UnityEngine;

[CreateAssetMenu(menuName = "Enemies/Behaviours/Ranged Kiter")]
public class EnemyRangedKiterBehaviour : EnemyBehaviour
{
    public override IEnemyState CreateInitialState(EnemyContext ctx, EnemyStateMachine fsm)
    {
        return new EnemyIdleState(ctx, fsm, () => new EnemyRangedKiteState(ctx, fsm));
    }
}