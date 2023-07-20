using System.Collections;
using UnityEngine;

public class FrostBolt : Skillshot
{
    private void FixedUpdate()
    {
        ProjectileMoveForward();
    }

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        base.OnTriggerEnter2D(collision);
        if (collision.CompareTag("Enemy"))
        {
            if(collision.GetComponent<FollowPlayer>() != null) 
            {
                FollowPlayer followPlayerScript = collision.GetComponent<FollowPlayer>();
                followPlayerScript.SlowDown(slowPercent);
                followPlayerScript.StartNormalSpeedRoutine(slowDuration);
                collision.GetComponent<EnemySprite>().TurnSpriteColorBlue(slowDuration);
            }
        }
    }

    
}
