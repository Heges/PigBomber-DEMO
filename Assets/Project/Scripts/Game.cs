using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game : MonoBehaviour
{
    [SerializeField] GameBoard _gameBoard;

    private void Start()
    {
        _gameBoard.StartInit();   
    }
}
