using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForcePush : Skillshot
{
    [SerializeField] private Transform point1;

    [SerializeField] private Transform point2;
    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        base.OnTriggerEnter2D(collision);
        if (collision.CompareTag("Enemy"))
        {
            FollowPlayer followPlayerScript = collision.GetComponent<FollowPlayer>();
            var enemyRb = collision.GetComponent<Rigidbody2D>();
            float dist = Vector2.Distance(collision.transform.position, point1.position);
            collision.GetComponent<Rigidbody2D>().AddForce(point2.position - point1.position  / dist, ForceMode2D.Impulse);         
            enemyRb.isKinematic = true;
            followPlayerScript.StartFreezeRoutine();           
        }        
    }
      
}
