using UnityEngine;
using System.Collections.Generic;
using Unity.Mathematics;

public class DeckManager : MonoBehaviour
{
    public static DeckManager Instance;
    public List<int> pickedIndexesTier1 = new List<int>();
    public List<int> pickedIndexesTier2 = new List<int>();
    public List<int> pickedIndexesTier3 = new List<int>();
    public Deal deathCard;

    public DealDatabase tier1Deals;
    public DealDatabase tier2Deals;
    public DealDatabase tier3Deals;

    public int dealsCount = 0;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
    }

    public List<Deal> GetRandomDeals ()
    {
        dealsCount += 1;

        List<Deal> deals = new List<Deal>();
        switch (dealsCount) {
            case <= 2:
                for (int i = 0; i < 3; i++)
                {
                    int k = UnityEngine.Random.Range(0, tier1Deals.dealDeck.Count - 1);
                    while (pickedIndexesTier1.Contains(k))
                    {
                        k = (k + 1) % tier1Deals.dealDeck.Count;
                    }
                    deals.Add(tier1Deals.dealDeck[k]);
                    pickedIndexesTier1.Add(k);
                }
                break;
            case <= 4:
                for (int i = 0; i < 3; i++)
                {
                    int k = UnityEngine.Random.Range(0, tier2Deals.dealDeck.Count - 1);
                    while (pickedIndexesTier2.Contains(k))
                    {
                        k = (k + 1) % tier2Deals.dealDeck.Count;
                    }
                    deals.Add(tier2Deals.dealDeck[k]);
                    pickedIndexesTier2.Add(k);
                }
                break;
            default:
                for (int i = 0; i < math.min(9 - dealsCount, 3); i++)
                {
                    int k = UnityEngine.Random.Range(0, tier3Deals.dealDeck.Count - 1);
                    while (pickedIndexesTier3.Contains(k))
                    {
                        k = (k + 1) % tier3Deals.dealDeck.Count;
                    }
                    deals.Add(tier3Deals.dealDeck[k]);
                    pickedIndexesTier3.Add(k);
                }
                while (deals.Count < 3)
                {
                    deals.Add(deathCard);
                }

                break;
        }

        return deals;
    }
}
