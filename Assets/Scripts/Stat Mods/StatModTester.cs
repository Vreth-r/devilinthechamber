using UnityEngine;

public class StatModTester : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StatModManager.AddStatModifier(StatName.LADY_FIRE_RATE, 3);
        StatModManager.AddStatModifier(StatName.LADY_MOVEMENT_SPEED, 3);
        StatModManager.AddStatModifier(StatName.LADY_PROJECTILE_SPEED, 3);
        //AbilityModManager.StartAbility(AbilityName.INFINITE_MAG, 3);
    }


}
