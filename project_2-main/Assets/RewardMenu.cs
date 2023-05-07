using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class RewardMenu : MonoBehaviour
{
    [SerializeField] private Button button0, button1, button2;
    [SerializeField] private GameObject buttonDescription0, buttonDescription1, buttonDescription2;
    [SerializeField] private GameObject buttonName0, buttonName1, buttonName2;
    private OffensiveSkillSO skillSO0;
    private OffensiveSkillSO skillSO1;
    private OffensiveSkillSO skillSO2;

    [SerializeField] private GameObject spawnedEnemies;
    [SerializeField] private PlayerController playerController;

    private List<string> skills = new List<string>();

    private void OnEnable()
    {
        PauseGame();

        skills.Add("Fireball");
        skills.Add("PoisonPool");
        skills.Add("Test");
        skills.Add("Test2");

        DrawSkills();
    }

    private void OnDisable()
    {
        for (int i = 0; i < spawnedEnemies.transform.childCount; i++)
        {
            spawnedEnemies.transform.GetChild(i).GetComponent<FollowPlayer>().enabled = true;
            spawnedEnemies.transform.GetChild(i).GetComponent<Animator>().enabled = true;
        }
        playerController.enabled = true;
    }

    private void PauseGame()
    {
        for (int i = 0; i < spawnedEnemies.transform.childCount; i++)
        {
            spawnedEnemies.transform.GetChild(i).GetComponent<FollowPlayer>().enabled = false;
            spawnedEnemies.transform.GetChild(i).GetComponent<Animator>().enabled = false;
        }
        playerController.enabled = false;
    }

    private void DrawSkills()
    {
        int number0 = Random.Range(0, skills.Count - 1);
        Debug.Log("pierwszy losowy numer to " + number0);
        skillSO0 = Resources.Load<OffensiveSkillSO>("SkillsSO/" + skills[number0]);
        button0.image.sprite = skillSO0.skillSprite;
        buttonName0.GetComponent<TextMeshProUGUI>().text = skillSO0.name;
        buttonDescription0.GetComponent<TextMeshProUGUI>().text = skillSO0.skillDescription;
        skills.RemoveAt(number0);

        int number1 = Random.Range(0, skills.Count - 1);
        Debug.Log("drugi losowy numer to " + number1);
        skillSO1 = Resources.Load<OffensiveSkillSO>("SkillsSO/" + skills[number1]);
        button1.image.sprite = skillSO1.skillSprite;
        buttonName1.GetComponent<TextMeshProUGUI>().text = skillSO1.name;
        buttonDescription1.GetComponent<TextMeshProUGUI>().text = skillSO1.skillDescription;
        skills.RemoveAt(number1);

        int number2 = Random.Range(0, skills.Count - 1);
        Debug.Log("trzeci losowy numer to " + number2);
        skillSO2 = Resources.Load<OffensiveSkillSO>("SkillsSO/" + skills[number2]);
        button2.image.sprite = skillSO2.skillSprite;
        buttonName2.GetComponent<TextMeshProUGUI>().text = skillSO2.name;
        buttonDescription2.GetComponent<TextMeshProUGUI>().text = skillSO2.skillDescription;
        skills.RemoveAt(number2);
    }
}
