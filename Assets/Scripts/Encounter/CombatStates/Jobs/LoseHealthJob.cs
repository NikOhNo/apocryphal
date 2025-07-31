using UnityEngine;
using UnityEngine.Events;

public class LoseHealthJob : IStateJob
{
    public UnityEvent OnComplete { get; } = new();
    
    private Enemy _target;
    private EnemyManager _em;
    private int _amount;

    public LoseHealthJob(EnemyManager enemyManager, Enemy target, int amount)
    {
        this._em = enemyManager;
        this._target = target;
        this._amount = amount;
    }
    
    public void StartJob()
    {
        Debug.Log("Lose health job has started! i am now LOSING HEALTH.");
        _em.DamageEnemy(_target, _amount);
        OnComplete?.Invoke();
    }
}
