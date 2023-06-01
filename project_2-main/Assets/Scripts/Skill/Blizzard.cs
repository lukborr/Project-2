using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blizzard : Skillshot
{
    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        base.OnTriggerEnter2D(collision);
        if (collision.CompareTag("Enemy"))
        {
            FollowPlayer followPlayer= collision.GetComponent<FollowPlayer>();
            followPlayer.speed = followPlayer.speed * 0.75f;
            SpriteRenderer sprite = collision.GetComponent<SpriteRenderer>();
            sprite.color = new Color(0.22f,0.22f,0.92f,1f);            
        }
    }

    protected override void OnTriggerExit2D(Collider2D collision)
    {
        base.OnTriggerExit2D(collision);
        if (collision.CompareTag("Enemy"))
        {
            FollowPlayer followPlayer = collision.GetComponent<FollowPlayer>();
            followPlayer.speed = followPlayer.enemySO.enemySpeed;
            SpriteRenderer sprite = collision.GetComponent<SpriteRenderer>();
            sprite.color = new Color(1f, 1f, 1f, 1f);
        }
    }
}
