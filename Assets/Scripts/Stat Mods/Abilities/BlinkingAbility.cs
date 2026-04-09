using UnityEngine;
using System.Collections;
//using UnityEditor.ShaderGraph.Internal;

public class BlinkingAbility : AbilityBase
{
    public override void startFunction()
    {
        //float t = CalculateBlinkTime();
        float t = Random.Range(0.5f, 2.5f);
        TimerHandler.Instance.CreateTimerHandle(abilityName.ToString(), t, DoBlink);
        base.startFunction();
    }
    void DoBlink ()
    {
        UIEvents.DoBlink();
        //float t = CalculateBlinkTime();
        float t = Random.Range(0.5f, 2.5f);
        TimerHandler.Instance.CreateTimerHandle(abilityName.ToString(), t, DoBlink);
    }

    float CalculateBlinkTime ()
    {
        float minBlinkInterval = 0.5f;
        float maxBlinkInterval = 2.5f;
        float speedPerc;
        if (PlayerManager.Instance.playerMotor.GetComponent<CharacterController>().isGrounded)
            speedPerc = PlayerManager.Instance.playerMotor.PlanarVelocity.magnitude / PlayerManager.Instance.playerMotor.maxGroundSpeed;
        else
            speedPerc = PlayerManager.Instance.playerMotor.PlanarVelocity.magnitude / PlayerManager.Instance.playerMotor.maxAirSpeed;

        speedPerc = 1 - speedPerc;
        return Mathf.Lerp(minBlinkInterval, maxBlinkInterval, speedPerc);
        
    }
}
