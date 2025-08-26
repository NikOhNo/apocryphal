using NUnit.Framework;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class HandDisplay : MonoBehaviour
{
    [SerializeField] GameObject cardDisplayPrefab;
    readonly List<CardDisplay> cardDisplays = new();

    public void DisplayHand(Hand hand)
    {
        // TODO: animate cards going to hand

        foreach (var card in hand.cards)
        {
            CardDisplay newCardDisplay = Instantiate(cardDisplayPrefab, this.transform).GetComponent<CardDisplay>();
            newCardDisplay.DisplayCard(card);
            //newCardDisplay.OnDragTopHalf.AddListener(cardValidator.ValidateCard);
            cardDisplays.Add(newCardDisplay);
        }
    }

    public void ClearDisplay()
    {
        // TODO: animate cards going to discard

        foreach (var card in cardDisplays)
        {
            Destroy(card.gameObject);
        }

        cardDisplays.Clear();
    }
}
