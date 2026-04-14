using System.Threading.Tasks;
using UnityEngine;
public class DeathAbility : AbilityBase
{
    public override void startFunction()
    {
        base.startFunction();
        _ = Die();
    }
    async Task Die()
    {
        if (AbilityModManager.abilityFlags[AbilityName.SURVIVOR]) return;
        GameManager.Instance.StopMusic();
        await SceneFader.Instance.FadeToScene("GameOver");
    }
}
