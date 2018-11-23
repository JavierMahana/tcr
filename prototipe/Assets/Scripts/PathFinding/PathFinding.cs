using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PathFinding : MonoBehaviour {

    Graph graph;
    PathRequestManager requestManager;

    private void Awake()
    {
        graph = GetComponent<Graph>();
        requestManager = GetComponent<PathRequestManager>();
    }

    public void StartFindingPath(Vector2 startPos, Vector2 endPos)
    {
        StartCoroutine(ShortestPath(startPos, endPos));
    }

    IEnumerator ShortestPath(Vector2 startPos, Vector2 goalPos)
    {

        HashSet<Node> closedList = new HashSet<Node>();
        List<Node> openList = new List<Node>();

        Node startNode = graph.GetNode(startPos);
        Node endNode = graph.GetNode(goalPos);


        if (startNode == endNode)
        {
            yield break;
        }

        Vector2[] wayPoints = new Vector2[0];
        bool success = false;

        if (endNode.walkable || startNode.walkable)
        {
            startNode.gCost = 0;
            startNode.hCost = GetDistance(startNode, endNode);
            openList.Add(startNode);

            while (openList.Count > 0)
            {
                Node current = openList[0];
                for (int i = 1; i < openList.Count; i++)
                {
                    Node s = openList[i];
                    if (current.fCost > s.fCost || current.fCost == s.fCost && current.hCost > s.hCost)
                    {
                        current = s;
                    }
                }
                openList.Remove(current);
                closedList.Add(current);

                if (current == endNode)
                {
                    success = true;
                    break;
                }

                foreach (Node neighbour in graph.GetNeighbours(current))
                {
                    if (!neighbour.walkable || closedList.Contains(neighbour))
                    {
                        continue;
                    }

                    int newPathCost = current.gCost + GetDistance(current, neighbour);

                    if (!openList.Contains(neighbour) || newPathCost < neighbour.gCost)
                    {
                        neighbour.gCost = newPathCost;
                        neighbour.hCost = GetDistance(neighbour, endNode);
                        neighbour.parent = current;
                        if (!openList.Contains(neighbour))
                        {
                            openList.Add(neighbour);
                        }
                    }
                }
            }
        }
        //the start or the end are not walkable
        else
            Debug.Log("Invalid Destination or start");


        yield return null;

        if (success)
        {
            wayPoints = RetrievePath(startNode, endNode);
        }

        requestManager.FinishedProcessingPath(wayPoints, success);
    }

    Vector2[] RetrievePath(Node startNode, Node endNode)
    {
        //que pasa si end node es igual a startnode
        List<Node> path = new List<Node>();
        Node checkNode = endNode;
        while (checkNode != startNode)
        {
            path.Add(checkNode);
            checkNode = checkNode.parent;
        }
        path.Reverse();
        Vector2[] waypoints = SimpifyPath(path);
        return waypoints;
    }

    Vector2[] SimpifyPath(List<Node> path)
    {
        foreach (Node waypoint in path)
        {
            Debug.Log(waypoint.position + "1");
        }
        List<Vector2> simplifiedPath = new List<Vector2>();
        Vector2 directionOld = Vector2.zero;
        for (int i = 0; i < path.Count; i++)
        {
            if (i + 1 == path.Count)
            {
                simplifiedPath.Add(path[i].position);
                break;
            }
            Vector2 directionNew = new Vector2(path[i + 1].gridX - path[i].gridX, path[i + 1].gridY - path[i].gridY);
            if (directionNew != directionOld)
            {
                simplifiedPath.Add(path[i].position);
            }
            directionOld = directionNew;
        }
        return simplifiedPath.ToArray();
    }

    int GetDistance(Node a, Node b)
    {
        int xDistance = Mathf.Abs(a.gridX - b.gridX);
        int yDistance = Mathf.Abs(a.gridY - b.gridY);

        if (xDistance< yDistance)
        {
            return (xDistance * 14) + (10 * (yDistance - xDistance));
        }
        else
            return (yDistance * 14) + (10 * (xDistance - yDistance));
    }
}
