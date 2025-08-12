using UnityEngine;

public class CardClickListener : MonoBehaviour
{
    // temporary very verry bad singleton for listening for when cards (buttons) are clicked
    
    public static CardClickListener Instance { get; private set; }
    [SerializeField] EncounterManager encounterManager;

    public void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this);
        }
    }
    
    // function called by CardDisplay(s) when they're clicked
    public void OnClickCard(PlayCard playCard)
    {
        encounterManager.cardEffectManager.PlayCard(playCard.Card);
    }
}
