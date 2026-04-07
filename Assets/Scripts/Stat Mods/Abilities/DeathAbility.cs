using UnityEngine;
public class DeathAbility : AbilityBase
{
    public override void startFunction()
    {
        base.startFunction();
        Die();
    }
    async void Die()
    {
        if (AbilityModManager.abilityFlags[AbilityName.SURVIVOR]) return;
        StatModManager.ResetStatMods();
        AbilityModManager.ResetAbilities();
        GameManager.Instance.StopMusic();
        Object.Destroy(GameManager.Instance.gameObject);
        await SceneFader.Instance.FadeToScene("GameOver");
        return;
    }
}
