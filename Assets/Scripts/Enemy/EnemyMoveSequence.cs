using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EnemyMoveSequence
{
    public int weight;
    public int cooldown;
    public List<EnemyMove> moves; // (in order of execution)

    public EnemyMove GetMove(int index)
    {
        return (index < moves.Count ? moves[index] : null);
    }

    public bool IsSequenceOver(int index)
    {
        return index > moves.Count - 1;
    }
}
