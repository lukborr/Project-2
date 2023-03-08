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

    [SerializeField] private OffensiveSkillSO offensiveSkillSO;
    private Vector2 worldPositionCursor;

    private void Update()
    {
        handRotation = handGameobject.transform.rotation * Quaternion.Euler(0, 0, 45);
        worldPositionCursor = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            LoadNewSkillPrefab("Fireball");
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            LoadNewSkillPrefab("PoisonPool");
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
            else if(offensiveSkillSO.whereSkillSpawn == WhereSkillSpawn.Cursor)
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
        switch (name)
        {
            case "Fireball":
               activeProjectile = Resources.Load("Prefabs/Skills/Fireball") as GameObject;
                offensiveSkillSO = Resources.Load<OffensiveSkillSO>("SkillsSO/Fireball");
                cooldownTime = offensiveSkillSO.skillCooldown;
                break;

            case "PoisonPool":
                activeProjectile = Resources.Load("Prefabs/Skills/PoisonPool") as GameObject;
                offensiveSkillSO = Resources.Load<OffensiveSkillSO>("SkillsSO/PoisonPool");
                cooldownTime = offensiveSkillSO.skillCooldown;
                break;
        }
    }



    

}
