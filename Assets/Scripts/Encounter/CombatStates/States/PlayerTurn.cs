using UnityEngine;

public class PlayerTurn : CombatState
{
    public override StateType StateType => StateType.PlayerTurn;

    public override void Enter(EncounterManager encounter)
    {
        base.Enter(encounter);
        
        _encounter.damageEnemyButton.AddClickListener(AddDamageEnemyJob);
        _encounter.damageEnemyButton.SetText("Deal 5 damage");
        _encounter.damageEnemyButton.Display();

        _encounter.genericButton.AddClickListener(AddEndTurnJob);
        _encounter.genericButton.SetText("End Turn");
        _encounter.genericButton.Display();
    }

    public override void Exit()
    {
        _encounter.damageEnemyButton.ClearDisplay();
        _encounter.damageEnemyButton.HideDisplay();
        
        _encounter.genericButton.ClearDisplay();
        _encounter.genericButton.HideDisplay();

        base.Exit();
    }

    private void AddEndTurnJob()
    {
        Jobs.Enqueue(new EndStateJob(Exit)); // fixme temporary etc.
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
