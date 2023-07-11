using System.Collections;
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
    [HideInInspector] public float stunDuration;
    [HideInInspector] public float skillSpeed;
    [HideInInspector] public WhereSkillSpawn whereSkillSpawn;
    [HideInInspector] public bool cooldownUp = true;
    [HideInInspector] public float skillRange;

    public Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    private void OnEnable()
    {
        StartCoroutine(DestroyAfterTime(skillDuration));
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

    protected void ProjectileMoveForward()
    {
        Vector2 move = new Vector2(transform.right.x, transform.right.y);
        rb.MovePosition(rb.position + skillSpeed * Time.deltaTime * move);
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
