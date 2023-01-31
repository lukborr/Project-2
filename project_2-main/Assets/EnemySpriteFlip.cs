using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemySpriteFlip : MonoBehaviour
{
    Vector2 currentPos;
    Vector2 nextPos;
    SpriteRenderer spriteRenderer;

    private void Start()
    {
        spriteRenderer= GetComponent<SpriteRenderer>();
        StartCoroutine("CheckPos");
    }

    IEnumerator CheckPos()
    {
        
        currentPos = transform.position;
        yield return new WaitForEndOfFrame();          
        nextPos = transform.position;
        if (nextPos.x > currentPos.x)
        {
            spriteRenderer.flipX = true;
        }
        else if (nextPos.x < currentPos.x)
        {
            spriteRenderer.flipX = false;
        }
        StartCoroutine("CheckPos");
    }
}
