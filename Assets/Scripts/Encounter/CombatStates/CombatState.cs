using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class CombatState : ICombatState
{
    public abstract StateType StateType { get; }
    public UnityEvent<StateType> OnExit { get; private set; } = new();

    protected EncounterManager _encounter;
    protected Queue<IStateJob> _jobs = new();

    public virtual void Enter(EncounterManager encounter)
    {
        Debug.Log($"{GetType().Name} entered");
        
        this._encounter = encounter;
    }

    public virtual void Exit()
    {
        Debug.Log($"{GetType().Name} exited");

        OnExit.Invoke(StateType);
        OnExit.RemoveAllListeners();
    }

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