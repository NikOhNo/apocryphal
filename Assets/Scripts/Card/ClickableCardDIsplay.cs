using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ClickableCardDisplay : MonoBehaviour
{
    // okay let me explain myself
    // this is a component for a prefab for a card that detects clicks ONLY!! NO DRAGGING!!
    // because instantiating the normal draggable prefab for the card selector didn't work very well
    // might want to refactor this into a CardDisplay with draggable/clickable COMPONENTS on it so we can reuse the same prefab.
    // i'll bring it up in meeting
    
    // invoked when this card is clicked!
    public UnityEvent<ClickableCardDisplay> OnClick { get; } = new();

    [SerializeField] TMP_Text cardName;
    [SerializeField] TMP_Text cardDescription;
    [SerializeField] TMP_Text damage;

    public PlayCard PlayCard { get; private set;}

    void Awake()
    {
        GetComponent<Button>().onClick.AddListener(OnClickCard);
    }
    
    private void OnClickCard()
    {
        OnClick.Invoke(this);
    }
    
    //-- Public Methods
    
    // called by anything that wants to display this card
    // it just populates the text right now
    public void DisplayCard(PlayCard playCard)
    {
        PlayCard = playCard;
        cardName.text = playCard.Card.name;
        cardDescription.text = playCard.Card.description;
    }

    public void SetSelected(bool value)
    {
        if (value)
        {
            GetComponent<Image>().color = Color.cyan;
        }
        else
        {
            GetComponent<Image>().color = Color.white;
        }
    }
}
