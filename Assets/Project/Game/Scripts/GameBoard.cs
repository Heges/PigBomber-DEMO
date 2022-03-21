using UnityEngine;
using System.Collections.Generic;

public class GameBoard : MonoBehaviour
{
    [SerializeField] private GridNodes grid;
    [SerializeField] private PlayerController playerPrefub;
    [SerializeField] private Pathfinding pathfinder;
    [SerializeField] private Enemy farmerPrefub;
    [SerializeField] private Enemy dogPrefub;
    [SerializeField] [Range(0, 1)] private int currentDifficulty = 0;
    [SerializeField] private Joystick joystick;
    [SerializeField] private BombButtonUI bombButton;
    [SerializeField] private Factory factory;

    private List<WorldObject> activeWorldObjects;
    public void StartInit()
    {
        activeWorldObjects = new List<WorldObject>(3);
        grid.StartInitialize();
        pathfinder.Initialize(grid);
        InitializeGamingInstances();
    }

    public void ReGenerateLevel()
    {
        if(activeWorldObjects.Count > 0)
        {
            foreach (var item in activeWorldObjects)
            {
                if(item != null)
                {
                    item.Reclaim();
                }
            }
            activeWorldObjects.Clear();
        }
        InitializeGamingInstances();
    }

    private WorldObject GetInstance(string name)
    {
        var obj = factory.Get(name);
        obj.transform.position = grid.GetRandomNode().position;
        activeWorldObjects.Add(obj);
        return obj;
    }

    private void InitializeGamingInstances()
    {
        var obj = GetInstance("player");
        var player = obj.GetComponent<PlayerController>();
        if (Application.platform == RuntimePlatform.Android)
            player.joystick = joystick;
        player.OnDie += ReGenerateLevel;

        if (currentDifficulty > 0)
        {
            var farmer = GetInstance("farmer");

            var dog = GetInstance("dog");
        }
        else
        {
            var farmer = GetInstance("farmer");
        }
    }
}
