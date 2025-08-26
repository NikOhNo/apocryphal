using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;

public class CardClickListener : MonoBehaviour
{
    // temporary very verry bad singleton for listening for when cards (buttons) are clicked
    
    public static CardClickListener Instance { get; private set; }
    [SerializeField] EncounterManager _encounterManager;

    public void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this);
        }
    }
    
    // function called by CardDisplay(s) when they're clicked. it doesn't do ANY verification on whether the card is legal to be played
    public void OnClickCard(PlayCard playCard, CardDisplay cd)
    {
        if (_encounterManager.CurrentState.StateType != StateType.PlayerTurn) return; // don't do anything if it's not time for the player to play
        _encounterManager.cardEffectManager.PlayCard(playCard.Card);
        _encounterManager.hand.Discard(playCard); // discard the card immediately when it's played. consider queueing a DiscardJob for the card instead 
        Destroy(cd);
        _encounterManager.handDisplay.ClearDisplay();
        _encounterManager.handDisplay.DisplayHand(_encounterManager.hand);
        _encounterManager.deckDisplay.UpdateDisplay();
    }
}
