using UnityEngine;

public class Health : MonoBehaviour
{
    public int maxHealth = 100;
    int hp;

    void Awake() => hp = maxHealth;

    public void TakeDamage(int amount)
    {
        hp -= amount;
        if (hp <= 0)
        {
            Destroy(gameObject);
        }
    }
}
