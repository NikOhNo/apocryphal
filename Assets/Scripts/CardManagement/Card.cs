using UnityEngine;

[CreateAssetMenu(fileName = "newCard", menuName = "New Card", order = 0)]
public class Card : ScriptableObject
{
    [SerializeField] public Sprite sprite = null;
    [SerializeField] public bool hasAPCost = false;
    [SerializeField] public int APCost = 0;
    [SerializeField] public bool hasMPCost = false;
    [SerializeField] public int MPCost = 0;
    [SerializeField] public string description = string.Empty;
}
