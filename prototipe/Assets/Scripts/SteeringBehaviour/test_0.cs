using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test_0 : MonoBehaviour {
    Rigidbody2D body;

    private void Awake()
    {
        body = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        Vector2 input = new Vector3(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"),0);
        body.AddForce(input);
    }

}
