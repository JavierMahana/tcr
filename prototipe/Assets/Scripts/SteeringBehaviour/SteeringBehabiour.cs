using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public static class SteeringBehabiour
{
    // every vector2.distance call use .magnitude/ maybe is better to do it manualy with the sqrMagnitude.
    const float defaultArribeRadious = 0.5f;
    
    public static Vector2 Separation(Rigidbody2D _rigidbody, float repulsionRadious, float _maxRepulsion, float _maxSpeed, LayerMask layerMask)
    {
        Vector2 steering = Vector2.zero;
        Collider2D[] entitiesAround = new Collider2D[10];
        int numOfEntities =  Physics2D.OverlapCircleNonAlloc(_rigidbody.position, repulsionRadious, entitiesAround, layerMask);
        Debug.Log(numOfEntities.ToString());
        for (int i = 0; i < numOfEntities; i++)
        {
            Vector2 ePos = entitiesAround[i].GetComponent<Rigidbody2D>().position;
            float intensity = repulsionRadious / Vector2.Distance(ePos, _rigidbody.position); //Mathf.Min( repulsionRadious / Vector2.Distance(ePos, _rigidbody.position),  _maxRepulsion);
            steering += (_rigidbody.position - ePos).normalized * intensity;
        }
        return steering.ShortenMagnitude(_maxSpeed);
    }
    public static Vector2 AvoidWalls(Rigidbody2D _rigidbody, List<Whisker> whiskers, float _maxSpeed, LayerMask wallLayer)
    {
        float velAngle = Mathf.Atan2(_rigidbody.velocity.y, _rigidbody.velocity.x);
        float velRelated =  _rigidbody.velocity.magnitude/ _maxSpeed;
        Vector2 steering = Vector2.zero;
        for (int i = 0; i < whiskers.Count; i++)
        {
            Whisker whisker = whiskers[i];

            float rayDistance = velRelated * whisker.range;
            float whiskerAngle = whisker.angle + velAngle;
            Vector2 whiskerDirection = new Vector2(Mathf.Cos(whiskerAngle), Mathf.Sin(whiskerAngle));
            RaycastHit2D hit = Physics2D.Raycast(_rigidbody.position + whisker.origin, whiskerDirection, rayDistance , wallLayer);
            if (hit)
            {
                steering += ((hit.normal + whiskerDirection).normalized) * (whisker.range - hit.distance);
            }
        }
        return steering.ShortenMagnitude(_maxSpeed);
    }
    public static Vector2 FollowPath(Rigidbody2D _rigidbody, Path path, float _maxSpeed, float _lookAhead, float _boomMagnitude)
    {
        Vector2 predictedPos = (_rigidbody.position + _rigidbody.velocity) * _lookAhead;
        Vector2 closestPointInThePath = Vector2.positiveInfinity;

        if (path.index < path.waypoints.Length - 1)
        {
            if (Vector2.Distance(path.waypoints[path.index + 1], _rigidbody.position) < path.radious)
            {
                path.index++;
            }
        }
        if (path.index == path.waypoints.Length - 1)
        {
            return Arribe(_rigidbody, path.waypoints[path.index], _maxSpeed, path.radious/2);
        }
        Vector2 currentSegmentOfThePathDirection;
        closestPointInThePath = GetTheNormalPointOfSegmentFromPoint(predictedPos, path.waypoints[path.index], path.waypoints[path.index + 1], out currentSegmentOfThePathDirection); 

        if (Vector2.Distance(closestPointInThePath, predictedPos) > path.radious)
        {
            closestPointInThePath += (currentSegmentOfThePathDirection * _boomMagnitude);
            return Seek(_rigidbody, closestPointInThePath, _maxSpeed);
        }

        return Vector2.zero;
    }
    public static Vector2 Seek(Rigidbody2D rigidbody, Vector2 objective, float maxSpeed)
    {

        Vector2 desiredVelocity = (objective - rigidbody.position).normalized * maxSpeed;
        Vector2 steering = desiredVelocity - rigidbody.velocity;
        return steering;
    }
    /// <summary>
    /// Arribe with fixed slowDownRadious
    /// </summary>
    /// <param name="rigidbody"></param>
    /// <param name="objective"></param>
    /// <param name="maxSpeed"></param>
    public static Vector2 Arribe(Rigidbody2D rigidbody, Vector2 objective, float maxSpeed, float arribeRadious)
    {
        //desde más pequeña es la desired vel mas rapido frenará.
        Vector2 distance = objective - rigidbody.position;
        float distanceMagnitude = distance.magnitude;
        float desiredSpeed = maxSpeed;
        Vector2 desiredVelocity;
        Vector2 steering;
        if (distanceMagnitude < arribeRadious)
        {
            desiredVelocity = Vector2.zero;
            steering = desiredVelocity - rigidbody.velocity;
            return (steering);
        }

        if (distanceMagnitude < defaultArribeRadious)
        {
            float percentToArribe = distanceMagnitude / defaultArribeRadious;
            desiredSpeed = maxSpeed * percentToArribe;

            desiredVelocity = (distance / distanceMagnitude) * desiredSpeed;
            steering = desiredVelocity - rigidbody.velocity;
            return (steering);
        }
        else
            return Seek(rigidbody, objective, maxSpeed);
    }
    public static Vector2 Arribe(Rigidbody2D rigidbody, Vector2 objective, float maxSpeed, float slowDownRadious, float arribeRadious)
    {
        //desde más pequeña es la desired vel mas rapido frenará.
        Vector2 distance = objective - rigidbody.position;
        float distanceMagnitude = distance.magnitude;
        float desiredSpeed = maxSpeed;
        Vector2 desiredVelocity;
        Vector2 steering;
        if (distanceMagnitude < arribeRadious)
        {
            desiredVelocity = Vector2.zero;
            steering = desiredVelocity - rigidbody.velocity;
            return (steering);
        }

        if (distanceMagnitude < slowDownRadious)
        {
            float percentToArribe = distanceMagnitude / slowDownRadious;
            desiredSpeed = maxSpeed * percentToArribe;

            desiredVelocity = (distance / distanceMagnitude) * desiredSpeed;
            steering = desiredVelocity - rigidbody.velocity;
            return (steering);
        }
        else
            return Seek(rigidbody, objective, maxSpeed);
    }

    static Vector2 GetTheNormalPointOfSegmentFromPoint(Vector2 point, Vector2 a, Vector2 b, out Vector2 segmentDirection)
    {
        Vector2 ap = point - a;
        Vector2 ab = b - a;
        segmentDirection = ab.normalized;

        float scalarProyection = Vector2.Dot(ap, segmentDirection);

        if (scalarProyection < 0 || scalarProyection * scalarProyection > ab.sqrMagnitude)
        {
            if (Vector2.Distance(point, a) < Vector2.Distance(point, b))
            {
                return a;
            }
            return b;
        }
        Vector2 normalPoint = a + (segmentDirection * scalarProyection);
        return normalPoint;
    }
    
}
