using System.Collections;
using UnityEngine;

public class BurningGround : Skillshot
{
    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        base.OnTriggerEnter2D(collision);
        if (collision.CompareTag("Enemy"))
        {
            
            Animator animatorEffect = collision.transform.GetChild(2).GetComponent<Animator>();
            animatorEffect.SetBool("isBurning", true);
            DotManager dotManager = collision.GetComponent<DotManager>();
            dotManager.StartDotAnimationDisableRoutine(dotDuration - 0.25f, animatorEffect, "isBurning");
        }
    }

 
}
