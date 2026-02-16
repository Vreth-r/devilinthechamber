using UnityEngine;

public class ExplodingEnemiesAbility : AbilityBase
{
    public override void initialize (float duration)
    {
        abilityName = AbilityName.EXPLODING_ENEMIES;
        this.duration = duration;
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
