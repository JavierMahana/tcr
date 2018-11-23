using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Vector2Extensions
{
    /// <summary>
    /// If the new magnitude if less than the current. change the magnitude to the new one
    /// </summary>
    /// <param name="a"></param>
    /// <param name="newMagnitude"></param>
    /// <returns></returns>
    public static Vector2 ShortenMagnitude(this Vector2 a, float newMagnitude)
    {
        float currentMagnitude = a.magnitude;
        if (currentMagnitude < newMagnitude)
        {
            return a;
        }
        float shortestMagnitude = Mathf.Min(currentMagnitude, newMagnitude);
        a = a / currentMagnitude * shortestMagnitude;
        return a;
    }

    public static float SqrDistance(this Vector2 a,  Vector2 b)
    {
        return (a - b).sqrMagnitude;
    }
}
