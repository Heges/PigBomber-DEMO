using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game : MonoBehaviour
{
    [SerializeField] private GameBoard gameBoard;

    private void Start()
    {
        gameBoard.StartInit();   
    }
}
