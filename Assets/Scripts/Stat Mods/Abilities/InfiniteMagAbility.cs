using Unity.VisualScripting;
using UnityEngine;

public class InfiniteMagAbility : AbilityBase
{
    public override void endFunction()
    {
        base.endFunction();
        AbilityModManager.abilityFlags[AbilityName.INFINITE_MAG] = false;
    }
}
