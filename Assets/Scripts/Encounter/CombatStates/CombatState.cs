using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class CombatState : ICombatState
{
    public abstract StateType StateType { get; }
    public UnityEvent<StateType> OnExit { get; private set; } = new();

    protected EncounterManager _encounter;
    public Queue<IStateJob> Jobs {get;} = new();

    public virtual void Enter(EncounterManager encounter)
    {
        // Debug.Log($"{GetType().Name} entered");
        
        this._encounter = encounter;
    }

    public virtual void Exit()
    {
        // Debug.Log($"{GetType().Name} exited");

        OnExit.Invoke(StateType);
        OnExit.RemoveAllListeners();
    }
    
    // job runner every conbat state ahs a job runner
    // which constantly polls the queue of jobs to see if there are any to run
    // (probably an update function)
    // when last job finishes set jobRunning to false (in performnextjob)

    protected abstract bool CanExit();
}

public enum StateType
{
    NULL,

    EncounterStart,
    RoundStart,
    PlayerTurnStart,
    PlayerTurn,
    PlayerTurnEnd,
    EnemyTurnStart,
    EnemyTurn,
    EnemyTurnEnd,
    RoundEnd,
    EncounterEnd,
}