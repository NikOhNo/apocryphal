using Mono.Cecil.Cil;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using static UnityEngine.InputSystem.InputAction;

public class DragHandler : MonoBehaviour
{
    CardDisplay dragCard;
    Transform dragCardParent;
    Canvas canvas;
    GraphicRaycaster raycaster;
    private Vector2 dragOffset;

    private void Start()
    {
        canvas = GetComponentInParent<Canvas>();
        raycaster = GetComponentInParent<GraphicRaycaster>();
    }

    public void OnBeginDrag(InputAction.CallbackContext context)
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
                dragCardParent = dragCard.transform.parent;
                dragCard.transform.SetParent(canvas.transform, true);

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


    public void OnDrag(InputAction.CallbackContext context)
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
            dragCard.transform.SetParent(dragCardParent, false);
            dragCard = null;
        }
    }
}
