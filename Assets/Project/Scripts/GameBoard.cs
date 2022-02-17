using UnityEngine;
using System.Collections.Generic;

public class GameBoard : MonoBehaviour
{
    [SerializeField] private GridNodes _grid;
    [SerializeField] private PlayerController _player;
    [SerializeField] private Pathfinding _pathfinder;
    [SerializeField] private GameContent _farmer;
    [SerializeField] private GameContent _dog;
    [SerializeField] [Range(0, 1)] private int currentDifficulty = 0;
    [SerializeField] private Joystick _joystick;
    [SerializeField] private BombButton _bombButton;

    List<IDestroeable> destroeable;
    public void StartInit()
    {
        destroeable = new List<IDestroeable>(3);
        _grid.StartInitialize();

        InitializeGamingInstancesLikeFactory();
    }

    public void ReGenerateLevel()
    {
        if(destroeable.Count > 0)
        {
            foreach (var item in destroeable)
            {
                if(item != null)
                {
                    item.OnDie -= ReGenerateLevel;
                    item.Resycle();
                }
            }
            destroeable.Clear();
        }
        InitializeGamingInstancesLikeFactory();
    }

    private void InitializeGamingInstancesLikeFactory()
    {
        var obj = Instantiate(_player);
        obj.transform.position = _grid.GetRandomNode().position;
        obj.OnDie += ReGenerateLevel;
        if(Application.platform == RuntimePlatform.Android)
            obj._joystick = _joystick;
        obj.Subscribe(_bombButton);
        destroeable.Add(obj);

        if(currentDifficulty > 0)
        {
            var obj1 = Instantiate(_farmer);
            obj1.transform.position = _grid.GetRandomNode().position;
            obj1._pathfinder = _pathfinder;
            destroeable.Add(obj1);
            var obj2 = Instantiate(_dog);
            obj2.transform.position = _grid.GetRandomNode().position;
            obj2._pathfinder = _pathfinder;
            destroeable.Add(obj2);
        }
        else
        {
            var obj1 = Instantiate(_farmer);
            obj1.transform.position = _grid.GetRandomNode().position;
            obj1._pathfinder = _pathfinder;
            destroeable.Add(obj1);
        }
    }
}
