using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public abstract class Steering : MonoBehaviour {

     public abstract Vector2 GetSteering(Rigidbody2D body, float maxSpeed);
}
