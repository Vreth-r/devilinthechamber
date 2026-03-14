// using UnityEngine;

// [CreateAssetMenu(menuName = "Enemies/Behaviours/Melee Chaser")]
// public class EnemyMeleeBehaviour : EnemyBehaviour
// {
//     public override IEnemyState CreateInitialState(EnemyContext ctx, EnemyStateMachine fsm)
//     {
//         return new EnemyIdleState(ctx, fsm, () => new EnemyMeleeChaseState(ctx, fsm));
//     }
// }