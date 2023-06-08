using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(SpriteRenderer))]

public class Skillshot : MonoBehaviour
{

    [SerializeField] public OffensiveSkillSO offensiveSkillSO;
    [HideInInspector] public int enemyCountBeforeDestroy;
    [HideInInspector] public int skillDamage;
    [HideInInspector] public float cooldownTime;
    [HideInInspector] public float skillDuration;
    [HideInInspector] public float dotDuration;
    [HideInInspector] public float skillSpeed;
    [HideInInspector] public  WhereSkillSpawn whereSkillSpawn;
    [HideInInspector] public bool cooldownUp = true;
    [HideInInspector] public float skillRange;

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
            if (offensiveSkillSO.skillShotType == SkillShotType.Projectile || offensiveSkillSO.skillShotType == SkillShotType.Aura)
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
            else if (offensiveSkillSO.skillShotType == SkillShotType.Dot || (offensiveSkillSO.skillShotType == SkillShotType.DotStick))
            {
                DotManager dotManager = collision.GetComponent<DotManager>();               
                dotManager.ApplyDotRoutine(offensiveSkillSO.skillName, skillDamage, dotDuration);
            }
        }
    }
  

    protected virtual void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            if (offensiveSkillSO.skillShotType == SkillShotType.Dot)
            {
                DotManager dotManager = collision.GetComponent<DotManager>();
                dotManager.RemoveDotRoutine(offensiveSkillSO.skillName);
            }
          
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
