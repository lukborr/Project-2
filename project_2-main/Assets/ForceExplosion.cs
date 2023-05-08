using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForceExplosion : Skillshot
{
    SpriteRenderer spriteRenderer;
    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        base.OnTriggerEnter2D(collision);
        
        if (collision.CompareTag("Enemy"))
        {          
           Vector2 enemyPos = collision.transform.position;
            Vector2 skillPos = transform.position;
            Vector2 forceDirection = enemyPos - (Vector2)spriteRenderer.bounds.center; 
            var rb = collision.GetComponent<Rigidbody2D>();
            rb.AddForce(forceDirection * 6, ForceMode2D.Impulse);
            FollowPlayer followPlayerScript = collision.GetComponent<FollowPlayer>();
            followPlayerScript.StartFreezeRoutine();
        }
    }

   

    

}
