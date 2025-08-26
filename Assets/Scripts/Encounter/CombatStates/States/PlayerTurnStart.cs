using UnityEngine;

public class PlayerTurnStart : CombatState
{
    public override StateType StateType => StateType.PlayerTurnStart;

    public override void Enter(EncounterManager encounter)
    {
        base.Enter(encounter);

        Jobs.Enqueue(new ClearPlayerBlockJob());
        Jobs.Enqueue(new WaitForSecondsJob(0.5f));
        Jobs.Enqueue(new CallFunctionJob(Exit));
        
        // Exit();
    }

    public override void Exit()
    {
        base.Exit();
    }

    protected override bool CanExit()
    {
        throw new System.NotImplementedException();
    }
}
