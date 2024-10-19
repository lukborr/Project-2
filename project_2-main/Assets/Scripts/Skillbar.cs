using UnityEngine;
using UnityEngine.UI;


public class Skillbar : MonoBehaviour
{
   [SerializeField] private GameObject[] skillIcons;

    private void OnEnable()
    {
        EventManager.OnSkillBarUpdated += UpdateSkillBar;
        EventManager.OnSkillChoose += HighlightSkill;
    }

    private void OnDisable()
    {
        EventManager.OnSkillBarUpdated -= UpdateSkillBar;
        EventManager.OnSkillChoose -= HighlightSkill;
    }


    private void UpdateSkillBar(int whichIcon, Sprite skillSprite)
    {
        skillIcons[whichIcon].GetComponent<Image>().sprite = skillSprite;     
    }

    private void HighlightSkill(int whichSkill)
    {
        for (int i = 0; i < skillIcons.Length; i++)
        {
            var gm = skillIcons[i].transform.GetChild(0).gameObject;
            gm.SetActive(false);
        }
        skillIcons[whichSkill].transform.GetChild(0).gameObject.SetActive(true);
    }


}
