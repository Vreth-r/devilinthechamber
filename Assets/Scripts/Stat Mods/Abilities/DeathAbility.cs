using UnityEngine;
public class DeathAbility : AbilityBase
{
    public override void initialize(float duration)
    {
        abilityName = AbilityName.DEATH;
        this.duration = -1;
    }

    public override bool startFunction()
    {
        Die();
        return true;
    }
    async void Die()
    {
        StatModManager.ResetStatMods();
        GameManager.Instance.StopMusic();
        Object.Destroy(GameManager.Instance.gameObject);
        await SceneFader.Instance.FadeToScene("GameOver");
        return;
    }

    public override bool endFunction()
    {
        return true;
    }
}
