using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class StatMod
{
    public StatName statName;
    public float percent;
}

[CreateAssetMenu(menuName = "Deals/Deal")]
public class Deal : ScriptableObject
{
    public List<StatMod> statDeals;
    public List<AbilityName> abilityDeals;
    

}
