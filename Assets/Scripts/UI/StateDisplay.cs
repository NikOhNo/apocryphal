using TMPro;
using UnityEngine;

public class StateDisplay : MonoBehaviour
{
    private TMP_Text text;
    
    public StateType State {get; private set;}

    void Awake()
    {
        text = GetComponent<TMP_Text>();
        
        UpdateText();
    }

    public void UpdateState(StateType state)
    {
        State = state;
        
        UpdateText();
    }

    void UpdateText()
    {
        text.text = "Current state: " + State.ToString();
    }
}
