using UnityEngine;

public class StatModTester : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Invoke(nameof(wrapper),1);
        //AbilityModManager.StartAbility(AbilityName.ONE_EYED, -1);
        //AbilityModManager.StartAbility(AbilityName.BLINKING, -1);
        AbilityModManager.StartAbility(AbilityName.BLINDNESS, -1);
    }

    void wrapper()
    {
        //StatModManager.AddStatModifier(StatName.HEADSHOT_BONUS, 3);
        //AbilityModManager.StartAbility(AbilityName.INVINCIBILITY, 10);
        //AbilityModManager.StartAbility(AbilityName.KNOCKBACK_ABILITY, 10);
//        AbilityModManager.StartAbility(AbilityName.FULL_AUTO, -1);
    }

}
