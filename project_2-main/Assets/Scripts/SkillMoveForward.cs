using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillMoveForward : MonoBehaviour
{
    private float speed;
    private Rigidbody2D rb;
    [SerializeField] private OffensiveSkillSO skillSO;

    private void Start()
    {
        speed = skillSO.skillSpeed;
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
