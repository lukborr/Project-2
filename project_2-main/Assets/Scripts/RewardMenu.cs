using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class RewardMenu : MonoBehaviour
{
    [SerializeField] private Button button0, button1, button2;
    
    [SerializeField] private GameObject buttonDescription0, buttonDescription1, buttonDescription2;
    [SerializeField] private GameObject buttonName0, buttonName1, buttonName2;
    private Button[] buttons = new Button[3];
    private GameObject[] buttonNames = new GameObject[3];
    private GameObject[] buttonDescriptions = new GameObject[3];
    [SerializeField] private GameObject spawnedEnemies;
    [SerializeField] private PlayerController playerController;
    [SerializeField] private SkillManager skillManager;


    private List<string> skillnames = new List<string>();


    private void OnEnable()
    { 
        PauseGame();  
    }

    private void OnDisable()
    {        
        UnPauseGame();
    }
    private void Awake()
    {
        buttons[0] = button0;
        buttons[1] = button1;   
        buttons[2] = button2;
        buttonNames[0] = buttonName0;
        buttonNames[1] = buttonName1;
        buttonNames[2] = buttonName2;
        buttonDescriptions[0] = buttonDescription0;
        buttonDescriptions[1] = buttonDescription1;
        buttonDescriptions[2] = buttonDescription2;
    }

    private void PauseGame()
    {
        EventManager.CallOnGamePausedEvent();
        for (int i = 0; i < spawnedEnemies.transform.childCount; i++)
        {
            spawnedEnemies.transform.GetChild(i).GetComponent<FollowPlayer>().enabled = false;
            spawnedEnemies.transform.GetChild(i).GetComponent<Animator>().enabled = false;
        }
        playerController.enabled = false;
        
    }

    public void DrawSkills(List<string> skillz)
    {
        for (int i = 0; i < skillz.Count; i++)
        {
            var so = Resources.Load("SO/SkillsSO/Offensive/" + skillz[i]);           
            if(so != null)
            {
                OffensiveSkillSO activeSkillSO = (OffensiveSkillSO)so;
                buttons[i].image.sprite = activeSkillSO.skillSprite;
                buttonNames[i].GetComponent<TextMeshProUGUI>().text = activeSkillSO.skillName;
                buttonDescriptions[i].GetComponent<TextMeshProUGUI>().text = activeSkillSO?.skillDescription;
                skillnames.Add(activeSkillSO.skillName);

            }
            else
            {
                so = Resources.Load("SO/SkillsSO/Passive/" + skillz[i]);
                if (so != null)
                {
                    PassiveSkillSO passiveSkillSO = (PassiveSkillSO)so;
                    buttons[i].image.sprite = passiveSkillSO.skillSprite;
                    buttonNames[i].GetComponent<TextMeshProUGUI>().text = passiveSkillSO.skillName;
                    buttonDescriptions[i].GetComponent<TextMeshProUGUI>().text = passiveSkillSO?.skillDescription;
                    skillnames.Add(passiveSkillSO.skillName);

                }
            }
            //skillnames.Add(skillz[i].skillName);
        }
      
    }

    public void ButtonClick(int number)
    {
        var skillName = skillnames[number];
        EventManager.CallOnButtonClickedEvent(skillName);
        skillnames.Clear();
    }

    private void UnPauseGame()
    {       
        for (int i = 0; i < spawnedEnemies.transform.childCount; i++)
        {
            spawnedEnemies.transform.GetChild(i).GetComponent<FollowPlayer>().enabled = true;
            spawnedEnemies.transform.GetChild(i).GetComponent<Animator>().enabled = true;
        }       
        EventManager.CallOnGameResumedEvent();
        playerController.enabled = true;
    }
}
