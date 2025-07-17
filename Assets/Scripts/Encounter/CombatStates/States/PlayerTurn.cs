using UnityEngine;

public class PlayerTurn : CombatState
{
    public override StateType StateType => StateType.PlayerTurn;

    public override void Enter(EncounterManager encounter)
    {
        base.Enter(encounter);

        _encounter.genericButton.AddClickListener(Exit);
        _encounter.genericButton.SetText("End Turn");
        _encounter.genericButton.Display();
    }

    public override void Exit()
    {
        _encounter.genericButton.ClearDisplay();
        _encounter.genericButton.HideDisplay();

        base.Exit();
    }

    protected override bool CanExit()
    {
        throw new System.NotImplementedException();
    }
}
