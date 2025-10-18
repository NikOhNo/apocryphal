using UnityEngine;
using UnityEngine.Events;

public class SealCardJob : IStateJob
{
    private PlayCard playCard;

    public SealCardJob(PlayCard playCard)
    {
        this.playCard = playCard;
        playCard.isSealed = true;
    }

    public UnityEvent OnComplete { get; } = new();

    public void StartJob(EncounterManager encounterManager)
    {
        encounterManager.deck.Seal(playCard);
        encounterManager.deckDisplay.UpdateDisplay();
        OnComplete.Invoke();
    }
}
