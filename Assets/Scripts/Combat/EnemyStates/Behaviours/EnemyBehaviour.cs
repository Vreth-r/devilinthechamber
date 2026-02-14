using UnityEngine;

public abstract class EnemyBehaviour : ScriptableObject
{
    public abstract IEnemyState CreateInitialState(EnemyContext ctx, EnemyStateMachine fsm);
}