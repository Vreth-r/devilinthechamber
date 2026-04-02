using UnityEngine;

public class InvincibilityAbility : AbilityBase
{
    public override void endFunction()
    {
        base.endFunction();
        AbilityModManager.abilityFlags[AbilityName.INVINCIBILITY] = false;
    }
}
