using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public abstract class EnemyMoveEffect
{
    // hello it's me garett '''''clef''''' hammerle this whole distinction between enemymoveeffect and cardeffect is kinda unnecessary
    // so in the future i think it might be wise to unify them into something called like CombatEffect or something 
    // just so that we don't have duplicated behavior and can reuse effects
    // i.e. this would cause the problem of having two different "deal damage" effects living in the codebase which like is fine for now but as we add more effects will get very annoying
    // especially if enemies and players share a majority of their powerset (like applying debuffs and buffs, adding block, dealing damage, etc.)
    // i will get the enemy behavior system working and then tackle that problem :)
    public abstract List<IStateJob> GetJobs();
}
