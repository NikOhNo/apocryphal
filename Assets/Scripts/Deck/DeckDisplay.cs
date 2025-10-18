using Scripts.Deck;
using TMPro;
using UnityEngine;

public class DeckDisplay : MonoBehaviour
{
    public TMP_Text drawCountText;
    public TMP_Text discardCountText;
    public TMP_Text sealCountText;

    [SerializeField] Deck deck;

    public void UpdateDisplay()
    {
        drawCountText.text = deck.cardsInDeck.Count.ToString();
        discardCountText.text = deck.cardsInDiscard.Count.ToString();
        sealCountText.text = deck.cardsInSeal.Count.ToString();
    }
}
