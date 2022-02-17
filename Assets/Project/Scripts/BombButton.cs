using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class BombButton : MonoBehaviour
{
    public Action OnClickButton;

    public void OnClick()
    {
        OnClickButton?.Invoke();
    }
}
