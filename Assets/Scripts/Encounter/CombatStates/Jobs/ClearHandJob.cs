using UnityEngine;
using UnityEngine.Events;

public class ClearHandJob : IStateJob
{
    public UnityEvent OnComplete { get; } = new();

    Hand hand;
    HandDisplay handDisplay;

    public ClearHandJob(EncounterManager _em)
    {
        hand = _em.hand;
        handDisplay = _em.handDisplay;
    }

    public void StartJob()
    {
        hand.DiscardHand();
        handDisplay.ClearDisplay();

        OnComplete.Invoke();
    }
}