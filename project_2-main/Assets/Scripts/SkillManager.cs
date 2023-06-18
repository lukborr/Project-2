using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillManager : MonoBehaviour
{
    [SerializeField] private GameObject activeProjectile;
    [SerializeField] private GameObject handGameobject;
    Quaternion handRotation;

    public List<string> gatheredSkills = new List<string>();
    private Skillshot[] skillsNumbers = new Skillshot[4];
    private bool[] cooldowns = new bool[4] { true, true, true, true };
    private int selectedNumber;

    [SerializeField] private OffensiveSkillSO offensiveSkillSO;
    private Vector2 worldPositionCursor;
    private float distancePlayerMouse;
    private bool inRange = false;

    [SerializeField] private GameObject testProjectile;
    [SerializeField] private GameObject aurasGm;

    [SerializeField] private float  sphereRadius;
    private float activeProjectileRange;


    private void Update()
    {
        handRotation = handGameobject.transform.rotation * Quaternion.Euler(0, 0, 45);
        worldPositionCursor = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        distancePlayerMouse = Vector2.Distance(worldPositionCursor, transform.position);
        if(distancePlayerMouse <= activeProjectileRange)
        {
            inRange= true;
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

    private void Awake()
    {
        LoadNewSkillPrefab("LightningChain");
        LoadNewSkillPrefab("Thunderbolt");
        LoadNewSkillPrefab("PoisonPool");
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
                if(activeProjectile.GetComponent<Skillshot>() != null)
                {
                    Skillshot skillshot = activeProjectile.GetComponent<Skillshot>();


                    if (skillshot.whereSkillSpawn == WhereSkillSpawn.Hand)
                    {
                        Instantiate(activeProjectile, handGameobject.transform.position, handRotation);

                    }
                    else if (skillshot.whereSkillSpawn == WhereSkillSpawn.Cursor)
                    {                      
                                             
                        if(inRange)
                        {                           
                            Instantiate(activeProjectile, worldPositionCursor, activeProjectile.transform.rotation);
                        }
                    }
                    else if(skillshot.whereSkillSpawn == WhereSkillSpawn.Self)
                    {
                        Instantiate(activeProjectile, transform.position, activeProjectile.transform.rotation);
                    }
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
        {
            GameObject gm = Resources.Load("Prefabs/Skills/" + name) as GameObject;

           
            if (gm.GetComponent<Skillshot>().offensiveSkillSO.skillShotType != SkillShotType.Aura)
            {
                gatheredSkills.Add(name);
                activeProjectile = gm;
                offensiveSkillSO = Resources.Load<OffensiveSkillSO>("SO/SkillsSO/" + name);
                Skillshot skillshot = activeProjectile.GetComponent<Skillshot>();
                activeProjectileRange = offensiveSkillSO.SkillRange;
                skillshot.skillDamage = offensiveSkillSO.skillDamage;
                skillshot.skillDuration = offensiveSkillSO.skillDuration;
                skillshot.stunDuration = offensiveSkillSO.stunDuration;
                skillshot.dotDuration = offensiveSkillSO.skillDuration;
                skillshot.skillSpeed = offensiveSkillSO.skillSpeed;
                skillshot.cooldownTime = offensiveSkillSO.skillCooldown;
                skillshot.enemyCountBeforeDestroy = offensiveSkillSO.enemyCountBeforeDestroy;
                skillshot.whereSkillSpawn = offensiveSkillSO.whereSkillSpawn;
                AssignSkillToNumber(skillshot);
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

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, sphereRadius);
        
    }

}
