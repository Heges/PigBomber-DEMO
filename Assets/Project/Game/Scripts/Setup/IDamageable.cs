using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public interface IDamageable
{
    bool Dirty { get; set; }

    void MakeYouDirtyOrCleanUP();
}
