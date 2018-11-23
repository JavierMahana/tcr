using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Path  {

    public Path(Vector2[] _waypoints)
    {
        waypoints = _waypoints;
        finalPoint = _waypoints[_waypoints.Length - 1];
        radious = 0.5f;
        index = 0;
    }

    public Path(Vector2[] _waypoints, float _radious)
    {
        waypoints = _waypoints;
        finalPoint = _waypoints[_waypoints.Length - 1];
        radious = _radious;
        index = 0;
    }

    public Vector2[] waypoints;
    public Vector2 finalPoint; 
    public float radious;
    public int index ;

}
