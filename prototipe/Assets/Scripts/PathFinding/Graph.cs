using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Graph : MonoBehaviour {

    [Header("The walkable Tm must contain all of the unwalkable")]
    public Tilemap walkableTilemap;
    public Tilemap unWalkableTilemap;
    public Transform TilemapGrid;

    public Vector2 nodeOffset;

    int graphSizeX, graphSizeY;
    Vector2 origin;
    

    public Node[,] graph;

    public bool displayGizmos;

    private void Awake()
    {
        CreateGraph();
    }
    void CreateGraph()
    {
        Grid grid = FindObjectOfType<Grid>();
        //el origen es la suma de: la multiplicacion de el origen de las tiles con su tamaño, la posicion de su padre y un offset(desuso)
        origin = new Vector2( (walkableTilemap.origin.x * grid.cellSize.x)+ walkableTilemap.transform.parent.position.x + TilemapGrid.position.x + nodeOffset.x,
            (walkableTilemap.origin.y * grid.cellSize.y) + walkableTilemap.transform.parent.position.x + TilemapGrid.position.y + nodeOffset.y);
        graphSizeX = walkableTilemap.size.x;
        graphSizeY = walkableTilemap.size.y;


        graph = new Node[graphSizeX, graphSizeY];

        for (int y = 0; y < walkableTilemap.size.y; y++)
        {
            for (int x = 0; x < walkableTilemap.size.x; x++)
            {
                float xPos = x + origin.x;
                float yPos = y + origin.y;
                
                if (unWalkableTilemap.HasTile(unWalkableTilemap.WorldToCell (new Vector3(xPos, yPos, 0))))
                {
                    graph[x, y] = new Node(new Vector2(xPos, yPos), false, x, y);
                    continue;
                }
                graph[x, y] = new Node(new Vector2(xPos, yPos), true , x, y);
            }
        }
    }
    public Node GetNode(Vector2 position)
    {

        float realX = (position.x - origin.x);
        float realY = (position.y - origin.y);
        realX = Mathf.Clamp(realX, 0, graphSizeX - 1);
        realY = Mathf.Clamp(realY, 0, graphSizeY - 1);

        int x = Mathf.RoundToInt(realX);
        int y = Mathf.RoundToInt(realY);

        return graph[x, y];
    }

    public Node GetNode(int x, int y)
    {
        return graph[x, y];
    }

    public List<Node> GetNeighbours(Node node)
    {
        List<Node> neighbours = new List<Node>();

        for (int y = -1; y <= 1; y++)
        {
            for (int x = -1; x <= 1; x++)
            {
                int checkX = node.gridX + x;
                int checkY = node.gridY + y;

                if (checkX >= 0 && checkX < graphSizeX && checkY >= 0 && checkY < graphSizeY)
                {
                    neighbours.Add(graph[checkX, checkY]);
                }
            }
        }
        return neighbours;
    }


    private void OnDrawGizmos()
    {
        if (graph != null && displayGizmos)
        {
            for (int y = 0; y < graph.GetLength(1); y++)
            {
                for (int x = 0; x < graph.GetLength(0); x++)
                {
                    Vector3 gizmosPos = new Vector3(graph[x, y].position.x , graph[x, y].position.y );
                    if (graph[x, y].walkable)
                    {
                        Gizmos.color = Color.blue;
                    }
                        
                    else
                        Gizmos.color = Color.red;
                    Gizmos.DrawWireCube(gizmosPos, Vector3.one);
                }
            }
        }
    }

}
