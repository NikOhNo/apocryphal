using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    // FIXME AAAAHHHHHH this is the targeting mode for card/enemymove effects to use when calling methods on this object
    // currently using this because cards cannot target enemies at the moment so this is how it has to be
    // NOTE: i have no idea how cards targeting enemies is going to work 👍
    
    // first targets the enemy in the very first slot
    // all targets all enemies (good luck with this one)
    // random targets a random enemy
    public enum EnemyTargetMode
    {
        First,
        All,
        Random
    } 

    private EncounterManager _encounterManager;
    
    [SerializeField] private List<Enemy> _enemies = new();
    
    public int EnemyCount { get; private set; }

    public void Initialize(EncounterManager encounterManager, EncounterData encounterData)
    {
        _encounterManager = encounterManager;
        _encounterManager.OnStateChange.AddListener(OnCombatStateChanged);
        CreateEnemies(encounterData);
    }

    private void OnCombatStateChanged(ICombatState combatState)
    {
        switch (combatState.StateType)
        {
            case StateType.EnemyTurnStart:
                OnEnemyTurnStart();
                break;
            case StateType.EnemyTurn:
                OnEnemyTurn();
                break;
            case StateType.RoundStart:
                OnRoundStart();
                break;
        }
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

    public void DamageEnemy(EnemyTargetMode mode, int damage)
    {
        Debug.Log("attempting to damage enemy");
        if (mode == EnemyTargetMode.First)
        {
            _enemies.First().HealthSystem.TakeHit(damage);
        }
        else if (mode == EnemyTargetMode.Random)
        {
            GetRandomEnemy().HealthSystem.TakeHit(damage);
        }
        else // mode is All
        {
            foreach (Enemy e in _enemies)
            {
                e.HealthSystem.TakeHit(damage);
            }
        }
    }

    public void AddEnemyBlock(EnemyTargetMode mode, int block)
    {
        Debug.Log("Adding block to enemy hello");
        if (mode == EnemyTargetMode.First)
        {
            _enemies.First().HealthSystem.GainBlock(block);
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

    public void ClearEnemyBlock(Enemy enemy)
    {
        enemy.HealthSystem.ClearBlock();
    }

    // hook to be called whenever the enemyturnstart state is reached
    // (the state itslef just calls this function directly)
    // rn it just clears the enemy block but eventually we could probably call another function on each enemy to handle its specific turnstart behavior
    public void OnEnemyTurnStart()
    {
        foreach (Enemy e in _enemies)
        {
            ClearEnemyBlock(e); 
        }
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

    public void OnRoundStart()
    {
        Debug.Log("ROUND STARTED");
        foreach (Enemy e in _enemies)
        {
            // compute intent at the start of the round
            e.OnRoundStart(_encounterManager);
        }
    }

    public void OnEnemiesDoneAttacking()
    {
        // assuming the current state is EnemyTurn, we can queue a EndTurnJob that calls the Exit() function on the EncounterManager's CurrentState to exit the end turn state
        // i'm a huge fan of spaghetti and meatballs
        _encounterManager.jobRunner.QueueJob(new WaitForSecondsJob(0.5f)); // wait for a moment for debug
        _encounterManager.jobRunner.QueueJob(new CallFunctionJob(_encounterManager.CurrentState.Exit)); // very dumb i apollochives
    }
}
