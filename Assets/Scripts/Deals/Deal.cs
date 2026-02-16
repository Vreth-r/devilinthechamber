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

}
