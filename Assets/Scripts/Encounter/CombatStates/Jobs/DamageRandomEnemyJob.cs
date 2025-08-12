using UnityEngine;
using UnityEngine.Events;

public class DamageRandomEnemyJob : IStateJob
{
    public UnityEvent OnComplete { get; } = new();
    
    private Enemy _target;
    private EnemyManager _em;
    private int _amount;

    public DamageRandomEnemyJob(int amount)
    {
        this._amount = amount;
    }
    
    public void StartJob(EncounterManager _em)
    {
        Debug.Log("Lose health job has started! i am now LOSING HEALTH.");
        Enemy target = _em.enemyManager.GetRandomEnemy();
        _em.enemyManager.DamageEnemy(target, _amount);
        OnComplete?.Invoke();
    }
}
