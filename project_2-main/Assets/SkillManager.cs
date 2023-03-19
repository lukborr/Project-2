using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillManager : MonoBehaviour
{
    [SerializeField] private GameObject activeProjectile;
    [SerializeField] private GameObject handGameobject;
    Quaternion handRotation;

    private bool cooldownUp = true;

    private List<string> gatheredSkills = new List<string>();
    private Skillshot[] skillsNumbers = new Skillshot[4];

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
            Skillshot skillshot = activeProjectile.GetComponent<Skillshot>();

            if (skillshot.whereSkillSpawn == WhereSkillSpawn.Hand)
            {
                Instantiate(activeProjectile, handGameobject.transform.position, handRotation);
                StartCoroutine(ResetCooldown(skillshot.cooldownTime));
            }
            else if (skillshot.whereSkillSpawn == WhereSkillSpawn.Cursor)
            {
                Instantiate(activeProjectile, worldPositionCursor, handRotation);
                StartCoroutine(ResetCooldown(skillshot.cooldownTime));
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

        gatheredSkills.Add(name);
        {
            switch (name)

            {
                case "Fireball":
                    activeProjectile = Resources.Load("Prefabs/Skills/Fireball") as GameObject;
                    offensiveSkillSO = Resources.Load<OffensiveSkillSO>("SkillsSO/Fireball");
                    break;

                case "PoisonPool":
                    activeProjectile = Resources.Load("Prefabs/Skills/PoisonPool") as GameObject;
                    offensiveSkillSO = Resources.Load<OffensiveSkillSO>("SkillsSO/PoisonPool");
                    break;
            }

            Skillshot skillshot = activeProjectile.GetComponent<Skillshot>();
            skillshot.skillDamage = offensiveSkillSO.skillDamage;
            skillshot.skillDuration = offensiveSkillSO.skillDuration;
            skillshot.skillSpeed = offensiveSkillSO.skillSpeed;
            skillshot.cooldownTime = offensiveSkillSO.skillCooldown;
            skillshot.enemyCountBeforeDestroy = offensiveSkillSO.enemyCountBeforeDestroy;
            skillshot.whereSkillSpawn = offensiveSkillSO.whereSkillSpawn;

            AssignSkillToNumber(skillshot);
        }
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
            Debug.Log(skillsNumbers[number].skillDamage);
        }
    }

    private void ModifySkillStats(Skillshot skillshot, int damage, float duration, float speed, float cooldown, int enemies)
    {
        skillshot.skillDamage += damage;
        skillshot.skillDuration+= duration;
        skillshot.skillSpeed+= speed;
        skillshot.cooldownTime-= cooldown; 
        skillshot.enemyCountBeforeDestroy += enemies;
    }



}
