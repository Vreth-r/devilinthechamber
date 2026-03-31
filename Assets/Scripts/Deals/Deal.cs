using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class StatMod
{
    public StatName statName;
    public float modifier;
}
[System.Serializable]
public class Ability
{
    public AbilityName AbilityName;
    public float duration;
}

[CreateAssetMenu(menuName = "Deals/Deal")]
public class Deal : ScriptableObject
{
    public string dealName;
    public string dealDescription;
    public List<StatMod> statDeals;
    public List<Ability> abilityDeals;
    public List<DrawCondition> drawConditions;

    public void ApplyDeal ()
    {
        foreach (StatMod statDeal in statDeals)
        {
            StatModManager.AddStatModifier(statDeal.statName, statDeal.modifier);
        }

        foreach (Ability abilityDeal in abilityDeals)
        {
            AbilityModManager.StartAbility(abilityDeal.AbilityName, abilityDeal.duration);
        }
    }
}