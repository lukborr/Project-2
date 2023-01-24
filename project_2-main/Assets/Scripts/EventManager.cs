using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate void PointerDelegate(Vector2 pos);
public static class EventManager 
{
    public static event PointerDelegate PointerEvent;


    public static void CallPointerEvent(Vector2 pos)
    {
        if (PointerEvent != null)
            PointerEvent(pos);
    }
}
