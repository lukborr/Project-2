using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skillshot : MonoBehaviour
{
   [SerializeField] private float speed;

    [SerializeField] private OffensiveSkillSO offensiveSkillSO;
    private int enemyCountBeforeDestroy;
    private bool cooldownUp = true;
    private float cooldownTime;
    

    private void Awake()
    {
        StartCoroutine("DestroyAfterTime");
        
    }

    private void Start()
    {
        enemyCountBeforeDestroy = offensiveSkillSO.enemyCountBeforeDestroy;
        cooldownTime = offensiveSkillSO.skillCooldown;
        speed = offensiveSkillSO.skillSpeed;
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

    IEnumerator ResetCooldown()
    {
        yield return new WaitForSeconds(cooldownTime);
        cooldownUp = true;
    }

    private void OnEnable()
    {
        cooldownUp = false;
        StartCoroutine(ResetCooldown());
    }

}
