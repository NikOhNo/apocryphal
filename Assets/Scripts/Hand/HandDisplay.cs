using NUnit.Framework;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class HandDisplay : MonoBehaviour
{
    [SerializeField] GameObject cardDisplayPrefab;
    readonly List<CardDisplay> cardDisplays = new();
    
    public UnityEvent<PlayCard, CardDisplay> OnClickCard;

    public void DisplayHand(Hand hand)
    {
        // TODO: animate cards going to hand

        foreach (var card in hand.cards)
        {
            CardDisplay newCardDisplay = Instantiate(cardDisplayPrefab, this.transform).GetComponent<CardDisplay>();
            newCardDisplay.DisplayCard(card);
            //newCardDisplay.OnDragTopHalf.AddListener(cardValidator.ValidateCard);
            cardDisplays.Add(newCardDisplay);
            //newCardDisplay.onClickCard.AddListener(OnCardClicked); // propagate the card clicked event up to listeners of this handdisplay
        }
    }

    public void ClearDisplay()
    {
        // TODO: animate cards going to discard

        foreach (var card in cardDisplays)
        {
            //card.onClickCard.RemoveAllListeners(); // no memory leaks on my watch
            Destroy(card.gameObject);
        }

        cardDisplays.Clear();
    }
    
    // yeah B)
    public void OnCardClicked(PlayCard card, CardDisplay cd)
    {
        OnClickCard?.Invoke(card, cd);
    }
}
