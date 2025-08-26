using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class CardExecutor
{
    public UnityEvent<PlayCard> OnCardExecuted { get; } = new();

    EncounterManager _encounterManager;

    public CardExecutor(EncounterManager encounterManager)
    {
        _encounterManager = encounterManager;
    }

    public void ExecuteCard(PlayCard card)
    {
        OnCardExecuted.Invoke(card);
    }
}
