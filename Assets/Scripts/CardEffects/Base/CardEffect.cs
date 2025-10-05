using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public abstract class CardEffect
{
    public PlayCard PlayCard { get; set; }
    public abstract List<IStateJob> GetJobs();
}
