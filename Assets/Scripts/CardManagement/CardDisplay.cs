using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class CardDisplay : MonoBehaviour
{
    [SerializeField] TMP_Text cardName;
    [SerializeField] TMP_Text cardDescription;
    [SerializeField] TMP_Text damage;
    
    private PlayCard _playCard;
    
    public UnityEvent<PlayCard> onClickCard;

    public void OnEnable()
    {
        // FIXME FIXME BAD BAD AWFUL BAD BUT WE ARE DOING THIS FOR EASE OF DEBUGGING PURPOSES
        GetComponent<Button>().onClick.AddListener(OnCardClicked);
    }

    public void OnDisable()
    {
        GetComponent<Button>().onClick.RemoveListener(OnCardClicked);
    }
    
    public void DisplayCard(PlayCard playCard)
    {
        this._playCard = playCard;
        cardName.text = playCard.Card.name;
        cardDescription.text = playCard.Card.description;
    }

    private void OnCardClicked()
    {
        CardClickListener.Instance.OnClickCard(_playCard);
    }
}
