using System.Collections.Generic;
using UnityEngine;

public class CardEffectManager
{
    private EncounterManager _encounterManager;

    public CardEffectManager(EncounterManager _em)
    {
        _encounterManager = _em;
    }
    
    // probably temporary, replace with whatever suits your fancy but this should be called when a card is for sure being played
    // (it doesn't destroy the card or anything so be careful)
    public void PlayCard(Card card)
    {
        foreach (CardEffect effect in card.effects)
        {
            List<IStateJob> cardJobs = effect.GetJobs();
            foreach (IStateJob cardJob in cardJobs)
            {
                _encounterManager.jobRunner.QueueJob(cardJob);
            }
        }
    }
}
