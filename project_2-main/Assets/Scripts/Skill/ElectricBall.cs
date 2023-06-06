using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElectricBall : Skillshot
{
    [SerializeField] private GameObject rotationPoint;
    private float stunDuration;
    private void Start()
    {
        stunDuration = offensiveSkillSO.skillDuration;
        skillDamage= offensiveSkillSO.skillDamage;
    }

    private void FixedUpdate()
    {
       rotationPoint.transform.Rotate(new Vector3(0, 0, 1) * skillSpeed * Time.fixedDeltaTime);
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
