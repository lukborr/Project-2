using System.Collections.Generic;
using UnityEngine;

public delegate void PointerDelegate(Vector2 pos);
public delegate void MouseButton0Delegate();
public delegate void HealthChangeDelegate(int health);
public delegate void LeveledUpDelegate();
public delegate void GeneratedRandomSkillsDelegate(List<string> skills);
public delegate void OnButtonClickedDelegate(string skillToUpgrade);
public static class EventManager 
{
    public static event PointerDelegate PointerEvent;
    public static event MouseButton0Delegate MouseButton0;
    public static event HealthChangeDelegate HealthChange;
    public static event LeveledUpDelegate LeveledUp;
    public static event GeneratedRandomSkillsDelegate GeneratedRandomSkills;
    public static event OnButtonClickedDelegate OnButtonClicked;

    public static void CallOnButtonClickedEvent(string skillToUpgrade)
    {
        if (OnButtonClicked != null)
        {
            OnButtonClicked(skillToUpgrade);
        }
    }

    public static void CallGeneratedRandomSkillsEvent(List<string> skills)
    {
        if(GeneratedRandomSkills != null)
        {
            GeneratedRandomSkills(skills);
        }
    }
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
    public static void CallLeveledUpEvent()
    {
        if(LeveledUp != null)
        {
            LeveledUp();
        }
    }
}
