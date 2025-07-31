using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public interface ICombatState
{
    public StateType StateType { get; }
    public void Enter(EncounterManager manager);
    public void Exit();
    public UnityEvent<StateType> OnExit { get; }
    public Queue<IStateJob> Jobs {get;}
}
