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

    public ICombatState CurrentState { get; private set; }

    private void Awake()
    {
        transitionHandler = new(this);
    }

    public void StartEncounter()
    {
        SwitchState(new RoundStart());
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
        newState?.Enter(this);
    }
}
