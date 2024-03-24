using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Experience : MonoBehaviour
{
    private int characterExperience = 0;
    private int characterLevel;
    private float requiredExpToLvlUp = 3;
    private int exp = 3;
    [SerializeField] GameObject rewardMenu;

    private void Update()
    {
           if (Input.GetKeyDown(KeyCode.Space))
        {
            GainExperience(1);
        }
    }
    public void GainExperience(int experience)
    {
        characterExperience += experience;
        
        if(characterExperience >= exp)
        {
            LevelUp();
        }    
    }
    private void OnEnable()
    {
        EventManager.GeneratedRandomSkills += OpenUpgradeWindow;
    }

    private void OnDisable()
    {
        EventManager.GeneratedRandomSkills -= OpenUpgradeWindow;
    }

    private void LevelUp()
    {
        EventManager.CallLeveledUpEvent();
        characterLevel++;
        characterExperience= 0;
        requiredExpToLvlUp = requiredExpToLvlUp + requiredExpToLvlUp * 0.33f;
        exp = (int)requiredExpToLvlUp;

        
    }

    private void OpenUpgradeWindow(List<string> skills)
    {
        rewardMenu.SetActive(true);
        if(rewardMenu.GetComponent<RewardMenu>() != null )
        {
            RewardMenu rewardMenuScript = rewardMenu.GetComponent<RewardMenu>();
            List<OffensiveSkillSO> skillsSo = new List<OffensiveSkillSO>();
            for(int i = 0; i < skills.Count; i++)
            {
                OffensiveSkillSO skillSO = Resources.Load<OffensiveSkillSO>("SO/SkillsSO/" + skills[i]);
                Debug.Log(skillSO.skillName);
                skillsSo.Add(skillSO);
            }
            rewardMenuScript.DrawSkills(skillsSo);
        
        }
        
    }
}
