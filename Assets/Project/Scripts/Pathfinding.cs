using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Pathfinding : MonoBehaviour
{
    [SerializeField] GridNodes _grid;

    public void FindPath(Vector3 startPos, Vector3 endPos, Action<List<Node>> onEnd)
    {
        Queue<Node> queque = new Queue<Node>();
        HashSet<Node> closetSet = new HashSet<Node>();
        bool pathSuccsess = false;

        Node start = _grid.TransformWorldPositionIntoNode(startPos);
        Node end = _grid.TransformWorldPositionIntoNode(endPos);
        // потому, патфаиндинг используем только для фермера и его собаки,
        //а их двигаем без коллизий, а он иногда получает желаемое движение с красного нода
        if ( end.empty) // && start.empty 
        {
            queque.Enqueue(start);
            
            while (queque.Count > 0)
            {
                var currentNode = queque.Dequeue();
                closetSet.Add(currentNode);

                if (currentNode == end)
                {
                    pathSuccsess = true;
                    break;
                }

                foreach (Node neighbor in _grid.WideExploreNeighbors(currentNode))
                {
                    if (!neighbor.empty || closetSet.Contains(neighbor))
                        continue;

                    int distance = currentNode.distance + GetDistance(currentNode, neighbor);
                    if (distance < neighbor.distance || !queque.Contains(neighbor))
                    {
                        neighbor.distance = distance;
                        neighbor.parent = currentNode;

                        if (!queque.Contains(neighbor))
                            queque.Enqueue(neighbor);
                    }
                }
            }
            if (pathSuccsess)
            {
                path = RetracePath(start, end);
                onEnd(path);
            }
        }
    }

    private List<Node> path;
    private List<Node> RetracePath(Node start, Node end)
    {
        List<Node> newPath = new List<Node>();
        var currentNode = end;
        while (currentNode != start)
        {
            newPath.Add(currentNode);
            currentNode = currentNode.parent;
        }
        return newPath;
    }

    public Vector3 GetRandomPosition()
    {
       return _grid.GetRandomNode().position;
    }

    private int GetDistance(Node start, Node end)
    {
        int distX = Mathf.Abs(start.gridX - end.gridX);
        int distY = Mathf.Abs(start.gridY - end.gridY);

        if (distX > distY)
            return 14 * distY + 10 * (distX - distY);
        else
            return 14 * distX + 10 * (distY - distX);
    }
}
