using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skillshot : MonoBehaviour
{
   [SerializeField] private float speed;
    Rigidbody2D rb;
    [SerializeField] private OffensiveSkillSO offensiveSkillSO;
    private int enemyCountBeforeDestroy;

    private void Awake()
    {
        StartCoroutine("DestroyAfterTime");
    }

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        enemyCountBeforeDestroy = offensiveSkillSO.enemyCountBeforeDestroy;
        speed = offensiveSkillSO.skillSpeed;
    }

    void FixedUpdate()
    {
        float posX = transform.right.x;
        float posY = transform.right.y;
        Vector2 move = new Vector2(posX, posY);

        rb.MovePosition(rb.position + speed * Time.deltaTime * move);
        
    }
    // Damage
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            collision.gameObject.GetComponent<Health>().RemoveHealth(offensiveSkillSO.skillDamage);  
            Debug.Log("-" + offensiveSkillSO.skillDamage +" hp");

            if(offensiveSkillSO.enemyCountBeforeDestroy != -1)
            {
                enemyCountBeforeDestroy--;
                if (enemyCountBeforeDestroy <= 0)
                {
                    Debug.Log("tu weszlo");
                    Destroy(gameObject);
                }
                    
            }

        }
    }
    // Destroy after time

    IEnumerator DestroyAfterTime()
    {
        yield return new WaitForSeconds(offensiveSkillSO.skillDuration);
        Destroy(gameObject);
    }




}
