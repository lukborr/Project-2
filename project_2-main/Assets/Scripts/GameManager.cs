using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] InputManager skillmanager;
    [SerializeField] Transform projectilesGo;

    private void DisableSkillshot()
    {
        for (int i = 0; i < projectilesGo.childCount ; i++)
        {
            GameObject childobj = projectilesGo.GetChild(i).gameObject;
            if(childobj.GetComponent<Skillshot>() != null)
            {
                Skillshot skillshot = childobj.GetComponent<Skillshot>();
                skillshot.enabled = false;
            }
        }
    }

    private void EnableSkillshot()
    {
        for (int i = 0; i < projectilesGo.childCount; i++)
        {
            GameObject childobj = projectilesGo.GetChild(i).gameObject;
            if (childobj.GetComponent<Skillshot>() != null)
            {
                Skillshot skillshot = childobj.GetComponent<Skillshot>();
                skillshot.enabled = true;
            }
        }
    }

    private void DisableSkillManager()
    {
        skillmanager.enabled= false;
    }
    private void EnableSkillManager()
    {
        StartCoroutine(DelaySkill());
    }

    private void OnEnable()
    {
        EventManager.OnGamePaused += DisableSkillManager;
        EventManager.OnGamePaused += DisableSkillshot;
        EventManager.OnGameResumed += EnableSkillManager;
        EventManager.OnGameResumed += EnableSkillshot;
    }

    private void OnDisable()
    {
        EventManager.OnGamePaused -= DisableSkillManager;
        EventManager.OnGameResumed -= EnableSkillManager;
        EventManager.OnGamePaused -= DisableSkillshot;
        EventManager.OnGameResumed -= EnableSkillshot;
    }

    private IEnumerator DelaySkill()
    {
        yield return new WaitForSeconds(0.25f);
        skillmanager.enabled = true;
    }
}

