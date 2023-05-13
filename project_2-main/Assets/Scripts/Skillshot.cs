using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(SpriteRenderer))]

public class Skillshot : MonoBehaviour
{

    [SerializeField] private OffensiveSkillSO offensiveSkillSO;
    [HideInInspector] public int enemyCountBeforeDestroy;
    [HideInInspector] public int skillDamage;
    [HideInInspector] public float cooldownTime;
    [HideInInspector] public float skillDuration;
    [HideInInspector] public float skillSpeed;
    [HideInInspector] public  WhereSkillSpawn whereSkillSpawn;
    [HideInInspector] public bool cooldownUp = true;

    private List<Health> healthsList= new List<Health>();

    private void OnEnable()
    {
        StartCoroutine(DestroyAfterTime(skillDuration));
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            Debug.Log(healthsList.Count);
        }
    }

    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {  
        if (collision.CompareTag("Enemy"))
        {
            Health health = collision.gameObject.GetComponent<Health>();
            if (offensiveSkillSO.skillShotType == SkillShotType.Projectile)
            {              
                health.RemoveHealth(skillDamage);

                if (enemyCountBeforeDestroy != -1)
                {
                    enemyCountBeforeDestroy--;
                    if (enemyCountBeforeDestroy <= 0)
                    {
                        Destroy(gameObject);
                    }
                }
            }
            else if (offensiveSkillSO.skillShotType == SkillShotType.Dot)
            {              
              health.StartDotRoutine(skillDamage);               
            }
        }
    }
  

    protected virtual void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy")  )
        {
            Health health = collision.gameObject.GetComponent<Health>();
            if(health.dotRoutine !=null)
            health.StopCoroutine(health.dotRoutine);
        }                
    }

    IEnumerator DestroyAfterTime(float time)
    {
        yield return new WaitForSeconds(time);
        Destroy(gameObject);
    }
    public IEnumerator ResetCooldown(float time)
    {
        yield return new WaitForSeconds(time);
        cooldownUp = true;
    }

    

}
