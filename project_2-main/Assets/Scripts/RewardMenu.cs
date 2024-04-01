using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class RewardMenu : MonoBehaviour
{
    [SerializeField] private Button button0, button1, button2;
    [SerializeField] private GameObject buttonDescription0, buttonDescription1, buttonDescription2;
    [SerializeField] private GameObject buttonName0, buttonName1, buttonName2;

    [SerializeField] private GameObject spawnedEnemies;
    [SerializeField] private PlayerController playerController;
    [SerializeField] private SkillManager skillManager;
    [SerializeField] private EnemySpawner enemySpawner;

    private List<string> skillnames = new List<string>();
    

  

    private void OnEnable()
    { 
        PauseGame();  
    }

    private void OnDisable()
    {
        UnPauseGame();
    }

    private void PauseGame()
    {
        for (int i = 0; i < spawnedEnemies.transform.childCount; i++)
        {
            spawnedEnemies.transform.GetChild(i).GetComponent<FollowPlayer>().enabled = false;
            spawnedEnemies.transform.GetChild(i).GetComponent<Animator>().enabled = false;
        }
        playerController.enabled = false;
        enemySpawner.enabled = false;
    }

    public void DrawSkills(List<OffensiveSkillSO> skillz)
    {
        for (int i = 0; i < skillz.Count; i++)
        {
            skillnames.Add(skillz[i].skillName);
        }
        button0.image.sprite = skillz[0].skillSprite;
        buttonName0.GetComponent<TextMeshProUGUI>().text = skillz[0].name;
        buttonDescription0.GetComponent<TextMeshProUGUI>().text = skillz[0].skillDescription;
        
        button1.image.sprite = skillz[1].skillSprite;
        buttonName1.GetComponent<TextMeshProUGUI>().text = skillz[1].name;
        buttonDescription1.GetComponent<TextMeshProUGUI>().text = skillz[1].skillDescription;

        button2.image.sprite = skillz[2].skillSprite;
        buttonName2.GetComponent<TextMeshProUGUI>().text = skillz[2].name;
        buttonDescription2.GetComponent<TextMeshProUGUI>().text = skillz[2].skillDescription;
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
        playerController.enabled = true;
        enemySpawner.enabled = true;
    }
}
