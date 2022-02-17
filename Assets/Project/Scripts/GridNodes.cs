using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridNodes : MonoBehaviour
{
    public Vector2 _gridSize;
    [SerializeField] float nodeRadius;
    [SerializeField] LayerMask _unwalkable;

    private float nodeDiameter;
    private int gridCountX;
    private int gridCountY;
    private Node[,] gridNodes; 

    private void CreateGrid()
    {
        gridNodes = new Node[gridCountX, gridCountY];

        Vector3 leftWorldGridPosition = transform.position - Vector3.right * _gridSize.x / 2 - transform.position - Vector3.up * _gridSize.y / 2f;
        for(int x = 0; x < gridCountX; x++)
        {
            for(int y = 0; y < gridCountY; y++)
            {
                Vector3 pos = leftWorldGridPosition + Vector3.right * (x * nodeDiameter + nodeRadius) + Vector3.up * (y * nodeDiameter + nodeRadius);
                bool empty = !Physics.CheckSphere(pos, nodeRadius, _unwalkable);
                gridNodes[x, y] = new Node(pos, empty, x,y);
            }
        }
        mapIsReady = true;
    }

    public List<Node> WideExploreNeighbors(Node current)
    {
        List<Node> neighbors = new List<Node>(gridCountX * gridCountY);
        for(int i = -1; i <= 1; i++)
        {
            for (int j = -1; j <= 1; j++)
            {
                if (i == 0 && j == 0)
                    continue;
                int x = current.gridX + i;
                int y = current.gridY + j;
                if(x >= 0 && x < gridCountX && y >= 0 && y < gridCountY)
                {
                    neighbors.Add(gridNodes[x, y]);
                }
            }
        }
        return neighbors;
    }

    public Node TransformWorldPositionIntoNode(Vector3 worldPosition)
    {
        float percentegeX = (worldPosition.x + _gridSize.x / 2f) / _gridSize.x;
        float percentegeY = (worldPosition.y + _gridSize.y / 2f) / _gridSize.y;

        percentegeX = Mathf.Clamp01(percentegeX);
        percentegeY = Mathf.Clamp01(percentegeY);

        int x = Mathf.RoundToInt((gridCountX - 1) * percentegeX);
        int y = Mathf.RoundToInt((gridCountY - 1) * percentegeY);
        return gridNodes[x, y];
    }

    public void StartInitialize()
    {
        nodeDiameter = nodeRadius * 2;
        gridCountX = Mathf.RoundToInt(_gridSize.x / nodeDiameter);
        gridCountY = Mathf.RoundToInt(_gridSize.y / nodeDiameter);
        CreateGrid();
    }

    public Node GetRandomNode()
    {
        int x = Random.Range(0, gridCountX);
        int y = Random.Range(0, gridCountY);
        Node currentNode = gridNodes[x, y];
        if (gridNodes[x, y].empty)
        {
            return currentNode;
        }
            
        else
        {
            while (!currentNode.empty)
            {
                 x = Random.Range(0, gridCountX);
                 y = Random.Range(0, gridCountY);
                 currentNode = gridNodes[x, y];
            }
            return currentNode;
        }
    }

    #region GIZMOS
    [SerializeField] private bool shouldDrawWireMap;
    private bool mapIsReady;
    public List<Node> path;
    private void OnDrawGizmos()
    {
        if (shouldDrawWireMap)
        {
            Gizmos.DrawWireCube(transform.position, _gridSize);
            if (mapIsReady)
            {
                for (int x = 0; x < gridCountX; x++)
                {
                    for (int y = 0; y < gridCountY; y++)
                    {
                        var cell = gridNodes[x, y];
                        Gizmos.color = cell.empty ? Color.white : Color.red;
                        Gizmos.DrawCube(cell.position, Vector3.one * (nodeDiameter - .1f));
                    }
                }
                if (path != null)
                {
                    Gizmos.color = Color.yellow;
                    
                    foreach (Node item in path)
                    {
                        Gizmos.DrawCube(item.position, Vector3.one * (nodeDiameter - .1f));
                    }
                }
            }
        }
        
    }
    #endregion
}
