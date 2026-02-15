using UnityEngine;

public class InvincibilityAbility : AbilityBase
{
    public override void initialize ()
    {
        abilityName = AbilityName.INVINCIBILITY;
    }
    public override bool startFunction()
    {
        PlayerScriptRefHolder.Instance.health.invincible = true;
        Debug.Log($"START: {abilityName}");
        return true;
    }

    public override bool endFunction()
    {
        PlayerScriptRefHolder.Instance.health.invincible = false;
        Debug.Log($"STOP: {abilityName}");
        return true;
    }
}
