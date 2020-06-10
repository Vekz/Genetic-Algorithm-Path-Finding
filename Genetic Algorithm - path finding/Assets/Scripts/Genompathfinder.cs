using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class implements our creature for Genetic Algorithm.
/// Creature is constructed to find shortest path from start point to exit point.
/// Fitness of creature is defined as a distance in straight line from creature to exit point
/// and difference of number 
/// </summary>
public class Genompathfinder : MonoBehaviour
{
    #region Fields
    #region Characteristics of the creature
    public DNA dna; //DNA of the creature
    public float creatureSpeed; //Speed in which creature traverses the path
    public float pathMultiplier; //Multiplier which determines the size of the steps
    #endregion

    #region Current status variables
    int pathIndex = 0; //Index for the current step
    public bool hasFinished = false; //Has the creature finished moving?
    bool hasInitialized = false; //Has the creature been initialized?
    Vector2 target; //Exit point that creature aims at
    Vector2 nextPoint; //Next point to which it steps
    #endregion

    #region Esthetics variables
    Quaternion targetRotation; //Rotation of the creature in the aim of the next step #esthetics
    public float rotationSpeed; //Speed of the rotation translation #esthetics
    LineRenderer LR; //Renderer of the line behind creature through its path #esthetics
    List<Vector2> TravelledPath = new List<Vector2>(); //List of visited points #esthetics
    #endregion

    #region Fitness variables
    public LayerMask obstacleLayer; //Layer of obstacles in the scene
    public float winBias = 1.15f; //Determines the fitness bonus for those creatures that touched the target.
    public float crashPenalty = 0.7f; //Penalty for crashing into wall
    bool hasCrashed = false; //Has creature crashed into the wall?
    private bool hasWon = false; //Has creature touched the target?
    #endregion

    //Fitness of creature is defined as a distance in straight line from creature to exit point
    // and difference of number
    public float fitness
    {
         get
        {
            float dist = Vector2.Distance(transform.position, target); //Current distance from creature to target
            if(dist == 0f)
            {
                dist = 0.0001f; //Substitution of 0 to avoid Divide by 0 error
            }

            RaycastHit2D[] obstacles = Physics2D.RaycastAll(transform.position, target,obstacleLayer); //List of obstacles in the straight line path from creature to target location
            float obstacleMultiplayer = 1f - ((1f - crashPenalty)*obstacles.Length); //Multiplier based on number of obstacles between creature and target (less is better)
            return (60 / dist) * (hasCrashed ? crashPenalty : 1f) * obstacleMultiplayer * (hasWon ? winBias : 1f );
        }
    }
#endregion

#region Metohds
    
    /// <summary>
    /// Initialization of the creature
    /// </summary>
    /// <param name="newDNA"></param> The DNA that should be used to create creature
    /// <param name="_target"></param> Target that creature should aim for
    public void initCreature(DNA newDNA, Vector2 _target)
    {
        TravelledPath.Add(transform.position); //Add starting position of creature to visited points
        LR = GetComponent<LineRenderer>(); //Get component of LineRenderer for current creature
        dna = newDNA;
        target = _target;
        nextPoint = transform.position; //Set first nextPoint as current position
        TravelledPath.Add(nextPoint); //Add nextPoint to visited points
        hasInitialized = true; //Creature has been initialized
    }
    
    /// <summary>
    /// Method that is taking care of rendering line based on points visited by the creature.
    /// </summary>
    public void RenderLine()
    {
        List<Vector3> LinePoints = new List<Vector3>(); //List of point for the rendered line
        if (TravelledPath.Count > 2 )
        {
            for (int i = 0; i < TravelledPath.Count-1; i++)
            {
                LinePoints.Add(TravelledPath[i]); //Adding visited point to list of points for line renderer

            }

            LinePoints.Add(transform.position); //Adding current position of the creature to points for line renderer
        }
        else
        {
            LinePoints.Add(TravelledPath[0]);
            LinePoints.Add(transform.position);
        }
        LR.positionCount = LinePoints.Count; 
        LR.SetPositions(LinePoints.ToArray()); //Set positions of rendered line points to positions of points visited by creature
    }

    /// <summary>
    /// Method called every frame.
    /// Updates status of the creature.
    /// </summary>
    private void Update()
    {
        if(hasInitialized && !hasFinished) //If creature is initialized and didn't finish
        {
            if(pathIndex == dna.genes.Count || Vector2.Distance(transform.position,target) < 0.5f) //If current step is equal to the number of genoms or distance of the creature to target is less than 0.5f
            {
                hasFinished = true; //Creature has finished
            }

            if ((Vector2)transform.position == nextPoint) //If current creature position is the same as next step point
            {
                nextPoint = (Vector2)transform.position + dna.genes[pathIndex]*pathMultiplier; //Next point is current position + gene of the creature @ pathIndex step multiplied by pathMultipier of the creature
                TravelledPath.Add(nextPoint); //Adding next point to the list of traversed points
                targetRotation = LookAt2D(nextPoint); //Changing target of rotation, so the creature looks in the way of its next step
                pathIndex++; //Incrementing pathIndex (step number)
            }
            else
            {
                transform.position = Vector2.MoveTowards(transform.position, nextPoint, creatureSpeed * Time.deltaTime); //Creature moves in way of next point with distance defined as creatureSpeed times deltaTime
            }

            if(transform.rotation != targetRotation) //If current rotation of creature is different than rotation to next point
            {
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime); //Creature rotates itself to face next point with rotation speed of rotationSpeed times deltaTime
            }
        RenderLine(); //Render line through points visited by the creature
        }
    }
          
    /// <summary>
    /// Method that calculates the rotation needed for the creature to face its next target.
    /// </summary>
    /// <param name="target"></param> Target to face by creature
    /// <param name="angleoffset"></param> Angle offset of rotation
    /// <returns></returns> Rotation value for the creature to face its next target.
    public Quaternion LookAt2D(Vector2 target,float angleoffset = 90f)
    {
        Vector2 fromTo = (target - (Vector2)transform.position).normalized;
        float zrotation = Mathf.Atan2(fromTo.y, fromTo.x) * Mathf.Rad2Deg;

        return Quaternion.Euler(0, 0, zrotation * angleoffset);
    }

    /// <summary>
    /// Method deciding did the creature collided with obstacle or exit point.
    /// </summary>
    /// <param name="collider"></param> Collider in Unity
    public void OnTriggerEnter2D(Collider2D collider)
    {
        if(collider.gameObject.layer == 8 ) //If collider is on layer 8th - obstacle layer
        {
            hasFinished = true; //Creature has finished
            hasCrashed = true; //Creature has crashed
        }
        else if ( collider.gameObject.layer == 9) //If collider is on layer 9th - goal layer
        {
            hasFinished = true; //Creature has finished
            hasWon = true; //Creature has reached goal
        }
    }
    #endregion
}
