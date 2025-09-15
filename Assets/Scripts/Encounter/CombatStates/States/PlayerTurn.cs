using UnityEngine;

public class PlayerTurn : CombatState
{
    public override StateType StateType => StateType.PlayerTurn;

    public override void Enter(EncounterManager encounter)
    {
        base.Enter(encounter);

        _encounter.genericButton.AddClickListener(AddEndTurnJob);
        _encounter.genericButton.SetText("End Turn");
        _encounter.genericButton.Display();
    }

    public override void Exit()
    {
        _encounter.genericButton.ClearDisplay();
        _encounter.genericButton.HideDisplay();

        base.Exit();
    }

    private void AddEndTurnJob()
    {
        Jobs.Enqueue(new CallFunctionJob(Exit)); // fixme temporary etc.
    }

    private void AddDamageEnemyJob()
    {
        Jobs.Enqueue(new DamageRandomEnemyJob(5)); // FIXME add targeting :)
    }

    protected override bool CanExit()
    {
        throw new System.NotImplementedException();
    }
}
