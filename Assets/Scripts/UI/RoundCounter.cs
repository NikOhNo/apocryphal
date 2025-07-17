using UnityEngine;
using TMPro;

public class RoundCounter : MonoBehaviour
{
    public int Round { get; private set; } = 0;

    private TMP_Text text;

    private void Awake()
    {
        text = GetComponent<TMP_Text>();

        UpdateText();
    }

    public void UpdateCounter(StateType stateType)
    {
        if (stateType == StateType.RoundEnd)
        {
            Round++;
        }

        UpdateText();
    }

    private void UpdateText()
    {
        text.text = $"Round: {Round}";
    }
}
