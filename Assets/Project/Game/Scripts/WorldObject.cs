using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class WorldObject : MonoBehaviour, IDamageable
{
    public Factory Factory { get; set; }
    public bool Dirty { get; set; }

    public void Reclaim()
    {
        Factory.Resycle(this);
    }

    public virtual void MakeYouDirtyOrCleanUP()
    {
        
    }
}
