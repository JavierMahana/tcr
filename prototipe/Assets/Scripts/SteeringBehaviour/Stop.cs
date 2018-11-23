using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stop : Steering {

    public override Vector2 GetSteering(Rigidbody2D body2D, float maxSpeed)
    {
        return -body2D.velocity;
    }
}
