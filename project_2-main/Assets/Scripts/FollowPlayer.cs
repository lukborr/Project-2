using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    [SerializeField] GameObject playerObject;
   [SerializeField] float speed;
    private Rigidbody2D rb;

    private void Start()
    {
        playerObject = GameObject.Find("Player");
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {  
        if (playerObject != null)
        transform.position = Vector2.MoveTowards(this.transform.position, playerObject.transform.position, speed * Time.deltaTime);
    }

   private IEnumerator FreezeSelf()
    {
        yield return new WaitForSeconds(0.3f);
        rb.velocity = Vector2.zero;
    }

    public void StartFreezeRoutine()
    {
        StartCoroutine(FreezeSelf());
    }


   
    
}
