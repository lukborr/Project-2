using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.PlayerSettings;

public class SkillManager : MonoBehaviour
{
    [SerializeField] public GameObject activeProjectile;
    [SerializeField] private int activeSkillLevel;
    [SerializeField] private GameObject secondarySkill;
    [SerializeField] private GameObject handGameobject;
    Quaternion handRotation;

    public Dictionary<string, Skillshot> gatheredSkills = new Dictionary<string, Skillshot>();
    private Skillshot[] skillsNumbers = new Skillshot[4];
    private bool[] cooldowns = new bool[4] { true, true, true, true };
    private Dictionary<string, string> secondarySkillsDictionary = new Dictionary<string, string>()
    {
        {"CometCall", "BurningGround" }
    };
    private int selectedNumber;

    [SerializeField] private OffensiveSkillSO offensiveSkillSO;
    private Vector2 worldPositionCursor;
    private float distancePlayerMouse;
    private bool inRange = false;

    [SerializeField] private GameObject aurasGm;

    [SerializeField] private float sphereRadius;
    private float activeProjectileRange;


    private void Update()
    {
        handRotation = handGameobject.transform.rotation * Quaternion.Euler(0, 0, 45);
        worldPositionCursor = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        distancePlayerMouse = Vector2.Distance(worldPositionCursor, transform.position);
        if (distancePlayerMouse <= activeProjectileRange)
        {
            inRange = true;
        }
        else
        {
            inRange = false;
        }

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

        else if (Input.GetKeyDown(KeyCode.Y))
        {
            LevelUpSkill(gatheredSkills["Ignite"]);
           
        }
        else if (Input.GetKeyDown(KeyCode.Space))
        {
            for (int i = 0; i < skillsNumbers.Length; i++)
            {
                Debug.Log(skillsNumbers[i]);
            }
        }

    }

    private void Awake()
    {
        LoadNewSkillPrefab("Icicle");
        LoadNewSkillPrefab("FrostBolt");
        LoadNewSkillPrefab("Ignite");
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
                Vector2 pos = new Vector2();
                if (activeProjectile.GetComponent<Skillshot>() != null)
                {

                    Skillshot skillshot = activeProjectile.GetComponent<Skillshot>();
                    
                    if (skillshot.whereSkillSpawn == WhereSkillSpawn.Hand)

                    {
                        GameObject go = Instantiate(activeProjectile, handGameobject.transform.position, handRotation);
                        pos= go.transform.position;
                    }
                    else if (skillshot.whereSkillSpawn == WhereSkillSpawn.Cursor)
                    {

                        if (inRange)
                        {
                            GameObject go = Instantiate(activeProjectile, worldPositionCursor, activeProjectile.transform.rotation);
                            pos = go.transform.position;
                        }
                    }
                    else if (skillshot.whereSkillSpawn == WhereSkillSpawn.Self)

                    {
                        GameObject go = Instantiate(activeProjectile, transform.position, activeProjectile.transform.rotation);
                        pos = go.transform.position;
                    }                   

                }
                if (secondarySkill != null)
                {
                    //Instantiate(secondarySkill, pos, secondarySkill.transform.rotation);                   
                }

                cooldowns[selectedNumber] = false;
                StartCoroutine(ResetCooldown(selectedNumber));

            }
        }
    }

    private void LoadNewSkillPrefab(string name)
    {
        if (gatheredSkills.ContainsKey(name))
        {
            return;
        }
        else
        {
            GameObject gm = Resources.Load("Prefabs/Skills/" + name) as GameObject;

            if (gm.GetComponent<Skillshot>().offensiveSkillSO.skillShotType != SkillShotType.Aura)
            {
                Skillshot skillshot = gm.GetComponent<Skillshot>();
                gatheredSkills.Add(name, skillshot);
                activeProjectile = gm;
                activeSkillLevel = skillshot.skillLevel;
                offensiveSkillSO = Resources.Load<OffensiveSkillSO>("SO/SkillsSO/" + name);
                activeProjectileRange = offensiveSkillSO.SkillRange;
                skillshot.skillDamage = offensiveSkillSO.skillDamage;
                skillshot.slowDuration= offensiveSkillSO.slowDuration;
                skillshot.slowPercent= offensiveSkillSO.slowPercent;
                skillshot.skillDuration = offensiveSkillSO.skillDuration;
                skillshot.stunDuration = offensiveSkillSO.stunDuration;
                skillshot.dotDuration = offensiveSkillSO.skillDuration;
                skillshot.skillSpeed = offensiveSkillSO.skillSpeed;
                skillshot.cooldownTime = offensiveSkillSO.skillCooldown;
                skillshot.enemyCountBeforeDestroy = offensiveSkillSO.enemyCountBeforeDestroy;
                skillshot.whereSkillSpawn = offensiveSkillSO.whereSkillSpawn;
                skillshot.skillLevel = 1;
                AssignSkillToNumber(skillshot);

                if (activeProjectile.GetComponent<Skillshot>().offensiveSkillSO.needsSecondarySkill)
                {                  
                    string secondarySkillName = secondarySkillsDictionary[name];
                    secondarySkill = Resources.Load("Prefabs/Skills/" + secondarySkillName) as GameObject;
                    Skillshot skillshot2 = secondarySkill.GetComponent<Skillshot>();
                    activeProjectileRange = offensiveSkillSO.SkillRange;
                    skillshot2.skillDamage = offensiveSkillSO.skillDamage;
                    skillshot2.slowPercent = offensiveSkillSO.slowPercent;
                    skillshot2.slowDuration = offensiveSkillSO.slowDuration;
                    skillshot2.skillDuration = offensiveSkillSO.skillDuration;
                    skillshot2.stunDuration = offensiveSkillSO.stunDuration;
                    skillshot2.dotDuration = offensiveSkillSO.skillDuration;
                    skillshot2.skillSpeed = offensiveSkillSO.skillSpeed;
                    skillshot2.cooldownTime = offensiveSkillSO.skillCooldown;
                    skillshot2.enemyCountBeforeDestroy = offensiveSkillSO.enemyCountBeforeDestroy;
                    skillshot2.whereSkillSpawn = offensiveSkillSO.whereSkillSpawn;
                }
            }

            else if (gm.GetComponent<Skillshot>().offensiveSkillSO.skillShotType == SkillShotType.Aura)
            {
                aurasGm.transform.Find(name).gameObject.SetActive(true);
            }

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
            selectedNumber = number;
            if ((activeProjectile.GetComponent<Skillshot>().offensiveSkillSO.needsSecondarySkill))
            {
                string secondarySkillName = secondarySkillsDictionary[activeProjectile.name];
                secondarySkill = Resources.Load("Prefabs/Skills/" + secondarySkillName) as GameObject;
            }
            else
            {
                secondarySkill = null;
            }
        }
    }

    private void LevelUpSkill(Skillshot skillshot)
    {
        int skillLevel = skillshot.skillLevel;

        switch (skillshot.name)
        {
            case "Icicle":
                switch (skillLevel)
                {
                    case 1:
                        skillshot.skillDamage += 10;
                        break;
                    case 2:
                        skillshot.cooldownTime -= 0.5f;
                        break;
                    case 3:
                        skillshot.enemyCountBeforeDestroy = 3;
                        break;
                    case 4:
                        skillshot.skillDamage += 10;
                        break;
                    case 5:
                        skillshot.cooldownTime -= 0.5f;
                        break;
                    case 6:
                        skillshot.skillDamage += 10;
                        break;
                }
                break;

            case "Ignite":
                switch (skillLevel)
                {
                    case 1:
                        skillshot.skillDamage += 2;
                        break;
                    case 2:
                        skillshot.cooldownTime -= 0.5f;
                        break;
                    case 3:
                        skillshot.transform.localScale = new Vector3(2, 2, 1);
                        break;
                    case 4:
                        skillshot.skillDamage += 2;
                        break;
                    case 5:
                        skillshot.cooldownTime -= 0.5f;
                        break;
                    case 6:
                        skillshot.transform.localScale = new Vector3(2, 2, 1);
                        break;
                }
                break;
        }
      
        skillshot.skillLevel++;
        Debug.Log($"{skillshot} lvl is now: {skillshot.skillLevel}");
    }

    IEnumerator ResetCooldown(int number)
    {
        yield return new WaitForSeconds(skillsNumbers[number].cooldownTime);
        cooldowns[number] = true;
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, sphereRadius);
    }

}
