using UnityEngine;

public class FullAutoAbility : AbilityBase
{
    public override void endFunction()
    {
        AbilityModManager.abilityFlags[AbilityName.FULL_AUTO] = false;
        base.endFunction();
    }
}
