using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Experience : MonoBehaviour
{
    private int characterExperience = 0;
    private int characterLevel = 1;
    private float requiredExpToLvlUp = 3;
    private int exp = 3;
    [SerializeField] GameObject rewardMenu;
    [SerializeField] Image expImage;
    [SerializeField] TextMeshProUGUI levelText;

    private void Start()
    {
        expImage.fillAmount = 0;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            GainExperience(2);
        }
    }
    public void GainExperience(int experience)
    {
        characterExperience += experience;
        if (characterExperience >= exp)
        {
            LevelUp();
        }
        ChangeExpBar();
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
        characterExperience = 0;
        requiredExpToLvlUp = requiredExpToLvlUp + requiredExpToLvlUp * 0.33f;
        exp = (int)requiredExpToLvlUp;
        ChangeLevelText();
    }

    private void OpenUpgradeWindow(List<string> skills)
    {
        rewardMenu.SetActive(true);
        if (rewardMenu.GetComponent<RewardMenu>() != null)
        {
            RewardMenu rewardMenuScript = rewardMenu.GetComponent<RewardMenu>();
            List<OffensiveSkillSO> skillsSo = new List<OffensiveSkillSO>();
            for (int i = 0; i < skills.Count; i++)
            {
                OffensiveSkillSO skillSO = Resources.Load<OffensiveSkillSO>("SO/SkillsSO/" + skills[i]);
                Debug.Log(skillSO.skillName);
                skillsSo.Add(skillSO);
            }
            rewardMenuScript.DrawSkills(skillsSo);
        }
    }

    private void ChangeExpBar()
    {
        expImage.fillAmount = characterExperience / requiredExpToLvlUp;
    }

    private void ChangeLevelText()
    {
        levelText.text = $"Level: {characterLevel.ToString()}";
    }

}
