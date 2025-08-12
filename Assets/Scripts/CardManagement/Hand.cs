using System.Collections.Generic;
using UnityEngine;

public class Hand
{
    public int size = 5;
    public readonly List<PlayCard> cards = new();

    private EncounterManager _encounter;

    public Hand(EncounterManager encounterManager)
    {
        _encounter = encounterManager;
    }

    public void Add(PlayCard card)
    {
        cards.Add(card);
    }

    public void DiscardHand()
    {
        foreach (var card in cards)
        {
            _encounter.deck.Discard(card);
        }

        cards.Clear();
    }
}
