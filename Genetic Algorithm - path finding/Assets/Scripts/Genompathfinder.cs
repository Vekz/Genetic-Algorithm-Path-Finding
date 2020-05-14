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

    Quaternion targetRotation;

    public float rotationSpeed;


    LineRenderer LR;

    List<Vector2> TravelledPath = new List<Vector2>();

    public LayerMask obstacleLayer;

    #region fitness variables
    public float winBias = 2f;
    public float crashPenalty = 0.7f;
    bool hasCrashed = false;
    private bool hasWon = false;
    #endregion

    public float fitness
    {
         get
        {
            float dist = Vector2.Distance(transform.position, target);
            if(dist == 0f)
            {
                dist = 0.0001f;
            }

            RaycastHit2D[] obstacles = Physics2D.RaycastAll(transform.position, target,obstacleLayer);
            float obstacleMultiplayer = 1f - ((1f - crashPenalty)*obstacles.Length);
            return (60 / dist) * (hasCrashed ? crashPenalty : 1f) * obstacleMultiplayer * (hasWon ? winBias : 1f );
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
        TravelledPath.Add(transform.position);
        LR = GetComponent<LineRenderer>();
        dna = newDNA;
        target = _target;
        nextPoint = transform.position;
        TravelledPath.Add(nextPoint);
        hasInitialized = true;
    }
    

    public void RenderLine()
    {
        List<Vector3> LinePoints = new List<Vector3>();
        if (TravelledPath.Count > 2 )
        {
            for (int i = 0; i < TravelledPath.Count-1; i++)
            {
                LinePoints.Add(TravelledPath[i]);

            }

            LinePoints.Add(transform.position);
        }
        else
        {
            LinePoints.Add(TravelledPath[0]);
            LinePoints.Add(transform.position);
        }
        LR.positionCount = LinePoints.Count;

        LR.SetPositions(LinePoints.ToArray());

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
                nextPoint = (Vector2)transform.position + dna.genes[pathIndex]*pathMultiplier;
                TravelledPath.Add(nextPoint);
                targetRotation = LookAt2D(nextPoint);
                pathIndex++;
            }
            else
            {
                transform.position = Vector2.MoveTowards(transform.position, nextPoint, creatureSpeed * Time.deltaTime);
            }

            if(transform.rotation != targetRotation)
            {
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
            }
        RenderLine();
        }

    }
          
    public Quaternion LookAt2D(Vector2 target,float angleoffset = 90f)
    {
        Vector2 fromTo = (target - (Vector2)transform.position).normalized;
        float zrotation = Mathf.Atan2(fromTo.y, fromTo.x) * Mathf.Rad2Deg;

        return Quaternion.Euler(0, 0, zrotation * angleoffset);
    }

    public void OnTriggerEnter2D(Collider2D collider)
    {
        if(collider.gameObject.layer == 8 )
        {
            hasFinished = true;
            hasCrashed = true;
        }
        else if ( collider.gameObject.layer == 9)
        {
            hasFinished = true;
            hasWon = true;
        }
    }

    #endregion




}
