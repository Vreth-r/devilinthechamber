using UnityEngine;

public class StatModTester : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //addStat();
        Invoke(nameof(addReloadSpeed), 5f);
        //Invoke(nameof(addFireRate), 5f);
        //AbilityModManager.Instance.StartAbility(AbilityName.AOE_RELOAD);
        //StatModManager.Instance.AddStatModifier(StatName.MOVEMENT_SPEED, -1f);
        //StatModManager.Instance.AddStatModifier(StatName.MOVEMENT_SPEED, 3f);

        //AbilityModManager.Instance.StartAbility(AbilityName.PHANTOM_NOISES);
        //AbilityModManager.Instance.StartAbility(AbilityName.BLINDNESS);
    }

    // Update is called once per frame
    void addReloadSpeed()
    {
        StatModManager.Instance.AddStatModifier(StatName.RELOAD_SPEED, 1.5f); 
    }
    void addFireRate()
    {
        StatModManager.Instance.AddStatModifier(StatName.FIRE_SPEED, 4f); 
    }
}
