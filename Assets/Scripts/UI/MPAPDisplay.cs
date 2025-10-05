using TMPro;
using UnityEngine;

public class MPAPDisplay : MonoBehaviour
{
    [SerializeField] TMP_Text MPCount;
    [SerializeField] TMP_Text APCount;

    PlayerMPAP playerMPAP;

    public void Initialize(PlayerMPAP playerMPAP)
    {
        this.playerMPAP = playerMPAP;
    }

    private void Start()
    {
        UpdateDisplay(playerMPAP.MP, playerMPAP.AP);
    }

    public void UpdateDisplay(int mp, int ap)
    {
        MPCount.text = $"{mp}/{playerMPAP.maxMP}";
        APCount.text = $"{ap}/{playerMPAP.maxAP}";
    }
}
