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
    public readonly PlayerMPAP PlayerMPAP = new();
    public Hand hand;
    public HandDisplay handDisplay;
    public Deck deck;
    public DeckDisplay deckDisplay;
    public RoundCounter roundCounter;
    public ButtonDisplay genericButton;
    public StateDisplay stateDisplay;
    
    public Player player; // ref to player.... good maybe probably
    
    public StateTransitionHandler transitionHandler;

    
    // FIXME extremely temporary :)
    
    public CardSelectInterface cardSelector;
    
    public ButtonDisplay damageEnemyButton;
    
    public Button startEncounterButton;
    
    public JobRunner jobRunner;
    public EnemyManager enemyManager;
    public CardEffectManager cardEffectManager;

    public EncounterData encounterData;
    
    public ICombatState CurrentState { get; private set; }

    private void Awake()
    {
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
        
        // connect the onClickCard event of the main HandDisplay to the CardClickListener
        //handDisplay.OnClickCard.AddListener(CardClickListener.Instance.OnClickCard); // TODO fixme do some kind of like verification on card clicking
        // the reason we're doing it like this instead of just directly calling the function on the CardClickListener is
        // so that we can have an arbitrary HandDisplay
    }

    public void StartEncounter()
    {
        startEncounterButton.onClick.RemoveAllListeners();
        startEncounterButton.gameObject.SetActive(false);
        
        SwitchState(new RoundStart());
        enemyManager.Initialize(this, encounterData);
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
        stateDisplay.UpdateState(newState.StateType);
        jobRunner.SwitchState(newState);
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
