using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class Enemy : MonoBehaviour
{
    private HealthSystem _healthSystem = new();
    public UnityEvent<Enemy> onDeath { get; private set; } = new();
    
    public GameObject displayPrefab;
    
    public int maxHealth;
    public HealthDisplay healthDisplay; // snet in inspector

    void Awake()
    {
        _healthSystem.ResetHealth(maxHealth);
    }

    public void Initialize(Canvas canvas)
    {
        var d = Instantiate(displayPrefab, canvas.transform);
        Debug.Log(d);
        healthDisplay = d.GetComponent<HealthDisplay>();
        healthDisplay.SetHealthSystem(_healthSystem);
    }

    void OnEnable()
    {
        _healthSystem.OnDeath.AddListener(OnDeath);
    }

    void OnDeath()
    {
        Debug.Log("death");
        onDeath?.Invoke(this);
    }

    public void TakeDamage(int damage)
    {
        this._healthSystem.TakeHit(damage);
    }

    // method for performing the enemy's intent.
    // fixme this is super temporary atm
    // called by the enemymanager when it's this enemy's turn
    public void PerformAction(EncounterManager em)
    {
        // just damage player for 5 every turn. yeah. this sucks.
        em.jobRunner.QueueJob(new DamagePlayerJob(5));
        StartCoroutine(DoAttackAnimation());
    }

    public IEnumerator DoAttackAnimation()
    {
        // interpolate forward x units and then backward x units
        const float dist = 50.0f;
        const float speed = 500.0f;
        Vector2 originalPosition = healthDisplay.transform.position;
        while (healthDisplay.transform.position.x > originalPosition.x - dist)
        {
            healthDisplay.transform.position = new Vector2(healthDisplay.transform.position.x - Time.deltaTime * speed, healthDisplay.transform.position.y);
            yield return null;
        }
        
        while (healthDisplay.transform.position.x < originalPosition.x)
        {
            healthDisplay.transform.position = new Vector2(healthDisplay.transform.position.x + Time.deltaTime * speed, healthDisplay.transform.position.y);
            yield return null;
        }
        
        healthDisplay.transform.position = originalPosition;
    }
}
