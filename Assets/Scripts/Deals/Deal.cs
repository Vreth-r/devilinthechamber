using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class StatMod
{
    public StatName statName;
    public DealType dealType;
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
    public List<StatMod> statDeals;
    public List<Ability> abilityDeals;

    public void ApplyDeal ()
    {
        foreach (StatMod statDeal in statDeals)
        {
            StatModManager.AddStatModifier(statDeal.statName, statDeal.dealType);
        }

        foreach (Ability abilityDeals in abilityDeals)
        {
            AbilityModManager.StartAbility(abilityDeals.AbilityName, abilityDeals.duration);
        }
    }
}

[CreateAssetMenu(menuName = "Deals/Deal Database")]
public class DealDatabase : ScriptableObject
{
    public List<Deal> dealDeck;
}
