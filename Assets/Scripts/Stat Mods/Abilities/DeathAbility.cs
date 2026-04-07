using UnityEngine;
public class DeathAbility : AbilityBase
{
    public override void startFunction()
    {
        base.startFunction();
        Die();
    }
    void Die()
    {
        if (AbilityModManager.abilityFlags[AbilityName.SURVIVOR]) return;
        StatModManager.ResetStatMods();
        AbilityModManager.ResetAbilities();
        GameManager.Instance.StopMusic();
        SceneFader.Instance.FadeToScene("GameOver");
    }
}
