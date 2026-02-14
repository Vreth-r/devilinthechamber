using UnityEngine;

public class EnemyStateMachine
{
    public IEnemyState Current { get; private set; }

    public void SetState(IEnemyState next)
    {
        if (Current == next) return;

        Current?.Exit();
        Current = next;
        Current?.Enter();
        Debug.Log(next);
    }

    public void Tick(float dt)
    {
        Current?.Tick(dt);
    }
}