using UnityEngine;
using UnityEngine.UI;

public class IntentDisplay : MonoBehaviour
{
    [SerializeField] private Image intentIcon;
    
    // in the future probably don't want to set these in the inspector and have them read from the project resources or something instead
    [SerializeField] private Sprite attackIntentIcon;
    [SerializeField] private Sprite defenseIntentIcon;

    public void ChangeIntent(EnemyMove.MoveType intent)
    {
        switch (intent)
        {
            case EnemyMove.MoveType.Attack:
                intentIcon.sprite = attackIntentIcon;
                break;
            case EnemyMove.MoveType.Block:
                intentIcon.sprite = defenseIntentIcon;
                break;
        }
    }
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
