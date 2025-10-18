using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class CardDisplay : MonoBehaviour
{
    public UnityEvent<PlayCard> OnDragTopHalf { get; } = new();

    [SerializeField] TMP_Text cardName;
    [SerializeField] TMP_Text cardDescription;
    [SerializeField] TMP_Text ap;
    [SerializeField] TMP_Text mp;

    public PlayCard PlayCard { get; private set;}
    private RectTransform rectTransform;
    private Canvas canvas;
    private Transform originalParent;

    //-- Set up & Set down

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvas = GetComponentInParent<Canvas>();
    }

    private void Start()
    {
        originalParent = transform.parent;
    }

    private void OnDisable()
    {
        OnDragTopHalf.RemoveAllListeners();
    }

    //-- Public Methods

    public void DisplayCard(PlayCard playCard)
    {
        PlayCard = playCard;
        cardName.text = playCard.Card.name;
        cardDescription.text = playCard.Card.description;
        ap.text = playCard.currentAPCost.ToString();
        mp.text = playCard.currentMPCost.ToString();
    }
}
