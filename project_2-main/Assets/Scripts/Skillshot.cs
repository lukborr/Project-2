using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class Skillshot : MonoBehaviour
{

    [SerializeField] private OffensiveSkillSO offensiveSkillSO;
    [HideInInspector] public int enemyCountBeforeDestroy;
    [HideInInspector] public int skillDamage;
    [HideInInspector] public float cooldownTime;
    private float skillDuration;

    private Coroutine dotRoutine;

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
        skillDamage = offensiveSkillSO.skillDamage;
        skillDuration = offensiveSkillSO.skillDuration;
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
