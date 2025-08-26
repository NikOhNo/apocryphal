using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class CardDisplay : MonoBehaviour
{
    [SerializeField] TMP_Text cardName;
    [SerializeField] TMP_Text cardDescription;
    [SerializeField] TMP_Text damage;

    public PlayCard PlayCard { get; private set; }

    public UnityEvent<PlayCard, CardDisplay> onClickCard;

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
        PlayCard = playCard;
        cardName.text = playCard.Card.name;
        cardDescription.text = playCard.Card.description;
    }

    
    // methods for setting this card as selected
    // this is used for the card select interface
    public void SetSelected()
    {
        this.GetComponent<Image>().color = Color.cyan;
    }

    public void SetUnselected()
    {
        this.GetComponent<Image>().color = Color.white;
    }

    private void OnCardClicked()
    {
        // CardClickListener.Instance.OnClickCard(_playCard);
        onClickCard?.Invoke(PlayCard, this);
    }
}
