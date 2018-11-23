using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node
{
    public Vector2 position;
    public int gridX, gridY;

    public bool walkable;
    public Node parent;

    public int gCost;
    public int hCost;
    public int fCost{ get { return gCost + hCost; } }

    public Node(Vector2 _position, bool _walkable, int x, int y)
    {
        position = _position;
        walkable = _walkable;
        gridX = x;
        gridY = y;
    }
}
