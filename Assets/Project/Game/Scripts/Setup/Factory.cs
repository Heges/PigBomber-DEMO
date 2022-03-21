using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="SimpleFactory")]
public class Factory : ScriptableObject
{
    [SerializeField] private WorldObject farmer;
    [SerializeField] private WorldObject dog;
    [SerializeField] private WorldObject player;

    public WorldObject Get(string name)
    {
        switch (name)
        {
            case "player":
                return Get(player);
            case "farmer":
                return Get(farmer);
            case "dog":
                return Get(dog);
            default:
                break;
        }
        return null;
    }

    public void Resycle(WorldObject obj)
    {
        Destroy(obj.gameObject);
    }

    private WorldObject Get(WorldObject prefub)
    {
        var obj = Instantiate(prefub);
        obj.Factory = this;
        return obj;
    }
}
