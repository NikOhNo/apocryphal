using UnityEngine;

public class PlayerTurnEnd : CombatState
{
    public override StateType StateType => StateType.PlayerTurnEnd;

    public override void Enter(EncounterManager encounter)
    {
        base.Enter(encounter);

        // TODO: perform turn end jobs
        Jobs.Enqueue(new ClearHandJob());

        Jobs.Enqueue(new WaitForSecondsJob(encounter, 0.5f));
        Jobs.Enqueue(new EndStateJob(Exit));
        
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
