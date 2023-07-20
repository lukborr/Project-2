using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    [SerializeField] GameObject playerObject;
    private Rigidbody2D rb;
    public bool canMove = true;
    public EnemySO enemySO;
    public float speed;

    private void Start()
    {
        playerObject = GameObject.Find("Player");
        rb = GetComponent<Rigidbody2D>();
        speed = enemySO.enemySpeed;
    }

    private void Update()
    {  
        if (playerObject != null && canMove)
        transform.position = Vector2.MoveTowards(this.transform.position, playerObject.transform.position, speed * Time.deltaTime);
    }

   private IEnumerator FreezeSelf()
    {
        yield return new WaitForSeconds(0.4f);
        rb.velocity = Vector2.zero;
        rb.isKinematic = false;
    }

   public void StartFreezeRoutine()
    {
        StartCoroutine(FreezeSelf());       
    }

    private IEnumerator Stun(FollowPlayer followPlayerScript, Animator animator, float duration)
    {
        followPlayerScript.enabled = false;
        animator.enabled = false;
        yield return new WaitForSeconds(duration);
        followPlayerScript.enabled = true;
        animator.enabled = true;
    }

    public void StartStunRoutine(FollowPlayer followPlayerScript, Animator animator, float duration)
    {
        StartCoroutine(Stun(followPlayerScript, animator, duration));
    }

    public void SlowDown(float procent)
    {
        speed = speed * procent;
    }
    public void StartNormalSpeedRoutine(float time)
    {
        StartCoroutine(SetNormalSpeed(time));
    }

    private IEnumerator SetNormalSpeed(float time)
    {
        yield return new WaitForSeconds(time);
        speed = enemySO.enemySpeed;
    }




}
