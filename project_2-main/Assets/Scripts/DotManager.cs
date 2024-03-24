using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DotManager : MonoBehaviour
{
    Dictionary<string, Coroutine> activeRoutinesDictionary = new Dictionary<string, Coroutine>();
    Health healthScript;

    private void Start()
    {
        healthScript = GetComponent<Health>();
        
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.T)) 
        {
            for (int i = 0; i < activeRoutinesDictionary.Count; i++)
            {
                Debug.Log(activeRoutinesDictionary.Keys);
            }        
        }
        if (Input.GetKeyDown(KeyCode.K))
        {
            ApplyDotRoutine("Storm", 1, 8);
        }
    }
    /// <summary>
    /// Takes name of the skill, amount of health to remove and duration of the DoT and then removes it gradually
    /// </summary>
    public IEnumerator RemoveHealthGradually(string skillName,int healthToRemove, float duration)
    {
        
        if (duration != -1)
        {
            StartCoroutine(Timer(duration));
        }
        while (duration >0 && healthScript.health > 0)
        {
            duration -= 0.5f;
            healthScript.health -= healthToRemove;
            healthScript.InstantiateDamageOutput(healthToRemove);
            
            if (healthScript.health <= 0)
            {
                healthScript.DieAndDrop();
            }else if(duration <= 0)
            {
               RemoveDotRoutine(skillName);
            }
            yield return new WaitForSeconds(0.5f);
        }
    }

    private IEnumerator Timer(float duration)
    {
        while (duration > 0)
        {
            yield return new WaitForSeconds(1);
            duration--;
        }
    }

    public void ApplyDotRoutine(string skillName,int damage, float duration)
    {
        if (activeRoutinesDictionary.ContainsKey(skillName))
        {
            RemoveDotRoutine(skillName);
        }      
            Coroutine dotRoutine = StartCoroutine(RemoveHealthGradually(skillName, damage, duration));
            activeRoutinesDictionary.Add(skillName, dotRoutine);                  
    }

    public void RemoveDotRoutine(string dotSkillName)
    {
        StopCoroutine(activeRoutinesDictionary[dotSkillName]);
        activeRoutinesDictionary.Remove(dotSkillName);
    }

   private IEnumerator DisableDotAnimation(float time, Animator animator, string boolName)
    {
        yield return new WaitForSeconds(time);
        animator.SetBool("isBurning", false);
    }

    public void StartDotAnimationDisableRoutine(float time, Animator animator, string boolName)
    {
        StartCoroutine(DisableDotAnimation(time, animator, boolName));
    }

}
