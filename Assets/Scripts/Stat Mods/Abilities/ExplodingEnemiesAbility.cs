using UnityEngine;

public class ExplodingEnemiesAbility : AbilityBase
{
    public override void initialize ()
    {
        abilityName = AbilityName.EXPLODING_ENEMIES;
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
