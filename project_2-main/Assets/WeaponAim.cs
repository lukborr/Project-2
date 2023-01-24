using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponAim : MonoBehaviour
{
   private void RotateWeapon(Vector2 pointerPosition)
    {
        var direction = (Vector3)pointerPosition - transform.position;
        var desiredAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(desiredAngle, Vector3.forward);
    }

    private void OnEnable()
    {
        EventManager.PointerEvent += RotateWeapon;
    }

    private void OnDisable()
    {
        EventManager.PointerEvent -= RotateWeapon;
    }
}
