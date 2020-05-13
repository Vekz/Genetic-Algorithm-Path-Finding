using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Genompathfinder : MonoBehaviour
{
    #region Fields
    public DNA dna;

    public float creatureSpeed;

    public float pathMultiplier;

    int pathIndex = 0;

    public bool hasFinished = false;

    public bool hasInitialized = false;

    Vector2 target;

    Vector2 nextPoint;

    public float fitness
    {
         get
        {
            float dist = Vector2.Distance(transform.position, target);
            if(dist == 0f)
            {
                dist = 0.0001f;
            }

            return (60/ dist);
        }
    

    }



#endregion

#region Metohds


/*

private void Start()
{
    initCreature(new DNA(), Vector2.zero);        
}

*/

public void initCreature(DNA newDNA, Vector2 _target)
    {
        dna = newDNA;
        target = _target;
        nextPoint = transform.position;
        hasInitialized = true;
    }
    

    private void Update()
    {
        if(hasInitialized && !hasFinished)
        {
            if(pathIndex == dna.genes.Count || Vector2.Distance(transform.position,target) < 0.5f)
            {
                hasFinished = true;
            }

            if ((Vector2)transform.position == nextPoint)
            {
                Debug.Log($"{pathIndex}");
                Debug.Log($"{dna.genes.Count}");
                nextPoint = (Vector2)transform.position + dna.genes[pathIndex];
                pathIndex++;
            }
            else
            {
                transform.position = Vector2.MoveTowards(transform.position, nextPoint, creatureSpeed * Time.deltaTime);
            }
        }
    }
          
    #endregion




}
