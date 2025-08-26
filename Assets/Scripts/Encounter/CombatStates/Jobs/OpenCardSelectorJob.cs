using UnityEngine;
using UnityEngine.Events;

public class OpenCardSelectorJob : IStateJob
{
    public UnityEvent OnComplete { get; } = new();
    
    private EncounterManager em; // store a reference to the encountermanager because i'm straight up evil and unredeemable

    public OpenCardSelectorJob()
    {
        // mroeeewww
    }

    public void StartJob(EncounterManager em)
    {
        this.em = em;
        em.OpenCardSelector("Pick a card, any card", FinishJob);
    }

    public void FinishJob(PlayCard card)
    {
        Debug.Log($"your card is {card.Card.name}");
        // em.hand.Discard(card); // this is why we store a reference to the encountermanager
        // queue a discard card job?
        em.jobRunner.QueueJob(new DiscardCardJob(card));
        OnComplete?.Invoke();
    }
}
