using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class CardSelectInterface : MonoBehaviour
{
    public EncounterManager encounterManager; 
    public ClickableHandDisplay handDisplay;
    
    public TextMeshProUGUI promptText;
    public ClickableCardDisplay selectedCard = null;
    public Button confirmButton;
    
    public UnityEvent<PlayCard> onCardSelected;

    public void SetPrompt(string newText)
    {
        promptText.text = newText;
    }
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // connect the handDisplay's onclick function to the select card function
        handDisplay.OnClickCard.AddListener(SelectCard);
        confirmButton.onClick.AddListener(ConfirmSelection);
    }

    void SelectCard(ClickableCardDisplay cd)
    {
        if (selectedCard != null) selectedCard.SetSelected(false);
        selectedCard = cd;
        selectedCard.SetSelected(true);
    }

    void ConfirmSelection()
    {
        onCardSelected?.Invoke(selectedCard.PlayCard);
    }

    public void UpdateHandDisplay(Hand hand)
    {
        handDisplay.ClearDisplay();
        handDisplay.DisplayHand(hand);
    }
}
