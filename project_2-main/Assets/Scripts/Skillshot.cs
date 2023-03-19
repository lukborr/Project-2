using System.Collections;
using UnityEngine;

public class Skillshot : MonoBehaviour
{

    [SerializeField] private OffensiveSkillSO offensiveSkillSO;
    [HideInInspector] public int enemyCountBeforeDestroy;
    [HideInInspector] public int skillDamage;
    [HideInInspector] public float cooldownTime;
    [HideInInspector] public float skillDuration;
    [HideInInspector] public float skillSpeed;
    [HideInInspector] public  WhereSkillSpawn whereSkillSpawn;


    private Coroutine dotRoutine;

    private void OnEnable()
    {
        StartCoroutine(DestroyAfterTime(skillDuration));
    }

    // Damage
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            Health health = collision.gameObject.GetComponent<Health>();
            if (offensiveSkillSO.skillShotType == SkillShotType.Projectile)
            {
                health.RemoveHealth(skillDamage);
                Debug.Log(skillDamage);

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

                dotRoutine = StartCoroutine(health.RemoveHealthGradually(skillDamage));
                Debug.Log(skillDamage);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)

    {
        if(offensiveSkillSO.skillShotType == SkillShotType.Dot && dotRoutine != null)
        {
            StopCoroutine(dotRoutine);
        }
    }
    // Destroy after time

    IEnumerator DestroyAfterTime(float time)
    {
        yield return new WaitForSeconds(time);
        Destroy(gameObject);
    }

}
