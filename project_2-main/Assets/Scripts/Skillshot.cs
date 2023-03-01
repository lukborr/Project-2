using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skillshot : MonoBehaviour
{
   [SerializeField] private float speed;

    [SerializeField] private OffensiveSkillSO offensiveSkillSO;
    [HideInInspector] public int enemyCountBeforeDestroy;
    [HideInInspector] public int skillDamage;
    [HideInInspector] public float cooldownTime;
    private float skillDuration;

    [SerializeField] private GameObject activeProjectile;
    [SerializeField] private GameObject handGameobject;

    private void OnEnable()
    {
        StartCoroutine(DestroyAfterTime(skillDuration));
    }

    private void Start()
    {
        Debug.Log("tu" + cooldownTime);
    }

    private void Awake()
    {
        enemyCountBeforeDestroy = offensiveSkillSO.enemyCountBeforeDestroy;
        cooldownTime = offensiveSkillSO.skillCooldown;
        
        speed = offensiveSkillSO.skillSpeed;
        skillDamage = offensiveSkillSO.skillDamage;
        skillDuration = offensiveSkillSO.skillDuration;
    }


    // Damage
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            collision.gameObject.GetComponent<Health>().RemoveHealth(skillDamage);  
            Debug.Log("-" +skillDamage+" hp");

            if(enemyCountBeforeDestroy != -1)
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

    IEnumerator DestroyAfterTime(float time)
    {
        yield return new WaitForSeconds(time);
        Destroy(gameObject);
    }

}
