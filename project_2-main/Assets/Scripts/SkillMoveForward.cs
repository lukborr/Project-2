using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillMoveForward : MonoBehaviour
{
   [SerializeField] private float speed;
    Rigidbody2D rb;
    [SerializeField] Transform shootPoint;


    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();        
        gameObject.SetActive(false);
        
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
