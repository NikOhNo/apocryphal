using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;

public class CardPerformer
{
    public CardPerformer(EncounterManager em)
    {
        _encounterManager = em;
    }

    EncounterManager _encounterManager;

    // function called by CardDisplay(s) when they're clicked.
    public void OnClickCard(PlayCard playCard, CardDisplay cd)
    {
        if (_encounterManager.CurrentState.StateType != StateType.PlayerTurn) return; // don't do anything if it's not time for the player to play
        if (_encounterManager.playerMPAP.UseCard(playCard))
        {
            PerformCard(playCard, cd);
        }
        else
        {
            ReturnCard(cd);
        }
    }

    private void PerformCard(PlayCard playCard, CardDisplay cd)
    {
        _encounterManager.cardEffectManager.PlayCard(playCard);
        _encounterManager.hand.Discard(playCard); // discard the card immediately when it's played. consider queueing a DiscardJob for the card instead 
        GameObject.Destroy(cd.gameObject);
        _encounterManager.handDisplay.ClearDisplay();
        _encounterManager.handDisplay.DisplayHand(_encounterManager.hand);
        _encounterManager.deckDisplay.UpdateDisplay();
    }

    private void ReturnCard(CardDisplay cd)
    {
        _encounterManager.handDisplay.AttachCard(cd);
    }
}
