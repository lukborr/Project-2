using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class SkillCooldownImage : MonoBehaviour
{
    private void OnEnable()
    {
        EventManager.OnCooldown += StartCooldownRoutine;
    }
    private void OnDisable()
    {
        EventManager.OnCooldown -= StartCooldownRoutine;
    }
    private IEnumerator CooldownRoutine(float seconds, GameObject cooldownGO)
    {
        cooldownGO.gameObject.SetActive(true);
        Image image = cooldownGO.GetComponent<Image>();
        float secondsLeft = seconds;
        for (int i = 0; i < seconds * 4; i++)
        {
            yield return new WaitForSeconds(0.25f);
            secondsLeft -= 0.25f; 
            image.fillAmount = secondsLeft / seconds;
        }
    }

    private void StartCooldownRoutine(float seconds, GameObject go)
    {
        StartCoroutine(CooldownRoutine(seconds, go));
    }
}
