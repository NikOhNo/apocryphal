using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

[CreateAssetMenu(fileName = "newEnemyBehavior", menuName = "Scriptable Objects/EnemyBehavior")]
public class EnemyBehavior : ScriptableObject
{
    // move sequences available from the start (i.e. when no moves have been executed yet)
    public List<EnemyMoveSequence> moveSequences = new();
    
    private EnemyMoveSequence currentSequence; // the sequence of moves we're currently on
    private int currentSequenceIndex; // araagh
    
    public EnemyMove Intent {get; private set;} // the move we want to execute this turn, i.e. our "next" move

    public void SelectIntent()
    {
        if (currentSequence != null)
        {
            // go next in the sequence if it exists
            
            // increment the currentsequenceindex
            currentSequenceIndex += 1;
            EnemyMove nextMove = currentSequence.GetMove(currentSequenceIndex);
            if (nextMove != null) // move is valid, so we can change that to our intent
            {
                Intent = nextMove;
                // and now we're done here
            }
            else // move is INVALID so this sequence is OVER so we need to do the whole move selection thing again (that's why the next if isn't an else if)
            {
                currentSequence = null;
            }
        }
        
        // not in a sequence, meaning we should randomly select from the moveSequences list
        if (currentSequence == null) // not else if in case we incremented the sequence and found we were at the end of it
        {
            currentSequence = SelectSequenceByWeight();
            currentSequenceIndex = 0;
            Intent = currentSequence.GetMove(currentSequenceIndex);
        }
        
        Debug.Log($"selected sequence {currentSequence} with intent {Intent.name}");
    }

    public EnemyMoveSequence SelectSequenceByWeight()
    {
        // weighted random selection algorithm
        float sum = 0f;
        foreach (EnemyMoveSequence ms in moveSequences)
        {
            sum += ms.weight;
        }
            
        float roll = Random.Range(0f, sum);
        foreach (EnemyMoveSequence ms in moveSequences)
        {
            if (roll <= ms.weight) // it's <= because if there's only one weight then it should always pick that as the intent
            {
                return ms;
            }
            roll -= ms.weight;
        }
        
        Assert.IsTrue(false); // autofail assertion always because we should never get here
        return null; // should be unreachable
    }

    // call to set the intent move to the current move and queue all of its effects
    // preferably called by the enemy that owns this enemybehavior object
    public void ExecuteIntent(EncounterManager em)
    {
        if (Intent == null)
        {
            Debug.LogError("Enemybehavior has no intent, not executing intent!"); // should never get here theoretically
            return;
        }
        
        Debug.Log($"executing intent: {Intent.name}");
        foreach (EnemyMoveEffect effect in Intent.moveEffects)
        {
            List<IStateJob> jobs = effect.GetJobs();
            foreach (IStateJob j in jobs)
            {
                em.jobRunner.QueueJob(j);
            }
        }
    }
}
