using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] InputManager skillmanager;
   
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
        EventManager.OnGameResumed += EnableSkillManager;
    }

    private void OnDisable()
    {
        EventManager.OnGamePaused -= DisableSkillManager;
        EventManager.OnGameResumed -= EnableSkillManager;
    }

    private IEnumerator DelaySkill()
    {
        yield return new WaitForSeconds(0.15f);
        skillmanager.enabled = true;
    }
}

