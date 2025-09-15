using UnityEngine;
using UnityEngine.Events;

public class PlayerMPAP
{
    /// <summary>
    /// Invoked anytime MP or AP values change. First int is MP, second is AP.
    /// </summary>
    public readonly UnityEvent<int, int> OnMPAPChanged = new();

    public int maxMP = 3;
    public int maxAP = 3;

    public int MP = 0;
    public int AP = 0;

    public void GainRoundAP()
    {
        GainAP(maxAP);
    }

    public void GainMP(int amount)
    {
        MP = Mathf.Clamp(MP + amount, 0, maxMP);
        MPAPChanged();
    }

    public void GainAP(int amount)
    {
        AP = Mathf.Clamp(AP + amount, 0, maxAP);
        MPAPChanged();
    }

    /// <summary>
    /// Returns True & spends AP & MP if Player has enough MP & AP to use the card, False and no MP & AP spent otherwise.
    /// </summary>
    /// <param name="playCard"></param>
    /// <returns></returns>
    public bool UseCard(PlayCard playCard)
    {
        if (MP >= playCard.currentMPCost && AP >= playCard.currentAPCost)
        {
            MP -= playCard.currentMPCost;
            AP -= playCard.currentAPCost;
            MPAPChanged();

            return true;
        }
        else
        {
            return false;
        }
    }

    private void MPAPChanged()
    {
        OnMPAPChanged.Invoke(MP, AP);
    }
}
