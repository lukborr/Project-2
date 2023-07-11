using System.Collections;
using UnityEngine;

public class BurningGround : MonoBehaviour
{
    private float dotDuration;
    private float skillDuration;
    [SerializeField] private OffensiveSkillSO skillSO;

    private  void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            Animator animatorEffect = collision.transform.GetChild(2).GetComponent<Animator>();
            animatorEffect.SetBool("isBurning", true);
            DotManager dotManager = collision.GetComponent<DotManager>();
            dotManager.StartDotAnimationDisableRoutine(dotDuration - 0.25f, animatorEffect, "isBurning");
        }
    }

    private void OnEnable()
    {
        Debug.Log("duration to" + skillDuration);
        StartCoroutine(DestroyAfterTime(skillDuration));
    }

   private IEnumerator DestroyAfterTime(float time)
    {
        yield return new WaitForSeconds(time);
        Destroy(gameObject);
    }
}
