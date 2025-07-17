using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;
using UnityEngine.Events;

public class ButtonDisplay : MonoBehaviour 
{
    protected Button button;
    protected TMP_Text text;

    private void Awake()
    {
        button = GetComponent<Button>();
        text = GetComponentInChildren<TMP_Text>();

        HideDisplay();
    }

    public void AddClickListener(UnityAction clickAction)
    {
        button.onClick.AddListener(clickAction);
    }

    public void SetText(string newText)
    {
        text.text = newText;
    }

    public void Display()
    {
        gameObject.SetActive(true);
    }

    public void ClearDisplay()
    {
        button.onClick.RemoveAllListeners();
        text.text = string.Empty;
    }

    public void HideDisplay()
    {
        gameObject.SetActive(false);
    }
}

