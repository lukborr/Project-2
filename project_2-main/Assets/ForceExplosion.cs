using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForceExplosion : Skillshot
{

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
           Vector2 enemyPos = collision.transform.position;
            Vector2 skillPos = transform.position;
            Vector2  forceDirection = enemyPos - skillPos;
            collision.GetComponent<Rigidbody2D>().AddForce(forceDirection, ForceMode2D.Impulse);
           
        }
    }
}
