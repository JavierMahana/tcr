using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Whisker  {

    public Whisker(Vector2 _origin, float _angle, float _range)
    {
        origin = _origin;
        range = _range;
        angle = _angle; 
    }

    public Vector2 origin;
    public float angle;
    public float range;
}
