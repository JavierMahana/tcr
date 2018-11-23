using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AutonomousAgents))]
public class Separate : Steering
{
    public float separationRadious;
    public float maxRepulsion;
    public LayerMask repulsionLayer;
    void OnValidate()
    {
        maxRepulsion = Mathf.Clamp(maxRepulsion, 0, float.PositiveInfinity);
    }

    public override Vector2 GetSteering(Rigidbody2D body2D, float maxSpeed)
    {
        Vector2 steering = SteeringBehabiour.Separation(body2D, separationRadious, maxRepulsion, maxSpeed, repulsionLayer);
        return steering;
    }
}
