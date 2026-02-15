using UnityEngine;

public class Health : MonoBehaviour
{
    public int maxHealth = 100;
    int hp;
    public bool invincible = false;

    void Awake() => hp = maxHealth;

    public void TakeDamage(int amount)
    {
        if (!invincible)
            hp -= amount;
        if (hp <= 0)
        {
            Destroy(gameObject);
        }
    }
}
