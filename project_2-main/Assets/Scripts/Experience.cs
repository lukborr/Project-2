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


    public void GainExperience(int experience)
    {
        characterExperience += experience;
        
        if(characterExperience >= exp)
        {
            LevelUp();
        }    
    }

    private void LevelUp()
    {
        characterLevel++;
        characterExperience= 0;
        requiredExpToLvlUp = requiredExpToLvlUp + requiredExpToLvlUp * 0.33f;
        exp = (int)requiredExpToLvlUp;
        Debug.Log(exp);

        OpenUpgradeWindow();
    }

    private void OpenUpgradeWindow()
    {
        rewardMenu.SetActive(true);
        Debug.Log("lvl up");
    }
}
