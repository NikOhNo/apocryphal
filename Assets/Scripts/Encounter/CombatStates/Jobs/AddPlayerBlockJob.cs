using UnityEngine;
using UnityEngine.Events;

public class AddPlayerBlockJob : IStateJob
{
    // hello its me gargat hamomerle
    // consider instead of having an AddPlayerBlockJob and an AddEnemyBlockJob having just a single AddBlockJob that has a target,
    // which targets some kind of base class that the player and enemy inherits
    // that should be better practice than having specific block adding jobs
    
    public UnityEvent OnComplete { get; } = new();
    
    private EnemyManager _em;
    private int _amount;

    public AddPlayerBlockJob(int amount)
    {
        this._amount = amount;
    }
    
    public void StartJob(EncounterManager _em)
    {
        Debug.Log("Add player block job started");
        _em.player.AddBlock(_amount);
        OnComplete?.Invoke();
    }
}
