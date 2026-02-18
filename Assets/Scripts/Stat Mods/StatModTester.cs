using UnityEngine;

public class StatModTester : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //AbilityModManager.StartAbility(AbilityName.FAKE_HIT_INDICATOR, 5);
        StatModManager.AddStatModifier(StatName.FIRE_SPEED, 1.25f);
        StatModManager.AddStatModifier(StatName.FIRE_SPEED, 1.25f);
        StatModManager.AddStatModifier(StatName.FIRE_SPEED, 1.25f);
        AbilityModManager.StartAbility(AbilityName.BLINDNESS, 5);
    }

    // Update is called once per frame
    void addReloadSpeed()
    {
        Debug.Log("eh");
        AbilityModManager.StartAbility(AbilityName.BLINDNESS, 5);
        //StatModManager.AddStatModifier(StatName.RELOAD_SPEED, DealType.NEGATIVE); 
    }
    void addFireRate()
    {
        StatModManager.AddStatModifier(StatName.FIRE_SPEED, 1.25f); 
    }
}
