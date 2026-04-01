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
        StatModManager.ResetStatMods();
        GameManager.Instance.StopMusic();
        Object.Destroy(GameManager.Instance.gameObject);
        await SceneFader.Instance.FadeToScene("GameOver");
        return;
    }
}
