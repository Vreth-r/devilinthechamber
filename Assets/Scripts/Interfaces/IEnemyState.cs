public interface IEnemyState
{
    string Name { get; }
    void Enter();
    void Tick(float dt);
    void Exit();
}