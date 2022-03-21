using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[CreateAssetMenu(fileName ="Event/EventVoidAction")]
public class EventVoidAction : ScriptableObject
{
    public Action OnAction;

    public void RaiseEvent()
    {
        OnAction?.Invoke();
    }
}
