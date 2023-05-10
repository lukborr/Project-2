using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    [SerializeField] GameObject playerObject;
    private Rigidbody2D rb;
    public bool canMove = true;
    [SerializeField] private EnemySO enemySO; 

    private void Start()
    {
        playerObject = GameObject.Find("Player");
        rb = GetComponent<Rigidbody2D>();            
    }

    private void Update()
    {  
        if (playerObject != null && canMove)
        transform.position = Vector2.MoveTowards(this.transform.position, playerObject.transform.position, enemySO.enemySpeed * Time.deltaTime);
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





}
