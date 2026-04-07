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
        GameManager.Instance.gameOverFlag = true;
    }
}
