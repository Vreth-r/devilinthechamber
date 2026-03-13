public interface IDamageable
{
    void TakeDamage(int amount, UnityEngine.Vector3 hitPoint, UnityEngine.Vector3 hitNormal);
    void Stun(float time);
}