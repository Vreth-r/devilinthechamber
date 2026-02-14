using UnityEngine;

public class InvincibilityAbility : AbilityBase
{
    public override void initialize ()
    {
        abilityName = AbilityName.INVINCIBILITY; // cringe but whatever
    }
    public override bool startFunction()
    {
        Debug.Log($"START: {abilityName}");
        return true;
    }

    public override bool endFunction()
    {
        Debug.Log($"STOP: {abilityName}");
        return true;
    }
}
