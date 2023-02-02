using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    [SerializeField] GameObject skill;
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Mouse0))
        {
            EventManager.CallMouseButton0();
        }
    }
}
