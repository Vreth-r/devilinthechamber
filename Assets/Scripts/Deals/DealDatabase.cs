using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(menuName = "Deals/Deal Database")]
public class DealDatabase : ScriptableObject
{
    public List<Deal> dealDeck;
}

