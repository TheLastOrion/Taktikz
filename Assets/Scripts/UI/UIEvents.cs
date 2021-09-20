using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIEvents
{
    public static event Action<bool> GridVisibilityChanged;


    public static void FireGridVisibilityChanged(bool isOn)
    {
        if (GridVisibilityChanged != null)
        {
            GridVisibilityChanged(isOn);
        }
    }
}
