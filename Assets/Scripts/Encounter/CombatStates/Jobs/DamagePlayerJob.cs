using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class DamagePlayerJob : IStateJob
{
    // hm

    public UnityEvent OnComplete { get; } = new();
    
    private int _damage;
    
    public DamagePlayerJob(int amount) { _damage = amount; }

    public void StartJob(EncounterManager em)
    {
        Debug.Log("Damage player job started");
        em.player.HealthSystem.TakeHit(_damage);
        OnComplete?.Invoke();
    }
}
