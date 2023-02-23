using UnityEngine;

public delegate void PointerDelegate(Vector2 pos);
public delegate void MouseButton0Delegate();
public static class EventManager 
{
    public static event PointerDelegate PointerEvent;
    public static event MouseButton0Delegate MouseButton0;


    public static void CallPointerEvent(Vector2 pos)
    {
        if (PointerEvent != null)
            PointerEvent(pos);
    }

    public static void CallMouseButton0()
    {
        if(MouseButton0 != null)
        {
            MouseButton0();
        }
    }
}
