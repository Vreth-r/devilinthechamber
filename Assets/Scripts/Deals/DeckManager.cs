using UnityEngine;
using System.Collections.Generic;
using Unity.Mathematics;

public class DeckManager : MonoBehaviour
{
    public static DeckManager Instance;
    public List<int> pickedIndexes = new List<int>();

    public DealDatabase dealDatabase;

    void Awake()
    {
        if (Instance == null)
            Instance = this;

        if (dealDatabase == null)
            Debug.Log("NO DEAL DATABASE");
    }

    public List<Deal> GetRandomDeals ()
    {
        List<Deal> deals = new List<Deal>();
        if (pickedIndexes.Count == dealDatabase.dealDeck.Count)
            pickedIndexes.Clear();

        int currentInd = 0;

        for (int i = 0; i < 3; i++)
        {
            int indexInc = UnityEngine.Random.Range(0, dealDatabase.dealDeck.Count - 1);
            currentInd = (currentInd + indexInc) % dealDatabase.dealDeck.Count;
            while (pickedIndexes.Contains(currentInd))
            {
                currentInd += 1;
            }
            pickedIndexes.Add(currentInd);
            deals.Add(dealDatabase.dealDeck[currentInd]);
        }

        return deals;
    }
}
