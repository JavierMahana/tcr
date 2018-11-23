using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AutonomousAgents))]
public class AvoidWalls :  Steering
{

    Rigidbody2D body2D;
    private void Awake()
    {
        body2D = GetComponent<Rigidbody2D>();
    }
    public List<Whisker> whiskers = new List<Whisker>();
    public LayerMask wallLayer;

    public override Vector2 GetSteering(Rigidbody2D body2D, float maxSpeed)
    {
        Vector2 steering = SteeringBehabiour.AvoidWalls( body2D, whiskers, maxSpeed, wallLayer);
        return steering;
    }

    private void OnDrawGizmos()
    {
        if (body2D)
        {
            
            foreach (Whisker w in whiskers)
            {
                Gizmos.color = Color.red;
                float angle = w.angle ;
                float bodyMag = body2D.velocity.magnitude;
                Vector2 bodyDirection;
                if (bodyMag > 0)
                {
                    bodyDirection = body2D.velocity / bodyMag;
                }
                else
                    bodyDirection = Vector2.right;
                float directionAngle = Mathf.Atan2(bodyDirection.y,bodyDirection.x);
                float whiskerAngle = angle + directionAngle;
                Vector2 wDir = new Vector2(Mathf.Cos(whiskerAngle), Mathf.Sin(whiskerAngle));
                Gizmos.DrawLine(body2D.position + w.origin, body2D.position + (wDir * w.range));
            }
        }
    }
}
