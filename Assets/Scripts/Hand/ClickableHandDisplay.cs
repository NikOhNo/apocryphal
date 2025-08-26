using NUnit.Framework;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class ClickableHandDisplay : MonoBehaviour
{
    [SerializeField] GameObject cardDisplayPrefab;
    readonly List<ClickableCardDisplay> cardDisplays = new();
    
    public UnityEvent<ClickableCardDisplay> OnClickCard;

    public void DisplayHand(Hand hand)
    {
        // TODO: animate cards going to hand

        foreach (var card in hand.cards)
        {
            ClickableCardDisplay newCardDisplay = Instantiate(cardDisplayPrefab, this.transform).GetComponent<ClickableCardDisplay>();
            newCardDisplay.DisplayCard(card);
            //newCardDisplay.OnDragTopHalf.AddListener(cardValidator.ValidateCard);
            cardDisplays.Add(newCardDisplay);
            newCardDisplay.OnClick.AddListener(OnCardClicked); // propagate the card clicked event up to listeners of this handdisplay
        }
    }

    public void ClearDisplay()
    {
        // TODO: animate cards going to discard

        foreach (var card in cardDisplays)
        {
            card.OnClick.RemoveAllListeners(); // no memory leaks on my watch
            Destroy(card.gameObject);
        }

        cardDisplays.Clear();
    }
    
    // yeah B)
    public void OnCardClicked(ClickableCardDisplay card)
    {
        OnClickCard?.Invoke(card);
    }
}