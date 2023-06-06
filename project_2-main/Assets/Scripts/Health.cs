using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Health : MonoBehaviour
{
  [HideInInspector] public int health;
    [SerializeField] private GameObject DamageOutput;
   // public Coroutine dotRoutine;
    private int counter = 2;
    [SerializeField] EnemySO enemySO;



    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.T)) 
        {
            
        }
    }
    void Start()
    {
        if (gameObject.CompareTag("Player"))
        {
            health = 100;
        }
        else
        {
            health = enemySO.enemyHealth;
        }   
        
    }

    public void RemoveHealth(int healthToRemove)
    {
        health -=healthToRemove;
        InstantiateDamageOutput(healthToRemove);


        if (health <= 0)
        {
            if (gameObject.CompareTag("Player"))
            {
                Destroy(gameObject);
            }             
            else if(gameObject.CompareTag("Enemy"))
            {
                DieAndDrop();
            }
        }
    }

    public IEnumerator RemoveHealthGradually(int healthToRemove, float duration)
    {
        bool dotFinished = false;
        if (duration != -1)
        {
            StartCoroutine(Timer(duration, dotFinished));
        }
        while(dotFinished == false && health > 0)
        {
            yield return new WaitForSeconds(0.5f);
            health -= healthToRemove;
            InstantiateDamageOutput(healthToRemove);         
            if(health <= 0) 
            {
                DieAndDrop();
            }
        }
    }

    private IEnumerator Timer(float duration, bool ready)
    {
        float timer = 0.0f;
        while(timer != duration)
        {
            yield return new WaitForSeconds(1);
            timer++;
            
            if(timer >= duration)
            {
                ready = true;
            }               
        }                
    }

    public Coroutine StartDotRoutine(int damage, float duration)
    {      
     Coroutine dotRoutine = StartCoroutine(RemoveHealthGradually(damage, duration));
        return dotRoutine;
    }


    private void DieAndDrop()
    {
        Transform childTransform = transform.GetChild(0);
        childTransform.gameObject.SetActive(true);
        childTransform.transform.parent = null;       
        Destroy(gameObject);
    }

    private void InstantiateDamageOutput(int damage)
    {
        counter++;
        if(counter >= 6)
        {
            counter = 2;
        }       
        GameObject dmgOutput = Instantiate(DamageOutput);
        MeshRenderer meshRen = dmgOutput.GetComponent<MeshRenderer>();
        meshRen.sortingOrder = counter;
        TextMeshPro textMeshpro = dmgOutput.GetComponent<TextMeshPro>();
        textMeshpro.text = damage.ToString();
        dmgOutput.transform.position = gameObject.transform.position;
    }

   public IEnumerator StopDotRoutine(Coroutine routine)
    {
        yield return null;   
        StopCoroutine(routine);

    }
}
