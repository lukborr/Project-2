using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SkillManager : MonoBehaviour
{
    [SerializeField] public GameObject activeProjectile;
    [SerializeField] private GameObject secondarySkill;
    [SerializeField] private GameObject handGameobject;
    Quaternion handRotation;

    private List<string> availableSkillsToUpgrade = new List<string>()
    { "Icicle","Frostbolt","LightningChain","ElectricBall","PoisonPool",
      "Blizzard","CometCall","Fireball","Ignite","Thunderbolt", "ForceExplosion"};
    public Dictionary<string, Skillshot> gatheredSkills = new Dictionary<string, Skillshot>();
    private Skillshot[] skillsNumbers = new Skillshot[5];
    private bool[] cooldowns = new bool[5] { true, true, true, true, true };
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
        else if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            LoadExistingSkill(4);
        }
        else if (Input.GetKeyDown(KeyCode.T))
        {
            Debug.Log("Testuje T");
        }

    }

    private void Start()
    {      
        LoadNewSkillPrefab("Fireball");
        LoadNewSkillPrefab("CometCall");
    }

    private void OnEnable()
    {
        EventManager.MouseButton0 += SpawnProjectile;
        EventManager.LeveledUp += GetRandomSkills;
        EventManager.OnButtonClicked += LevelUpSkill;
    }
    private void OnDisable()
    {
        EventManager.MouseButton0 -= SpawnProjectile;
        EventManager.LeveledUp -= GetRandomSkills;
        EventManager.OnButtonClicked -= LevelUpSkill;
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
                        go.transform.localScale = new Vector3(GlobalStats.projectileSizeMultiplier, GlobalStats.projectileSizeMultiplier);
                        pos = go.transform.position;
                    }
                    else if (skillshot.whereSkillSpawn == WhereSkillSpawn.Cursor)
                    {

                        if (inRange)
                        {
                            GameObject go = Instantiate(activeProjectile, worldPositionCursor, activeProjectile.transform.rotation);
                            go.transform.localScale = new Vector3(GlobalStats.projectileSizeMultiplier, GlobalStats.projectileSizeMultiplier);
                            pos = go.transform.position;
                        }
                    }
                    else if (skillshot.whereSkillSpawn == WhereSkillSpawn.Self)

                    {
                        GameObject go = Instantiate(activeProjectile, transform.position, activeProjectile.transform.rotation);
                        go.transform.localScale = new Vector3(GlobalStats.projectileSizeMultiplier, GlobalStats.projectileSizeMultiplier);
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
            Skillshot skillshot = gm.GetComponent<Skillshot>();
            offensiveSkillSO = Resources.Load<OffensiveSkillSO>("SO/SkillsSO/" + name);
            EventManager.CallOnSkillBarUpdatedEvent(AssignSkillToNumber(skillshot), offensiveSkillSO.skillSprite);
            if (gm.GetComponent<Skillshot>().offensiveSkillSO.skillShotType != SkillShotType.Aura)
            {              
                gatheredSkills.Add(name, skillshot);
                activeProjectile = gm;
                activeProjectileRange = offensiveSkillSO.SkillRange;
                skillshot.skillDamage = offensiveSkillSO.skillDamage;
                skillshot.slowDuration= offensiveSkillSO.slowDuration;
                skillshot.slowPercent= offensiveSkillSO.slowPercent;
                skillshot.skillDuration = offensiveSkillSO.skillDuration;
                skillshot.stunDuration = offensiveSkillSO.stunDuration;
                skillshot.dotDuration = offensiveSkillSO.skillDuration;
                skillshot.skillSpeed = offensiveSkillSO.skillSpeed;
                skillshot.cooldownTime = offensiveSkillSO.skillCooldown * GlobalStats.cooldownMultiplier;
                skillshot.enemyCountBeforeDestroy = offensiveSkillSO.enemyCountBeforeDestroy;
                skillshot.whereSkillSpawn = offensiveSkillSO.whereSkillSpawn;
                skillshot.skillLevel = 1;
                
                

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
                gatheredSkills.Add(name, skillshot);               
            }
        }
    }

    private int AssignSkillToNumber(Skillshot skillshot)
    {
        for (int i = 0; i < skillsNumbers.Length; i++)
        {
            if (skillsNumbers[i] == skillshot)
            {
                return i;
            }
            else if (skillsNumbers[i] == null)
            {
                skillsNumbers[i] = skillshot;
                if (skillsNumbers[4] != null)
                {
                    availableSkillsToUpgrade.Clear();
                    availableSkillsToUpgrade = gatheredSkills.Keys.ToList();
                }               
                return i;             
            }  
        }
        return 0;
    }

    private void LoadExistingSkill(int number)
    {
        if (skillsNumbers[number] != null)
        {
            EventManager.CallOnSkillChooseEvent(number);
            activeProjectile = skillsNumbers[number].gameObject;
            activeProjectile.GetComponent<Skillshot>().cooldownTime *= GlobalStats.cooldownMultiplier;
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
    public void LevelUpSkill(string skillName)
    {
        if (gatheredSkills.ContainsKey(skillName))
        {
            Skillshot skillshot = gatheredSkills[skillName];
            int skillLevel = skillshot.skillLevel;
            Debug.Log($"{skillName} you already have");
            switch (skillshot.name)
            {
                case "Icicle":
                    switch (skillLevel)
                    {
                        case 1:
                            skillshot.skillDamage += 2;
                            break;
                        case 2:
                            skillshot.cooldownTime -= 0.5f;
                            break;
                        case 3:
                            skillshot.enemyCountBeforeDestroy = 3;
                            break;
                        case 4:
                            skillshot.skillDamage += 2;
                            break;
                        case 5:
                            skillshot.cooldownTime -= 0.5f;
                            break;
                        case 6:
                            skillshot.skillDamage += 4;
                            availableSkillsToUpgrade.Remove("Icicle");
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
                            availableSkillsToUpgrade.Remove("Ignite");
                            break;
                    }
                    break;

                case "Thunderbolt":
                    switch (skillLevel)
                    {
                        case 1:
                            skillshot.skillDamage += 2;
                            break;
                        case 2:
                            skillshot.stunDuration += 0.25f;
                            break;
                        case 3:
                            skillshot.skillDamage += 2;
                            break;
                        case 4:
                            skillshot.skillDamage += 2;
                            break;
                        case 5:
                            skillshot.cooldownTime -= 0.5f;
                            break;
                        case 6:
                            skillshot.skillDamage += 2;
                            skillshot.stunDuration += 0.25f;
                            availableSkillsToUpgrade.Remove("Thunderbolt");
                            break;
                    }
                    break;

                case "LightningChain":
                    switch (skillLevel)
                    {
                        case 1:
                            skillshot.skillDamage += 1;
                            break;
                        case 2:
                            skillshot.stunDuration += 0.25f;
                            break;
                        case 3:
                            skillshot.skillDamage += 1;
                            break;
                        case 4:
                            skillshot.skillDamage += 1;
                            break;
                        case 5:
                            skillshot.cooldownTime -= 0.5f;
                            break;
                        case 6:
                            skillshot.skillDamage += 2;
                            skillshot.stunDuration += 0.25f;
                            availableSkillsToUpgrade.Remove("LightningChain");
                            break;
                    }
                    break;

                case "Frostbolt":
                    switch (skillLevel)
                    {
                        case 1:
                            skillshot.skillDamage += 2;
                            break;
                        case 2:
                            skillshot.slowPercent += 0.25f;
                            break;
                        case 3:
                            skillshot.cooldownTime -= 0.5f;
                            break;
                        case 4:
                            skillshot.skillDamage += 2;
                            break;
                        case 5:
                            skillshot.cooldownTime -= 0.5f;
                            break;
                        case 6:
                            skillshot.skillDamage += 2;
                            skillshot.slowPercent += 0.25f;
                            availableSkillsToUpgrade.Remove("Frostbolt");
                            break;
                    }
                    break;

                case "Fireball":
                    switch (skillLevel)
                    {
                        case 1:
                            skillshot.skillDamage += 2;
                            break;
                        case 2:
                            skillshot.cooldownTime -= 0.5f;
                            break;
                        case 3:
                            skillshot.skillDamage += 2;
                            break;
                        case 4:
                            skillshot.skillDamage += 2;
                            break;
                        case 5:
                            skillshot.cooldownTime -= 0.5f;
                            break;
                        case 6:
                            skillshot.skillDamage += 2;
                            skillshot.cooldownTime -= 0.25f;
                            availableSkillsToUpgrade.Remove("Fireball");
                            break;
                    }
                    break;

                case "Blizzard":
                    switch (skillLevel)
                    {
                        case 1:
                            skillshot.skillDamage += 1;
                            break;
                        case 2:
                            skillshot.slowPercent += 0.15f;
                            break;
                        case 3:
                            skillshot.skillDamage += 1;
                            break;
                        case 4:
                            skillshot.transform.localScale = new Vector3(2, 2, 1);
                            break;
                        case 5:
                            skillshot.cooldownTime -= 0.5f;
                            break;
                        case 6:
                            skillshot.skillDamage += 2;
                            skillshot.slowPercent += 0.15f;
                            availableSkillsToUpgrade.Remove("Blizzard");
                            break;
                    }
                    break;

                case "CometCall":
                    switch (skillLevel)
                    {
                        case 1:
                            skillshot.skillDamage += 1;
                            break;
                        case 2:
                            skillshot.cooldownTime -= 0.25f;
                            break;
                        case 3:
                            skillshot.skillDamage += 1;
                            break;
                        case 4:
                            // powiekszyc burning grounda
                            break;
                        case 5:
                            skillshot.cooldownTime -= 0.25f;
                            break;
                        case 6:
                            skillshot.skillDamage += 2;
                            // powiekszyc obszar burning grounda
                            availableSkillsToUpgrade.Remove("CometCall");
                            break;
                    }
                    break;

                case "ElectricBall":
                    switch (skillLevel)
                    {
                        case 1:
                            skillshot.skillDamage += 1;
                            break;
                        case 2:
                            skillshot.stunDuration += 0.25f;
                            break;
                        case 3:
                            skillshot.skillSpeed += 1.25f;
                            break;
                        case 4:
                            skillshot.skillDamage += 1;
                            break;
                        case 5:
                            skillshot.cooldownTime -= 0.5f;
                            break;
                        case 6:
                            skillshot.skillDamage += 2;
                            skillshot.stunDuration += 0.25f;
                            availableSkillsToUpgrade.Remove("ElectricBall");
                            break;
                    }
                    break;

                case "ForceExplosion":
                    switch (skillLevel)
                    {
                        case 1:
                            skillshot.skillDamage += 1;
                            break;
                        case 2:
                            skillshot.cooldownTime -= 0.25f;
                            break;
                        case 3:
                            skillshot.skillDamage += 1;
                            break;
                        case 4:
                            skillshot.transform.localScale = new Vector3(1.5f, 1.5f, 1);
                            break;
                        case 5:
                            skillshot.cooldownTime -= 0.25f;
                            break;
                        case 6:
                            skillshot.skillDamage += 2;
                            skillshot.transform.localScale = new Vector3(2, 2, 1);
                            availableSkillsToUpgrade.Remove("ForceExplosion");
                            break;
                    }
                    break;

                case "PoisonPool":
                    switch (skillLevel)
                    {
                        case 1:
                            skillshot.skillDamage += 1;
                            break;
                        case 2:
                            skillshot.cooldownTime -= 0.25f;
                            break;
                        case 3:
                            skillshot.skillDamage += 1;
                            break;
                        case 4:
                            skillshot.transform.localScale = new Vector3(1.5f, 1.5f, 1);
                            break;
                        case 5:
                            skillshot.cooldownTime -= 0.25f;
                            break;
                        case 6:
                            skillshot.skillDamage += 2;
                            skillshot.transform.localScale = new Vector3(2, 2, 1);
                            availableSkillsToUpgrade.Remove("PoisonPool");
                            break;
                    }
                    break;
            }
            skillshot.skillLevel++;
            Debug.Log($"{skillshot} lvl is now: {skillshot.skillLevel}");
        }
        else
        {
            Debug.Log($"You dont have the {skillName} already so i will load it");
            LoadNewSkillPrefab(skillName);
        }
        
    }

    private void GetRandomSkills()
    {
        int skill0 = Random.Range(0, availableSkillsToUpgrade.Count - 1);
        string _skill0 = availableSkillsToUpgrade[skill0];
        availableSkillsToUpgrade.Remove(_skill0);
        int skill1 = Random.Range(0, availableSkillsToUpgrade.Count - 1);
        string _skill1 = availableSkillsToUpgrade[skill1];
        availableSkillsToUpgrade.Remove(_skill1);
        int skill2 = Random.Range(0, availableSkillsToUpgrade.Count - 1);
        string _skill2 = availableSkillsToUpgrade[skill2];
        availableSkillsToUpgrade.Remove(_skill2);
        availableSkillsToUpgrade.Add(_skill0);
        availableSkillsToUpgrade.Add(_skill1);
        availableSkillsToUpgrade.Add(_skill2);
        var randomSkills = new List<string>() { _skill0, _skill1, _skill2 };
        EventManager.CallGeneratedRandomSkillsEvent(randomSkills);
        
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
