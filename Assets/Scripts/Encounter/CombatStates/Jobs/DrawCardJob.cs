using Scripts.Deck;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class DrawCardJob : IStateJob
{
    public UnityEvent OnComplete { get; } = new();

    Deck deck;
    DeckDisplay deckDisplay;
    Hand hand;
    HandDisplay handDisplay;
    int amount;

    public DrawCardJob(EncounterManager _em)
    {
        deck = _em.deck;
        hand = _em.hand;
        amount = _em.hand.size;
        deckDisplay = _em.deckDisplay;
        handDisplay = _em.handDisplay;
    }

    public void StartJob()
    {
        // TODO: Play animations!

        for (int i = 0; i < amount; i++)
        {
            PlayCard drawnCard = deck.Draw();

            if (drawnCard != null)
            {
                hand.Add(drawnCard);
            }
            else
            {
                if (deck.cardsInDiscard.Count > 0)
                {
                    deck.ShuffleCards();
                    hand.Add(deck.Draw());
                }
            }
        }

        deckDisplay.UpdateDisplay();
        handDisplay.DisplayHand(hand);

        OnComplete.Invoke();
    }
}
