using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Unit : MonoBehaviour {

    Rigidbody2D rb2D;
    FollowPath fp;
    public Transform aa;

    private void Awake()
    {
        rb2D = GetComponent<Rigidbody2D>();
        fp = GetComponent<FollowPath>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            fp.SetPath(rb2D.position, aa.position);
        }
    }
}
