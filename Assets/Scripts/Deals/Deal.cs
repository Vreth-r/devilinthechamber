using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class StatMod
{
    public StatName statName;
    public float modifier;
    public DealType dealType;
}
[System.Serializable]
public class Ability
{
    public AbilityName AbilityName;
    public float duration;
    public DealType dealType;
}

[CreateAssetMenu(menuName = "Deals/Deal")]
public class Deal : ScriptableObject
{
    public List<StatMod> statDeals;
    public List<Ability> abilityDeals;

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