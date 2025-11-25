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
    [SerializeField] private GameObject rangePreview;
    [SerializeField] private GameObject projectiles;

    [SerializeField] GameObject[] cooldownImages;

    private List<string> availableActiveSkillsToUpgrade = new List<string>()
    { "Icicle","Frostbolt","LightningChain","ElectricBall","PoisonPool",
      "Blizzard","CometCall","Fireball","Ignite","Thunderbolt"};
    public Dictionary<string, Skillshot> gatheredActiveSkills = new Dictionary<string, Skillshot>();

    private List<string> availablePassiveSkillsToUpgrade = new List<string>()
    { "LuckyKnife", "EnlargingWand", "GoldenApple", "MagicShoes", "SpectralArmor", "SpellBook"};
    private List<string> gatheredPassiveSkills = new List<string>();
    List<string> availableSkillsToUpgrade = new List<string>();

    private Skillshot[] activeSkillsNumbers = new Skillshot[5];
    private bool[] cooldowns = new bool[5] { true, true, true, true, true };
    private Dictionary<string, string> secondarySkillsDictionary = new Dictionary<string, string>()
    {
        {"CometCall", "BurningGround" }
    };
    private int selectedNumber;

    [SerializeField] private OffensiveSkillSO offensiveSkillSO;
    [SerializeField] private PassiveSkillSO passiveSkillSO;
    private Vector2 worldPositionCursor;
    private float distancePlayerMouse;
    private bool inRange = false;

    [SerializeField] private GameObject aurasGm;

    [SerializeField] private float sphereRadius;
    private float activeProjectileRange;

    private bool[] isRunning = new bool[4];
    private float[] elapsedTime = new float[4];
    

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
            StartTimer(1);

        }
        else if (Input.GetKeyDown(KeyCode.L))
        {
            PauseCooldowns(FindActiveCooldowns());
        }

            for (int i = 0; i < 4; i++)
            {
                if (isRunning[i])
                {
                    elapsedTime[i] += Time.deltaTime;
                }
            }

    }

    private void Awake()
    {

    }
    private void Start()
    {
        CombineAvailableSkills();
        LoadNewSkillPrefab("Fireball");
        LoadNewSkillPrefab("PoisonPool");
        LoadExistingSkill(0);
    }
    private void CombineAvailableSkills()
    {
        var skills = availableActiveSkillsToUpgrade.Union(availablePassiveSkillsToUpgrade);
        availableSkillsToUpgrade = skills.ToList();
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
                    GameObject go;

                    if (skillshot.whereSkillSpawn == WhereSkillSpawn.Hand)

                    {
                        go = Instantiate(activeProjectile, handGameobject.transform.position, handRotation, projectiles.transform);                       
                        go.transform.localScale = new Vector3(GlobalStats.projectileSizeMultiplier, GlobalStats.projectileSizeMultiplier);
                        pos = go.transform.position;
                        cooldowns[selectedNumber] = false;
                        StartCoroutine(ResetCooldown(selectedNumber));
                    }
                    else if (skillshot.whereSkillSpawn == WhereSkillSpawn.Cursor)
                    {
                        if (inRange)
                        {
                            go = Instantiate(activeProjectile, worldPositionCursor, activeProjectile.transform.rotation, projectiles.transform);
                            go.transform.localScale = new Vector3(GlobalStats.projectileSizeMultiplier, GlobalStats.projectileSizeMultiplier);
                            pos = go.transform.position;
                            cooldowns[selectedNumber] = false;
                            StartCoroutine(ResetCooldown(selectedNumber));

                        }
                        else
                        {
                            return;
                        }
                    }
                    else if (skillshot.whereSkillSpawn == WhereSkillSpawn.Self)

                    {
                        go = Instantiate(activeProjectile, transform.position, activeProjectile.transform.rotation, projectiles.transform);
                        go.transform.localScale = new Vector3(GlobalStats.projectileSizeMultiplier, GlobalStats.projectileSizeMultiplier);
                        pos = go.transform.position;
                        cooldowns[selectedNumber] = false;
                        StartCoroutine(ResetCooldown(selectedNumber));
                    }
                        
                }
                if (secondarySkill != null)
                {
                    //Instantiate(secondarySkill, pos, secondarySkill.transform.rotation);                   
                }


            }
        }
    }

    private void LoadNewSkillPrefab(string name)
    {
        if (gatheredActiveSkills.ContainsKey(name) || gatheredPassiveSkills.Contains(name))
        {
            Debug.Log("returned");
        }
        else
        {
            if (Resources.Load("Prefabs/Skills/" + name) as GameObject != null)       // if it's active skill
            {
               
                GameObject gm = Resources.Load("Prefabs/Skills/" + name) as GameObject;
                Skillshot skillshot = gm.GetComponent<Skillshot>();
                offensiveSkillSO = Resources.Load<OffensiveSkillSO>("SO/SkillsSO/Offensive/" + name);
                if (gatheredActiveSkills.Count < 1)
                {
                    activeProjectile = gm;
                }
                EventManager.CallOnSkillBarUpdatedEvent(AssignSkillToNumber(skillshot), offensiveSkillSO.skillSprite);
                if (gm.GetComponent<Skillshot>().offensiveSkillSO.skillShotType != SkillShotType.Aura)
                {



                    gatheredActiveSkills.Add(name, skillshot);
                    activeProjectileRange = offensiveSkillSO.SkillRange;
                    skillshot.skillRange = offensiveSkillSO.SkillRange;
                    skillshot.skillDamage = offensiveSkillSO.skillDamage;
                    skillshot.slowDuration = offensiveSkillSO.slowDuration;
                    skillshot.slowPercent = offensiveSkillSO.slowPercent;
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
                    gatheredActiveSkills.Add(name, skillshot);
                }
            }
            else if (availablePassiveSkillsToUpgrade.Contains(name) || gatheredPassiveSkills.Count < 6)
            {
                gatheredPassiveSkills.Add(name);
                passiveSkillSO = Resources.Load<PassiveSkillSO>("SO/SkillsSO/Passive/" + name);
                EventManager.CallOnSkillBarUpdatedEvent(gatheredPassiveSkills.IndexOf(name) + 5, passiveSkillSO.skillSprite);
                GlobalStats.cooldownMultiplier *= passiveSkillSO.cooldownReduction;
                GlobalStats.damageMultiplier *= passiveSkillSO.attackDamageIncrease;
                GlobalStats.movementSpeedMultiplier *= passiveSkillSO.movementSpeedIncrease;
                GlobalStats.projectileSpeedMultiplier *= passiveSkillSO.movementSpeedIncrease;
                GlobalStats.projectileSizeMultiplier *= passiveSkillSO.projectileSizeIncrease;
                GlobalStats.armor *= passiveSkillSO.armorIncrease;
                ApplyCooldown();
            }

        }

        if (gatheredActiveSkills.Count == 5 || gatheredPassiveSkills.Count == 5)
        {
            CombineAvailableSkills();
        }

    }

    private int AssignSkillToNumber(Skillshot skillshot)
    {
        for (int i = 0; i < activeSkillsNumbers.Length; i++)
        {
            if (activeSkillsNumbers[i] == skillshot)
            {
                return i;
            }
            else if (activeSkillsNumbers[i] == null)
            {
                activeSkillsNumbers[i] = skillshot;
                if (activeSkillsNumbers[4] != null)
                {
                    availableActiveSkillsToUpgrade.Clear();
                    availableActiveSkillsToUpgrade = gatheredActiveSkills.Keys.ToList();
                }
                return i;
            }
        }
        return 0;
    }


    private void LoadExistingSkill(int number)
    {
        if (activeSkillsNumbers[number].offensiveSkillSO.skillShotType == SkillShotType.Aura)

        {
            return;
        }
        else if (activeSkillsNumbers[number] != null)
        {
            EventManager.CallOnSkillChooseEvent(number);
            activeProjectile = activeSkillsNumbers[number].gameObject;
            var skillshot = activeProjectile.GetComponent<Skillshot>();
            skillshot.cooldownTime *= GlobalStats.cooldownMultiplier;
            activeProjectileRange = skillshot.skillRange;


            if (skillshot.skillRange > 0)
            {
                rangePreview.gameObject.SetActive(true);
                rangePreview.transform.localScale = new Vector2(skillshot.skillRange * 2, skillshot.skillRange * 2);
            }
            else
            {
                rangePreview.gameObject.SetActive(false);
            }

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
        else {
            Debug.Log("nie ma");
        }

    }
    public void LevelUpSkill(string skillName)
    {
        var so = Resources.Load("SO/SkillsSO/Offensive/" + skillName);
        if (so != null)
        {
            if (gatheredActiveSkills.ContainsKey(skillName))
            {
                Skillshot skillshot = gatheredActiveSkills[skillName];
                int skillLevel = skillshot.skillLevel;
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
                                availableActiveSkillsToUpgrade.Remove("Icicle");
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
                                availableActiveSkillsToUpgrade.Remove("Ignite");
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
                                availableActiveSkillsToUpgrade.Remove("Thunderbolt");
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
                                availableActiveSkillsToUpgrade.Remove("LightningChain");
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
                                availableActiveSkillsToUpgrade.Remove("Frostbolt");
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
                                availableActiveSkillsToUpgrade.Remove("Fireball");
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
                                availableActiveSkillsToUpgrade.Remove("Blizzard");
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
                                availableActiveSkillsToUpgrade.Remove("CometCall");
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
                                availableActiveSkillsToUpgrade.Remove("ElectricBall");
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
                                availableActiveSkillsToUpgrade.Remove("ForceExplosion");
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
                                availableActiveSkillsToUpgrade.Remove("PoisonPool");
                                break;
                        }
                        break;
                }
                skillshot.skillLevel++;
                Debug.Log($"{skillshot} lvl is now: {skillshot.skillLevel}");
            }
            else
            {
                LoadNewSkillPrefab(skillName);
            }

        }
        else
        {
            var so2 = Resources.Load("SO/SkillsSO/Passive/" + skillName);
            if (so2 != null)
            {
                if (gatheredPassiveSkills.Contains(skillName))
                {

                    switch (skillName)
                    {
                        case "LuckyKnife":
                            if (GlobalStats.luckyKnifeLevel < 5)
                            {
                                GlobalStats.luckyKnifeLevel++;
                                GlobalStats.damageMultiplier *= 1.1f;
                            }
                            break;
                        case "EnlargingWand":
                            if (GlobalStats.enlargingWandLevel < 5)
                            {
                                GlobalStats.enlargingWandLevel++;
                                GlobalStats.projectileSizeMultiplier *= 1.15f;
                            }
                            break;
                        case "GoldenApple":
                            if (GlobalStats.goldenAppleLevel < 5)
                            {
                                GlobalStats.goldenAppleLevel++;
                                GlobalStats.health *= 1.1f;
                                EventManager.CallPlayerMaxHealthChangedEvent(1.1f);
                            }
                            break;
                        case "MagicShoes":
                            if (GlobalStats.magicShoesLevel < 5)
                            {
                                GlobalStats.magicShoesLevel++;
                                GlobalStats.movementSpeedMultiplier *= 1.1f;
                            }
                            break;
                        case "SpectralArmor":
                            if (GlobalStats.spectralArmorLevel < 5)
                            {
                                GlobalStats.spectralArmorLevel++;
                                GlobalStats.armor += 5;
                            }
                            break;
                        case "SpellBook":
                            if (GlobalStats.spellBookLevel < 5)
                            {

                                GlobalStats.spellBookLevel++;
                                GlobalStats.cooldownMultiplier *= 0.9f;
                                ApplyCooldown();
                            }
                            break;
                    }
                }
                else
                {
                    LoadNewSkillPrefab(skillName);
                }
            }
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
        EventManager.CallOnCooldownEvent(activeSkillsNumbers[number].cooldownTime * GlobalStats.cooldownMultiplier, cooldownImages[number]);
        float time = activeSkillsNumbers[number].cooldownTime * GlobalStats.cooldownMultiplier;
        StartTimer(number);
        yield return new WaitForSeconds(time);
        StopTimer(number);
        cooldowns[number] = true;
    }

    private void ApplyCooldown()
    {
        var skillshot = activeProjectile.GetComponent<Skillshot>();
        skillshot.cooldownTime *= GlobalStats.cooldownMultiplier;
    }

    private void OnDrawGizmos()
    {
        //Gizmos.DrawWireSphere(transform.position, sphereRadius);
        Gizmos.DrawWireSphere(transform.position, 10);
    }

    private void StartTimer(int number)
    {
        isRunning[number] = true;
        Debug.Log($"zaczeto timer{isRunning[number]}");
    }

    private float StopTimer(int number)
    {
        isRunning[number] = false;
        float time = elapsedTime[number];
        ResetTimer(number);
        Debug.Log($"zatrzymano timer {time}");
        return time;
    }

    private void ResetTimer(int number)
    {
        elapsedTime[number] = 0f;
        Debug.Log("zresetowano timer");
    }

    private List<bool> FindActiveCooldowns()
    {
        List<bool> activeCoold = new List<bool>();
        for (int i = 0; i < isRunning.Length; i++)
        {
            if (isRunning[i])
            {
                activeCoold.Add(isRunning[i]);
            }
        }
        
        return activeCoold;
    }

    private void PauseCooldowns(List<bool> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            Debug.Log("czas: "+StopTimer(i));
        }
        StopAllCoroutines();
    }

}
