using Mono.Cecil.Cil;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using static UnityEngine.InputSystem.InputAction;

public class DragHandler : MonoBehaviour
{
    [SerializeField] HandDisplay handDisplay;
    CardDisplay dragCard;
    Transform dragCardParent;
    Canvas canvas;
    GraphicRaycaster raycaster;
    private Vector2 dragOffset;
    
    public UnityEvent<PlayCard, CardDisplay> OnPlayCard; // invoked when a card is Supposed to be played.
                                                         // might want to invoke it with more parameters (e.g. the target if it's a targeted card, etc.)

    private void Start()
    {
        canvas = GetComponentInParent<Canvas>();
        raycaster = GetComponentInParent<GraphicRaycaster>();
    }

    public void OnBeginDrag(CallbackContext context)
    {
        if (!context.performed) return;

        PointerEventData pointerData = new PointerEventData(FindFirstObjectByType<EventSystem>())
        {
            position = Mouse.current.position.ReadValue()
        };

        var results = new List<RaycastResult>();
        raycaster.Raycast(pointerData, results);

        foreach (var result in results)
        {
            var cardDisplay = result.gameObject.GetComponent<CardDisplay>();
            if (cardDisplay != null)
            {
                dragCard = cardDisplay;
                handDisplay.DetachCard(dragCard);

                // Calculate drag offset
                RectTransform cardRect = dragCard.GetComponent<RectTransform>();
                RectTransformUtility.ScreenPointToLocalPointInRectangle(
                    canvas.transform as RectTransform,
                    pointerData.position,
                    canvas.worldCamera,
                    out Vector2 localPoint
                );

                dragOffset = cardRect.anchoredPosition - localPoint;

                break;
            }
        }
    }


    public void OnDrag(CallbackContext context)
    {
        if (dragCard == null) return;

        Vector2 screenPos = Mouse.current.position.ReadValue();

        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            canvas.transform as RectTransform,
            screenPos,
            canvas.worldCamera,
            out Vector2 localPoint
        );

        dragCard.GetComponent<RectTransform>().anchoredPosition = localPoint + dragOffset;
    }


    public void OnEndDrag(CallbackContext context)
    {
        if (!context.canceled) return;

        if (dragCard != null)
        {
            if (Mouse.current.position.ReadValue().y > Screen.height / 2)
            {
                OnPlayCard.Invoke(dragCard.PlayCard, dragCard);
            }
            else
            {
                handDisplay.AttachCard(dragCard);
            }

            dragCard = null;
        }
    }
}
