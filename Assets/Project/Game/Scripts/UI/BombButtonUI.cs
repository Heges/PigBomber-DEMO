using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class BombButtonUI : MonoBehaviour
{
    [SerializeField] private EventVoidAction OnClickAction;

    public void OnClick()
    {
        OnClickAction?.RaiseEvent();
    }
}
