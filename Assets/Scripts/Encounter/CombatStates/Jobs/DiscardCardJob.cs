using Scripts.Deck;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class DiscardCardJob : IStateJob
{
    public UnityEvent OnComplete { get; } = new();

    Deck deck;
    DeckDisplay deckDisplay;
    Hand hand;
    HandDisplay handDisplay;
    PlayCard cardToDiscard;

    public DiscardCardJob(PlayCard card)
    {
        cardToDiscard = card;
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
        
        hand.Discard(cardToDiscard);

        deckDisplay.UpdateDisplay();
        handDisplay.ClearDisplay(); // without this line it stacks the displayed hands atop eachother
        handDisplay.DisplayHand(hand);

        OnComplete.Invoke();
    }
}
