using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteFlip : MonoBehaviour
{
    SpriteRenderer spriteRenderer;


    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
    private void FaceDirection(Vector2 pointerInput)
    {
        var direction = (Vector3)pointerInput - transform.position;
        var result = Vector3.Cross(Vector2.up, direction);

        if (result.z < 0 )
        {
            spriteRenderer.flipX = false;
        }
        else if(result.z > 0)
        {
            spriteRenderer.flipX = true;
        }
    }

    private void OnEnable()
    {
        EventManager.PointerEvent += FaceDirection;
    }

    private void OnDisable()
    {
        EventManager.PointerEvent -= FaceDirection;
    }
}
