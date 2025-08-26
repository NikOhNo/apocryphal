using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public abstract class CardEffect
{
    public abstract List<IStateJob> GetJobs();
}
