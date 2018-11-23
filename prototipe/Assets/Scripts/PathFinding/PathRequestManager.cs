using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PathRequestManager : MonoBehaviour {

    public static PathRequestManager instance;
    PathFinding pathfinding;

    public Queue<PathRequest> requestQueue = new Queue<PathRequest>();
    PathRequest currentRequest;

    bool isProcessing = false;

    private void Awake()
    {
        instance = this;
        pathfinding = GetComponent<PathFinding>();
    }


    public void RequestPath(Vector2 startPos, Vector2 endPos, Action<Vector2[], bool> callback)
    {
        PathRequest request = new PathRequest(startPos, endPos, callback);
        instance.requestQueue.Enqueue(request);
        TryProcessNext();
    }

    public void TryProcessNext()
    {
        if (! isProcessing && requestQueue.Count > 0)
        {
            currentRequest = requestQueue.Dequeue();
            isProcessing = true;
            pathfinding.StartFindingPath(currentRequest.startPos, currentRequest.endPos);
        }
    }

    public void FinishedProcessingPath(Vector2[] path, bool success)
    {
        currentRequest.callback(path, success);
        isProcessing = false;
        TryProcessNext();
    }

    public struct PathRequest
    {
        public Vector2 startPos;
        public Vector2 endPos;
        public Action<Vector2[], bool> callback;
        public PathRequest(Vector2 _startPos, Vector2 _endPos, Action<Vector2[], bool> _callback)
        {
            startPos = _startPos;
            endPos = _endPos;
            callback = _callback;
        }
    }
}
