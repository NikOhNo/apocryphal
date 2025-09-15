using System.Collections.Generic;
using UnityEngine;

public class StateTransitionHandler
{
    protected EncounterManager _encounter;

    public StateTransitionHandler(EncounterManager encounter)
    {
        this._encounter = encounter;
    }

    public void HandleTransition(StateType currState)
    {
        switch (currState)
        {
            case StateType.EncounterStart:
                _encounter.SwitchState(new RoundStart());
                break;
            case StateType.RoundStart:
                _encounter.SwitchState(new PlayerTurnStart());
                break;
            case StateType.PlayerTurnStart:
                _encounter.SwitchState(new PlayerTurn());
                break;
            case StateType.PlayerTurn:
                _encounter.SwitchState(new PlayerTurnEnd());
                break;
            case StateType.PlayerTurnEnd:
                _encounter.SwitchState(new EnemyTurnStart());
                break;
            case StateType.EnemyTurnStart:
                _encounter.SwitchState(new EnemyTurn());
                break;
            case StateType.EnemyTurn:
                _encounter.SwitchState(new EnemyTurnEnd());
                break;
            case StateType.EnemyTurnEnd:
                _encounter.SwitchState(new RoundEnd());
                break;
            case StateType.RoundEnd:
                // TODO: transition to encounter end if all enemies defeated
                _encounter.SwitchState(new RoundStart());
                break;
            default:
                Debug.LogError($"Unrecognized StateType: {currState.ToString()}. Cannot transition to next state.");
                break;
        }
    }
}
