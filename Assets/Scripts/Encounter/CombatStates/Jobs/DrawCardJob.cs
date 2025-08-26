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

    public DrawCardJob(int numCardsDrawn)
    {
        amount = numCardsDrawn;
        // deck = _em.deck;
        // hand = _em.hand;
        // amount = _em.hand.size;
        // deckDisplay = _em.deckDisplay;
        // handDisplay = _em.handDisplay;
        
        // moved all the above Stuff to the StartJob method :)
    }

    public void StartJob(EncounterManager encounterManager)
    {
        deck = encounterManager.deck;
        hand = encounterManager.hand;
        // amount = encounterManager.hand.size;
        deckDisplay = encounterManager.deckDisplay;
        handDisplay = encounterManager.handDisplay;
        
        // TODO: Play animations!
        
        Debug.Log(amount);
        for (int i = 0; i < amount; i++)
        {
            PlayCard drawnCard = deck.Draw();
            
            Debug.Log($"drawing card {drawnCard}");
            Debug.Log($"{deck.cardsInDeck.Count} cards in deck");

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
        handDisplay.ClearDisplay(); // without this line it stacks the displayed decks atop eachother
        handDisplay.DisplayHand(hand);

        OnComplete.Invoke();
    }
}
