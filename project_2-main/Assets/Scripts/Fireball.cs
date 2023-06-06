using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fireball : Skillshot
{
    private float speed;
    private Rigidbody2D rb;   

    private void Start()
    {
        speed = offensiveSkillSO.skillSpeed;
        rb = GetComponent<Rigidbody2D>();
    }
    private void FixedUpdate()
    {
        float posX = transform.right.x;
        float posY = transform.right.y;
        Vector2 move = new Vector2(posX, posY);
        rb.MovePosition(rb.position + speed * Time.deltaTime * move);
    }
}
