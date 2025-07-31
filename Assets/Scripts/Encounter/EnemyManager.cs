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
}
