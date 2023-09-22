using UnityEngine;

public delegate void PointerDelegate(Vector2 pos);
public delegate void MouseButton0Delegate();
public delegate void HealthChangeDelegate(int health);
public static class EventManager 
{
    public static event PointerDelegate PointerEvent;
    public static event MouseButton0Delegate MouseButton0;
    public static event HealthChangeDelegate HealthChange;


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
    public static void CallHealthChangeEvent(int health)
    {
        if(HealthChange != null)
        {
            HealthChange(health);
        }
    }
}
