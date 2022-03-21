using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node 
{
    public Vector3 position;
    public bool empty;
    public int gridX;
    public int gridY;

    public int distance;
    public Node parent;

    public Node(Vector3 _pos, bool _empty, int _gridX, int _gridY)
    {
        position = _pos;
        empty = _empty;
        gridX = _gridX;
        gridY = _gridY;
    }
}
