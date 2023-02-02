using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillMoveForward : MonoBehaviour
{
   [SerializeField] private float speed;
    [SerializeField] Transform shootPoint;
    Rigidbody2D rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }


    // Update is called once per frame
    void FixedUpdate()
    {
        float posX = transform.right.x;
        float posY = transform.right.y;
        Vector2 move = new Vector2(posX, posY);

          rb.MovePosition(rb.position + move * speed * Time.deltaTime);
        
    }



    
}
