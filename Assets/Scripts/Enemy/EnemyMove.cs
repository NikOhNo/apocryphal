using System.Collections.Generic;
using SerializeReferenceEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "newEnemyMove", menuName = "New Enemy Move", order = 0)]
public class EnemyMove : ScriptableObject
{
    public enum MoveType {Attack, Block, Buff, Debuff};
    [SerializeField] public MoveType moveType;
    
    // name and scrytext for tooltip (NOT IMPLEMENTED YET) when you scry the enemy!
    public string Name { get; private set; }
    public string ScryText { get; private set; }

    // [SerializeField, Range(0.0f, 100.0f)] public float weight;
    // [SerializeField] public List<EnemyMove> nextMoves;
    // [SerializeField] public int cooldown; // number of turns needed to wait for the move to be used again
    [SerializeReference, SR] public List<EnemyMoveEffect> moveEffects;
}
