using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SkillManager : MonoBehaviour
{
    [SerializeField] private GameObject activeProjectile;
    [SerializeField] private GameObject handGameobject;
    Quaternion handRotation;

    private List<string> gatheredSkills = new List<string>();
    private Skillshot[] skillsNumbers = new Skillshot[4];
    private bool[] cooldowns = new bool[4] { true, true, true, true };
    private int selectedNumber;

    [SerializeField] private OffensiveSkillSO offensiveSkillSO;
    private Vector2 worldPositionCursor;

    private void Update()
    {
        handRotation = handGameobject.transform.rotation * Quaternion.Euler(0, 0, 45);
        worldPositionCursor = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            LoadExistingSkill(0);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            LoadExistingSkill(1);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            LoadExistingSkill(2);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            LoadExistingSkill(3);
        }

        else if (Input.GetKeyDown(KeyCode.Z))
        {
            LoadNewSkillPrefab("Fireball");
            LoadNewSkillPrefab("PoisonPool");
        }
        else if (Input.GetKeyDown(KeyCode.Y))
        {
            ModifySkillStats(skillsNumbers[0], 2, 0, 0, 2, 0);
        }
        else if (Input.GetKeyDown(KeyCode.Space))
        {
            for (int i = 0; i < skillsNumbers.Length; i++)
            {
                Debug.Log(skillsNumbers[i]);
            }
        }
        else if (Input.GetKeyDown(KeyCode.T))
        {
            Debug.Log(skillsNumbers[0].name);
        }
    }

    private void OnEnable()
    {
        EventManager.MouseButton0 += SpawnProjectile;
    }
    private void OnDisable()
    {
        EventManager.MouseButton0 -= SpawnProjectile;
    }
    private void SpawnProjectile()
    {

        if (cooldowns[selectedNumber] == true)
        {        
            
            if (activeProjectile != null)
            {
                Skillshot skillshot = activeProjectile.GetComponent<Skillshot>();


                if (skillshot.whereSkillSpawn == WhereSkillSpawn.Hand)
                {
                    Instantiate(activeProjectile, handGameobject.transform.position, handRotation);

                }
                else if (skillshot.whereSkillSpawn == WhereSkillSpawn.Cursor)
                {
                    Instantiate(activeProjectile, worldPositionCursor, handRotation);

                }
                cooldowns[selectedNumber] = false;
                StartCoroutine(ResetCooldown(selectedNumber));
            }
        }
         
    }


    private void LoadNewSkillPrefab(string name)
    {
        if (gatheredSkills.Contains(name))
        {
            return;
        }
        else

            gatheredSkills.Add(name);
        activeProjectile = Resources.Load("Prefabs/Skills/" + name) as GameObject;
        offensiveSkillSO = Resources.Load<OffensiveSkillSO>("SkillsSO/" + name);
        Skillshot skillshot = activeProjectile.GetComponent<Skillshot>();
        skillshot.skillDamage = offensiveSkillSO.skillDamage;
        skillshot.skillDuration = offensiveSkillSO.skillDuration;
        skillshot.skillSpeed = offensiveSkillSO.skillSpeed;
        skillshot.cooldownTime = offensiveSkillSO.skillCooldown;
        skillshot.enemyCountBeforeDestroy = offensiveSkillSO.enemyCountBeforeDestroy;
        skillshot.whereSkillSpawn = offensiveSkillSO.whereSkillSpawn;

        AssignSkillToNumber(skillshot);

    }

    private void AssignSkillToNumber(Skillshot skillshot)
    {
        for (int i = 0; i < skillsNumbers.Length; i++)
        {
            if (skillsNumbers[i] == skillshot)
            {
                return;
            }
            else if (skillsNumbers[i] == null)
            {
                skillsNumbers[i] = skillshot;
                break;
            }
        }
    }

    private void LoadExistingSkill(int number)
    {
        if (skillsNumbers[number] != null)
        {
            activeProjectile = skillsNumbers[number].gameObject;
            selectedNumber = number;
            Debug.Log(skillsNumbers[number].skillDamage);
        }
    }

    private void ModifySkillStats(Skillshot skillshot, int damage, float duration, float speed, float cooldown, int enemies)
    {
        skillshot.skillDamage += damage;
        skillshot.skillDuration += duration;
        skillshot.skillSpeed += speed;
        skillshot.cooldownTime -= cooldown;
        skillshot.enemyCountBeforeDestroy += enemies;
    }

    IEnumerator ResetCooldown(int number)
    {
        yield return new WaitForSeconds(skillsNumbers[number].cooldownTime);
        cooldowns[number] = true;
    }



}