using UnityEngine;

public class BulletRestoreAbility : AbilityBase
{
    public override void initialize ()
    {
        abilityName = AbilityName.KILL_BULLET_RESTORE; // cringe but whatever
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
