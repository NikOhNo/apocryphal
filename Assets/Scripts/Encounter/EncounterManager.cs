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
    public StateTransitionHandler transitionHandler;
    public Deck deck;
    public DeckDisplay deckDisplay;
    public Hand hand;
    public HandDisplay handDisplay;
    public RoundCounter roundCounter;
    public ButtonDisplay genericButton;
    public StateDisplay stateDisplay;
    
    
    // FIXME extremely temporary :)
    public ButtonDisplay damageEnemyButton;
    
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
        deck.Initialize();
        deckDisplay.UpdateDisplay();
    }

    public void StartEncounter()
    {
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
}
