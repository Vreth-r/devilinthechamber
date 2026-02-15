using UnityEngine;

public class StatModTester : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Invoke(nameof(addStat), 1f);
        //AbilityModManager.Instance.StartAbility(AbilityName.AOE_RELOAD);
        //StatModManager.Instance.AddStatModifier(StatName.MOVEMENT_SPEED, -1f);
        //StatModManager.Instance.AddStatModifier(StatName.MOVEMENT_SPEED, 3f);
//
        //AbilityModManager.Instance.StartAbility(AbilityName.INVINCIBILITY);
        //AbilityModManager.Instance.StartAbility(AbilityName.BLINDNESS);
    }

    // Update is called once per frame
    void addStat()
    {
        StatModManager.Instance.AddStatModifier(StatName.JUMP_HEIGHT, 2f); 
    }


}
