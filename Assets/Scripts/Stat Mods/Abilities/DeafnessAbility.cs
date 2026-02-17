using UnityEngine;

public class DeafnessAbility : AbilityBase
{
    private float normalGameplayVolume;
    public override void initialize (float duration)
    {
        abilityName = AbilityName.DEAFNESS;
        this.duration = duration;
    }
    public override bool startFunction()
    {
        normalGameplayVolume = AudioManager.Instance.gameplayVolume;
        AudioManager.Instance.gameplayVolume = 0f;
        Debug.Log($"START: {abilityName}");
        return true;
    }

    public override bool endFunction()
    {
        AudioManager.Instance.gameplayVolume = normalGameplayVolume;
        Debug.Log($"STOP: {abilityName}");
        return true;
    }
}


