using UnityEngine;
using UnityEngine.Events;

public class AddEnemyBlockJob : IStateJob
{
    // hello its me gargat hamomerle
    // consider instead of having an AddPlayerBlockJob and an AddEnemyBlockJob having just a single AddBlockJob that has a target,
    // which targets some kind of base class that the player and enemy inherits
    // that should be better practice than having specific block adding jobs
    
    // haha oops have not done that yet
    
    public UnityEvent OnComplete { get; } = new();
    
    private int _amount;
    private Enemy _target;
    private EnemyManager.EnemyTargetMode _mode;

    public AddEnemyBlockJob(EnemyManager.EnemyTargetMode mode, int amount)
    {
        this._amount = amount;
        this._mode = mode;
    }
    
    public void StartJob(EncounterManager _em)
    {
        Debug.Log("Add enemy block job started");
        _em.enemyManager.AddEnemyBlock(_mode, _amount);
        OnComplete?.Invoke();
    }
}
