using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    // given probably like an EncounterData object or something
    // create the enemies associated with this encounter
    // for now just give it an enemy prefab
    
    private EncounterManager _encounterManager;
    
    [SerializeField] private List<Enemy> _enemies = new();
    
    public int EnemyCount { get; private set; }

    public void Initialize(EncounterManager encounterManager, EncounterData encounterData)
    {
        _encounterManager = encounterManager;
        CreateEnemies(encounterData);
    }
    
    public void CreateEnemies(EncounterData data)
    {
        foreach (var enemy in data.enemies)
        {
            var go = Instantiate(enemy, transform);
            Enemy enemyInstance = go.GetComponent<Enemy>();
            _enemies.Add(enemyInstance);
            enemyInstance.onDeath.AddListener(OnEnemyDie);
            enemyInstance.Initialize(transform.GetComponentInParent<Canvas>());
            
            EnemyCount += 1; // aurgh
        }
    }

    public void DamageEnemy(Enemy e, int damage)
    {
        Debug.Log("attempting to damage enemy");
        if (_enemies.Contains(e))
        {
            e.TakeDamage(damage);
        }
    }

    public void OnEnemyDie(Enemy e)
    {
        EnemyCount -= 1;
    }

    public Enemy GetRandomEnemy()
    {
        return _enemies[Random.Range(0, _enemies.Count)];
    }

    public void OnEnemyTurn()
    {
        foreach (Enemy e in _enemies)
        {
            e.PerformAction(_encounterManager);
        }
        
        // queueing the job like this should ensure all the enemy attacks went through before we end the state.
        // assuming enemies' actions also queue jobs
        _encounterManager.jobRunner.QueueJob(new CallFunctionJob(OnEnemiesDoneAttacking));
    }

    public void OnEnemiesDoneAttacking()
    {
        // assuming the current state is EnemyTurn, we can queue a EndTurnJob that calls the Exit() function on the EncounterManager's CurrentState to exit the end turn state
        // i'm a huge fan of spaghetti and meatballs
        _encounterManager.jobRunner.QueueJob(new WaitForSecondsJob(0.5f)); // wait for a moment for debug
        _encounterManager.jobRunner.QueueJob(new CallFunctionJob(_encounterManager.CurrentState.Exit)); // very dumb i apollochives
    }
}
