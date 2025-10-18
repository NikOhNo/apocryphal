using Scripts.Deck;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.PlayerLoop;
using UnityEngine.UI;

public class EncounterManager : MonoBehaviour
{
    public readonly PlayerMPAP playerMPAP = new();
    public MPAPDisplay MPAPDisplay;
    public Hand hand;
    public HandDisplay handDisplay;
    public Deck deck;
    public DeckDisplay deckDisplay;
    public RoundCounter roundCounter;
    public ButtonDisplay endTurnButton;
    public ButtonDisplay addStatusButton;
    public DebugAddStatusButton addStatusPickerButton;
    public StateDisplay stateDisplay;
    
    public Player player; // ref to player.... good maybe probably
    
    public StateTransitionHandler transitionHandler;

    
    // FIXME extremely temporary :)
    
    public CardSelectInterface cardSelector;
    
    public Button startEncounterButton;
    
    public JobRunner jobRunner;
    public EnemyManager enemyManager;
    public CardEffectManager cardEffectManager;
    
    public DragHandler dragHandler;
    
    // data for the encounter, containing its enemies and such
    public EncounterData encounterData;

    private CardPerformer cardPerfomer;
    
    public ICombatState CurrentState { get; private set; }
    
    public UnityEvent<ICombatState> OnStateChange; // event invoked when the combat changes state :D 

    #region EventConnectors
    private void OnEnable()
    {
        dragHandler.OnPlayCard.AddListener(cardPerfomer.OnClickCard);
        playerMPAP.OnMPAPChanged.AddListener(MPAPDisplay.UpdateDisplay);
    }

    private void OnDisable()
    {
        dragHandler.OnPlayCard.RemoveListener(cardPerfomer.OnClickCard);
        playerMPAP.OnMPAPChanged.RemoveListener(MPAPDisplay.UpdateDisplay);

    }
    #endregion

    private void Awake()
    {
        cardPerfomer = new(this);
        MPAPDisplay.Initialize(playerMPAP);
        hand = new(this);
        transitionHandler = new(this);
        jobRunner = new GameObject("JobRunner").AddComponent<JobRunner>(); // haha uhhh okay
        jobRunner.GetComponent<JobRunner>().encounterManager = this;
        cardEffectManager = new(this);
    }

    private void Start()
    {
        startEncounterButton.onClick.AddListener(StartEncounter);
        
        deck.Initialize();
        deckDisplay.UpdateDisplay();
    }


    public void StartEncounter()
    {
        startEncounterButton.onClick.RemoveAllListeners();
        startEncounterButton.gameObject.SetActive(false);
        
        enemyManager.Initialize(this, encounterData); // initialize enemies before the round starts so the enemymanager listens for the roundstart 
        SwitchState(new EncounterStart());
    }

    public void EndEncounter()
    {
        SwitchState(null);
    }

    public void SwitchState(ICombatState newState)
    {
        CurrentState = newState;
        newState?.OnExit.AddListener(transitionHandler.HandleTransition);
        newState?.OnExit.AddListener(roundCounter.UpdateCounter);
        stateDisplay.UpdateState(newState.StateType); // update display, this is here for debug purposes only!
        jobRunner.SwitchState(newState);
        OnStateChange?.Invoke(newState); // tell everyone else that the state changed!
        newState?.Enter(this);
    }

    // scuffed way to wait for a certain amount of seconds on anything in this scene
    // waits for `s` seconds and invokes `callback` when finished
    public void StartWaitingForSeconds(float s, Action callback) 
    {
        StartCoroutine(Wait(s, callback));
    }

    private IEnumerator Wait(float s, Action callback)
    {
        yield return new WaitForSeconds(s);
        callback?.Invoke();
    }

    public void OpenCardSelector(string title, Action<PlayCard> onCloseCallback)
    {
        handDisplay.gameObject.SetActive(false); // stop displaying the main hand
        cardSelector.gameObject.SetActive(true);
        cardSelector.SetPrompt(title);
        cardSelector.onCardSelected.RemoveAllListeners(); // or else bad stuff happens
        cardSelector.onCardSelected.AddListener(playCard =>
        {
            // function to call when the card selector closes
            // re-display the main hand
            handDisplay.gameObject.SetActive(true);
            // stop displaying the card selector
            cardSelector.gameObject.SetActive(false);
            // finally call the callback!
            onCloseCallback(playCard);
        });
        cardSelector.UpdateHandDisplay(hand);
    }
    
    
}
