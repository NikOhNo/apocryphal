using UnityEngine;

public class PlayCard
{
    public int currentAPCost;
    public int currentMPCost;

    public Card Card { get; private set; }

    public void Initialize(Card card)
    {
        this.Card = card;

        currentAPCost = card.APCost;
        currentMPCost = card.MPCost;
    }
}
