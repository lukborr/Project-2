using System.Collections;
using UnityEngine;



public class Skillshot : MonoBehaviour
{
    [SerializeField] public OffensiveSkillSO offensiveSkillSO;
    [HideInInspector] public int enemyCountBeforeDestroy;
    [HideInInspector] public int skillDamage;
    [HideInInspector] public float cooldownTime;
    [HideInInspector] public float skillDuration;
    [HideInInspector] public float dotDuration;
    [HideInInspector] public float stunDuration;
    [HideInInspector] public float skillSpeed;
    [HideInInspector] public WhereSkillSpawn whereSkillSpawn;
    [HideInInspector] public bool cooldownUp = true;
    [HideInInspector] public float skillRange;
    [HideInInspector] public float slowPercent;
    [HideInInspector] public float slowDuration;
    [HideInInspector] public Vector2 move;
    public int skillLevel;

    public Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        move = new Vector2(transform.right.x, transform.right.y);
       
    }
    private void OnEnable()
    {
        StartCoroutine(DestroyAfterTime(skillDuration));
    }

    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            EnemyHealth health = collision.gameObject.GetComponent<EnemyHealth>();
            if (offensiveSkillSO.skillShotType == SkillShotType.Projectile || offensiveSkillSO.skillShotType == SkillShotType.Aura)
            {
                health.RemoveHealth((Mathf.CeilToInt(skillDamage * GlobalStats.damageMultiplier)));

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
                dotManager.ApplyDotRoutine(offensiveSkillSO.skillName, (Mathf.CeilToInt(skillDamage * GlobalStats.damageMultiplier)), dotDuration);
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

    protected void ProjectileMoveForward()
    {
        
        rb.MovePosition(rb.position + skillSpeed * GlobalStats.projectileSpeedMultiplier * Time.deltaTime * move);
    }

    protected void ShockStun(Collider2D collision)
    {

        if (collision.CompareTag("Enemy"))
        {
            Animator animatorEffect = collision.transform.GetChild(1).GetComponent<Animator>();
            animatorEffect.SetTrigger("isShocked");

            if (collision.GetComponent<FollowPlayer>() != null)
            {
                FollowPlayer followplayer = collision.GetComponent<FollowPlayer>();
                Animator animator = collision.GetComponent<Animator>();
                followplayer.StartStunRoutine(followplayer, animator, stunDuration);
            }
        }

    }

    protected void ShockStun(Collider2D[] colliders)
    {
        foreach (Collider2D collider in colliders)
        {
            if (collider.CompareTag("Enemy"))
            {
                Animator animatorEffect = collider.transform.GetChild(1).GetComponent<Animator>();
                animatorEffect.SetTrigger("isShocked");

                if (collider.GetComponent<FollowPlayer>() != null)
                {
                    FollowPlayer followplayer = collider.GetComponent<FollowPlayer>();
                    Animator animator = collider.GetComponent<Animator>();
                    followplayer.StartStunRoutine(followplayer, animator, stunDuration);
                }
            }
        }
    }

   



}
