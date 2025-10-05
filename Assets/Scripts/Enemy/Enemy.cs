using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class Enemy : Entity
{
    public GameObject displayPrefab;
    
    public IntentDisplay intentDisplay; // also snet in insnector
    
    [SerializeField] private EnemyBehavior behavior; // SET ME IN THE INSPECTOR PLEEAASEEEEE
    
    
    public void Initialize(Canvas canvas)
    {
        var d = Instantiate(displayPrefab, canvas.transform);
        Debug.Log(d);
        healthDisplay = d.GetComponent<HealthDisplay>();
        healthDisplay.SetHealthSystem(HealthSystem);
        statusDisplay = d.GetComponent<StatusDisplay>();
        statusDisplay.SetStatusManager(StatusManager);
        intentDisplay = d.GetComponent<IntentDisplay>();
    }

    // method for selecting intent
    public void OnRoundStart(EncounterManager em)
    {
        behavior.SelectIntent(); // TODO: add parameters/conditions to move selection function!
        intentDisplay.ChangeIntent(behavior.Intent.moveType); // fuck it just call it here
    }

    // method for performing the enemy's intent.
    // fixme this is super temporary atm
    // called by the enemymanager when it's this enemy's turn
    public void PerformAction(EncounterManager em)
    {
        behavior.ExecuteIntent(em);

        if (behavior.Intent.moveType == EnemyMove.MoveType.Attack) // only do attack animation the move is an attack move
        {
            StartCoroutine(DoAttackAnimation());
        }
    }

    
    // called if the action the enemy is going to perform is an attack
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
