using System.Collections;
using UnityEngine;

public class Thunderbolt : Skillshot  
{
    private float stunDuration;
    private void Start()
    {
        stunDuration = offensiveSkillSO.skillDuration;
    }

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        base.OnTriggerEnter2D(collision);
       
        if (collision.CompareTag("Enemy"))
        {
            Animator animatorEffect = collision.transform.GetChild(1).GetComponent<Animator>();
            animatorEffect.SetTrigger("isShocked");
           
            if (collision.GetComponent<FollowPlayer>() != null)
            {
                FollowPlayer followplayer = collision.GetComponent<FollowPlayer>();
                Animator animator = collision.GetComponent<Animator>();
                followplayer.StartStunRoutine(followplayer, animator, stunDuration);
            }           
        }       
    }
}