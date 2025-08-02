using TMPro;
using UnityEngine;

public class CardDisplay : MonoBehaviour
{
    [SerializeField] TMP_Text cardName;
    [SerializeField] TMP_Text cardDescription;
    [SerializeField] TMP_Text damage;

    public void DisplayCard(PlayCard playCard)
    {
        cardName.text = playCard.Card.name;
        cardDescription.text = playCard.Card.description;
    }
}
