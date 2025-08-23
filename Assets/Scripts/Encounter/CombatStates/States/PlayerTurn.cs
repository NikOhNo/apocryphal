using UnityEngine;

public class PlayerTurn : CombatState
{
    public override StateType StateType => StateType.PlayerTurn;

    public override void Enter(EncounterManager encounter)
    {
        base.Enter(encounter);
        
        // for the button that damages the enemy
        // a problem i can see with this is how is any given attacking card going to interface with the jobRunner to queue a DamageEnemyJob for the current state?
        // perhaps it has a reference to the encountermanager or something
        // or we make some static singleton called like JobQueuer or smth
        // maybe the JobRunner can be that? idk
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
