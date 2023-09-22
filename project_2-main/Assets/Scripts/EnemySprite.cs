using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(FollowPlayer))]
[RequireComponent(typeof(Health))]
[RequireComponent(typeof(FollowPlayer))]
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(DotManager))]
[RequireComponent(typeof(SpriteRenderer))]

public class EnemySprite : MonoBehaviour
{
    Vector2 currentPos;
    Vector2 nextPos;
    SpriteRenderer spriteRenderer;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
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

    public void TurnSpriteColorBlue()
    {     
        spriteRenderer.color = new Color(0.22f, 0.22f, 0.92f, 1f);
    }

    public void TurnSpriteColorBlue(float time)
    {
        TurnSpriteColorBlue();
        StartCoroutine(TurnSpriteColorBackRoutine(time));
    }
    public void TurnSpriteColorBack()
    {
        spriteRenderer.color = new Color(1f, 1f, 1f, 1f);
    }

    private IEnumerator TurnSpriteColorBackRoutine(float time)
    {
        yield return new WaitForSeconds(time);
        TurnSpriteColorBack();
    }
}
  
