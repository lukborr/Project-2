using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UseActiveSkill : MonoBehaviour
{
    [SerializeField] private GameObject activeProjectile;
    [SerializeField] private GameObject handGameobject;
    Quaternion handRotation;

    private bool cooldownUp = true;
    private float cooldownTime;

    private List<string> gatheredSkills = new List<string>();
    private string[] skills = new string[4];

    [SerializeField] private OffensiveSkillSO offensiveSkillSO;
    private Vector2 worldPositionCursor;

    private void Update()
    {
        handRotation = handGameobject.transform.rotation * Quaternion.Euler(0, 0, 45);
        worldPositionCursor = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            if (skills[0] != null)
                LoadExistingSkill(0);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            if (skills[1] != null)
                LoadExistingSkill(1);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            if (skills[2] != null)
                LoadExistingSkill(2);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            if (skills[3] != null)
                LoadExistingSkill(3);
        }
        else if (Input.GetKeyDown(KeyCode.X))

        {
            for (int i = 0; i < gatheredSkills.Count; i++)
            {
                Debug.Log(gatheredSkills[i]);
            }
        }

        else if(Input.GetKeyDown(KeyCode.Z)) 
        {
            AssignSkillToNumber("Fireball");
            AssignSkillToNumber("faja");
        }
        else if (Input.GetKeyDown(KeyCode.Space)) 
        {
            Debug.Log(skills[0]);
            Debug.Log(skills[1]);
            Debug.Log(skills[2]);
            Debug.Log(skills[3]);
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
        if (activeProjectile != null && cooldownUp)
        {
            cooldownUp = false;

            if (offensiveSkillSO.whereSkillSpawn == WhereSkillSpawn.Hand)
            {

                Instantiate(activeProjectile, handGameobject.transform.position, handRotation);
                StartCoroutine(ResetCooldown(cooldownTime));
            }
            else if (offensiveSkillSO.whereSkillSpawn == WhereSkillSpawn.Cursor)
            {
                Instantiate(activeProjectile, worldPositionCursor, handRotation);
                StartCoroutine(ResetCooldown(cooldownTime));
            }

        }
    }
    IEnumerator ResetCooldown(float time)
    {
        yield return new WaitForSeconds(time);
        cooldownUp = true;
    }

    private void LoadNewSkillPrefab(string name)
    {
        if (gatheredSkills.Contains(name))
        {
            return;
        }
        else
        {

            gatheredSkills.Add(name);
            

            switch (name)

            {
                case "Fireball":
                    activeProjectile = Resources.Load("Prefabs/Skills/Fireball") as GameObject;
                    offensiveSkillSO = Resources.Load<OffensiveSkillSO>("SkillsSO/Fireball");
                    StatsManager.skill1Cooldown = offensiveSkillSO.skillCooldown;
                    StatsManager.skill1Duration = offensiveSkillSO.skillDuration;
                    StatsManager.skill1Speed = offensiveSkillSO.skillSpeed;
                    StatsManager.skill1Damage = offensiveSkillSO.skillDamage;
                    StatsManager.skill1Enemies = offensiveSkillSO.enemyCountBeforeDestroy;
                    break;

                case "PoisonPool":
                    activeProjectile = Resources.Load("Prefabs/Skills/PoisonPool") as GameObject;
                    offensiveSkillSO = Resources.Load<OffensiveSkillSO>("SkillsSO/PoisonPool");
                    StatsManager.skill2Cooldown = offensiveSkillSO.skillCooldown;
                    StatsManager.skill2Duration = offensiveSkillSO.skillDuration;
                    StatsManager.skill2Speed = offensiveSkillSO.skillSpeed;
                    StatsManager.skill2Damage = offensiveSkillSO.skillDamage;
                    StatsManager.skill2Enemies = offensiveSkillSO.enemyCountBeforeDestroy;
                    break;
            }
        }


    }

    private void AssignSkillToNumber(string skill)
    {
        for (int i = 0; i < skills.Length; i++)
        {
            if (skills[i] == skill)
            {
                return;
            }
             else if (skills[i] == null)
            {
                skills[i] = skill;
                break;
            }
        }
    }

    private void LoadExistingSkill(int number)
    {
        activeProjectile = Resources.Load("Prefabs/Skills/" + skills[number]) as GameObject;
        offensiveSkillSO = Resources.Load<OffensiveSkillSO>("SkillsSO/" + skills[number]);
      
    }



}
