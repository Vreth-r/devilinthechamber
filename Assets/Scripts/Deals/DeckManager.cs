using UnityEngine;
using System.Collections.Generic;
using UnityEngine.AddressableAssets;
using Unity.VisualScripting;

public class DeckManager : MonoBehaviour
{
    public static DeckManager Instance;
    List<Deal> deck = new List<Deal>();
    public Deal deathCardPrefab;

    public int deathCardsInDeck = 0;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
    }
    void Start()
    {
        Addressables.LoadAssetsAsync<Deal>("DealCard", OnDealLoaded);
    }

    void OnDealLoaded(Deal deal)
    {
        deck.Add(deal);
        if (deal.abilityDeals.Count != 0 && deal.abilityDeals[0].AbilityName == AbilityName.DEATH)
            deathCardsInDeck++;

        Shuffle();
    }

    public List<Deal> DrawDeals ()
    {
        List<Deal> drawnDeals = new List<Deal>(deck.GetRange(0, 3));
        deck.RemoveRange(0, 3);
        return drawnDeals;
    }

    public void AddDeathCard (int count)
    {
        deathCardsInDeck += count;
        for (int i = 0; i < count; i++) deck.Add(Instantiate(deathCardPrefab));

        Shuffle();
    }

    void Shuffle ()
    {
        for (int i = deck.Count - 1; i > 0; i--)
        {
            int j = Random.Range(0, i + 1);
            Deal temp = deck[i];
            deck[i] = deck[j];
            deck[j] = temp;
        }
    }

}
