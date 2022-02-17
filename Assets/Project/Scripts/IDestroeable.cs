using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public interface IDestroeable
{
    event Action OnDie;
    void Resycle();
}
