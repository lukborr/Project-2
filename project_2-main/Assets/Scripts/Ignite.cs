using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ignite : Skillshot
{

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        base.OnTriggerEnter2D(collision);
        if (collision.CompareTag("Enemy"))
        {
            Animator animatorEffect = collision.transform.GetChild(2).GetComponent<Animator>();
            animatorEffect.SetBool("isBurning" , true);
            DotManager dotManager = collision.GetComponent<DotManager>();
            dotManager.StartDotAnimationDisableRoutine(dotDuration - 0.25f, animatorEffect, "isBurning");
        }
        
    }

    protected override void OnTriggerExit2D(Collider2D collision)
    {
       
    }

   

}
