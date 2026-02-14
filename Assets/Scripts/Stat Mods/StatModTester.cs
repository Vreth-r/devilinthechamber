using UnityEngine;

public class StatModTester : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StatModManager.Instance.AddStatModifier(StatName.MOVEMENT_SPEED, 1f);
        StatModManager.Instance.AddStatModifier(StatName.MOVEMENT_SPEED, -1f);
        StatModManager.Instance.AddStatModifier(StatName.MOVEMENT_SPEED, 3f);

        Debug.Log(StatModManager.Instance.GetStatModsRaw(StatName.MOVEMENT_SPEED));
        Debug.Log(StatModManager.Instance.GetTotalStatMod(StatName.MOVEMENT_SPEED));

        AbilityModManager.Instance.StartAbility(AbilityName.INVINCIBILITY);
        AbilityModManager.Instance.StartAbility(AbilityName.BLINDNESS);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
