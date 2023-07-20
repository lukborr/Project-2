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
            followPlayer.SlowDown(slowPercent);
            collision.GetComponent<EnemySprite>().TurnSpriteColorBlue();        
        }
    }

    protected override void OnTriggerExit2D(Collider2D collision)
    {
        base.OnTriggerExit2D(collision);
        if (collision.CompareTag("Enemy"))
        {
            FollowPlayer followPlayer = collision.GetComponent<FollowPlayer>();
            followPlayer.speed = followPlayer.enemySO.enemySpeed;
            collision.GetComponent<EnemySprite>().TurnSpriteColorBack();
        }
    }
}
