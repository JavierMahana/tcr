using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutonomousMovement : MonoBehaviour {

    FollowPath followPathComponent;
    AvoidWalls avoidWallsComponent;
    Separate separateComponent;

    public float maxSpeed;

    private void Awake()
    {
        followPathComponent = GetComponent<FollowPath>();
        avoidWallsComponent = GetComponent<AvoidWalls>();
        separateComponent = GetComponent<Separate>();

    }


    bool FollowPath
    {
        get
        {
            if (followPathComponent != null)
                return true;
            else
                return false;
                
        }
    }
    bool AvoidWalls
    {
        get
        {
            if (avoidWallsComponent != null)
                return true;
            else
                return false;
                
        }
    }


    bool Separate
    {
        get
        {
            if (separateComponent != null)
                return true;

            else
                return false;
                
        }
    }


    [Range(0, 1)]
    public float followPathPriority;
    [Range(0, 1)]
    public float separationPriority;
    [Range(0, 1)]
    public float avoidWallsPriority;


    private void FixedUpdate()
    {
        
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="steeringbehabiours">the order the arrey is sort is the order the forces are added to the steeringForce</param>
    /// <returns></returns>
    //Vector2 Calculate(ISteering[] steeringbehabiours)
    //{
    //    Vector2 totalSteeringForce = Vector2.zero;
    //    for (int i = 0; i < steeringbehabiours.Length; i++)
    //    {
    //        Vector2 steeringToAdd = steeringbehabiours[i].GetSteering(maxSpeed) * steeringbehabiours[i].;
    //    }
        

    //}

}


