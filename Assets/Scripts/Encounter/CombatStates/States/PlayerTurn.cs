using UnityEngine;

public class PlayerTurn : CombatState
{
    public override StateType StateType => StateType.PlayerTurn;

    public override void Enter(EncounterManager encounter)
    {
        base.Enter(encounter);

        _encounter.endTurnButton.AddClickListener(AddEndTurnJob);
        _encounter.endTurnButton.SetText("End Turn");
        _encounter.endTurnButton.Display();

        _encounter.addStatusButton.AddClickListener(AddStatusJob);
        _encounter.addStatusButton.SetText("add dummy status");
        _encounter.addStatusButton.Display();
    }

    public override void Exit()
    {
        _encounter.endTurnButton.ClearDisplay();
        _encounter.endTurnButton.HideDisplay();

        _encounter.addStatusButton.ClearDisplay();
        _encounter.addStatusButton.HideDisplay();

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

    private void AddStatusJob()
    {
        Jobs.Enqueue(new ApplyStatusJob(1)); // always applies burning rn!
    }

    protected override bool CanExit()
    {
        throw new System.NotImplementedException();
    }
}
