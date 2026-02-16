using UnityEngine;

public class BulletRestoreAbility : AbilityBase
{
    public override void initialize (float duration)
    {
        abilityName = AbilityName.KILL_BULLET_RESTORE;
        this.duration = duration;
    }
    public override bool startFunction()
    {
        GameManager.Instance.bulletRestoreMod = true;
        Debug.Log($"START: {abilityName}");
        return true;
    }

    public override bool endFunction()
    {
        GameManager.Instance.bulletRestoreMod = false;
        Debug.Log($"STOP: {abilityName}");
        return true;
    }
}
