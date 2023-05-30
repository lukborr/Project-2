using System.Collections;
using UnityEngine;

public class Thunderbolt : Skillshot  
{
    private float stunDuration = 1;
   private IEnumerator Stun(FollowPlayer followPlayerScript, Animator animator)
    {
        followPlayerScript.enabled= false;
        animator.enabled= false;
        yield return new WaitForSeconds(stunDuration);
        followPlayerScript.enabled = true;
        animator.enabled= true; 
    }

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        base.OnTriggerEnter2D(collision);
        if (collision.CompareTag("Enemy"))
        {
            if(collision.GetComponent<FollowPlayer>() != null)
            {
                FollowPlayer followplayer = collision.GetComponent<FollowPlayer>();
                Animator animator = collision.GetComponent<Animator>();
                StartCoroutine(Stun(followplayer, animator));
            }           
        }       
    }
}
