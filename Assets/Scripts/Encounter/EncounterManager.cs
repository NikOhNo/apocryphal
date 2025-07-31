using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.PlayerLoop;
using UnityEngine.UI;

public class EncounterManager : MonoBehaviour
{
    public StateTransitionHandler transitionHandler;
    public RoundCounter roundCounter;
    public ButtonDisplay genericButton;
    
    
    // FIXME extremely temporary :)
    public ButtonDisplay damageEnemyButton;
    
    public JobRunner jobRunner;
    
    public EnemyManager enemyManager;

    public EncounterData encounterData;
    
    public ICombatState CurrentState { get; private set; }

    private void Awake()
    {
        transitionHandler = new(this);
        jobRunner = new GameObject("JobRunner").AddComponent<JobRunner>(); // haha uhhh okay
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
        jobRunner.SwitchState(newState);
        newState?.Enter(this);
    }
}
