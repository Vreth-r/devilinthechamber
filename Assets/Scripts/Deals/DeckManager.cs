using UnityEngine;
using System.Collections.Generic;
using UnityEngine.AddressableAssets;
using FMOD;

public class DeckManager : MonoBehaviour
{
    public static DeckManager Instance;
    List<Deal> deck = new List<Deal>();
    public List<Deal> chosenDeals = new List<Deal>();
    public Deal deathCardPrefab;
    
    public float cardFlipChance = 0.0f;
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

    public void OnDealLoaded(Deal deal)
    {
        deck.Add(deal);
        if (deal.abilityDeals.Count != 0 && deal.abilityDeals[0].AbilityName == AbilityName.DEATH)
            deathCardsInDeck++;

        Shuffle();
    }

    public List<Deal> DrawDeals ()
    {
        List<Deal> drawnDeals = new List<Deal>();
        List<int> removeInds = new List<int>();
        int deathCardsInHand = 0;
        int i = 0;

        // weird
        while (i < deck.Count && removeInds.Count < 3)
        {
            if (deck[i].dealName == "Death") deathCardsInHand += 1;
            if (AbilityModManager.abilityFlags[AbilityName.INSATIABLE_GREED] && deathCardsInHand == 1 && deck.Count - deathCardsInDeck >= 2)
            {
                StatModManager.AddStatModifier(StatName.FLIPPED_CARD_CHANCE, 0.05f);
            }
            else 
            {
                drawnDeals.Add(deck[i]);
                removeInds.Add(i);   
            }
            i++;
        }
        for (int k = removeInds.Count - 1; k >= 0; k--) deck.RemoveAt(k);

        return drawnDeals;
    }

    public void AddDeathCard (int count)
    {
        deathCardsInDeck += count;
        for (int i = 0; i < count; i++) deck.Add(Instantiate(deathCardPrefab));

        if (deck.Count < 3) deck.Add(Instantiate(deathCardPrefab));

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

    public void AddToChosenDeals (Deal deal)
    {
        chosenDeals.Add(deal);
    }

    public void RemoveLastChosenDeal ()
    {
        if (chosenDeals.Count == 0) return;
        Deal d = chosenDeals[chosenDeals.Count - 1];

        foreach (StatMod statDeal in d.statDeals)
        {
            StatModManager.RemoveStatModifierExact(statDeal.statName, statDeal.modifier);
        }

        foreach (Ability abilityDeal in d.abilityDeals)
        {
            if (AbilityModManager.abilities.ContainsKey(abilityDeal.AbilityName)) AbilityModManager.abilities[abilityDeal.AbilityName].endFunction();
            AbilityModManager.abilityFlags[abilityDeal.AbilityName] = false;
        }
        UnityEngine.Debug.Log($"Removed {d.dealName}");
        chosenDeals.RemoveAt(chosenDeals.Count - 1);
    }
    
    public void RemoveDeathCard ()
    {
        for (int i = 0; i < deck.Count; i++)
        {
            if (deck[i].dealName == "Death")
            {
                deck.RemoveAt(i);
                deathCardsInDeck -= 1;
                return;
            }
        }
    }

}
