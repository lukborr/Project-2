using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Health : MonoBehaviour
{
  [HideInInspector] public int health;
    [SerializeField] private GameObject DamageOutput;
    public Coroutine dotRoutine;
    private int counter = 2;
    
    void Start()
    {
        health = 100;  // tu wstawic scriptableObject
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

    public IEnumerator RemoveHealthGradually(int healthToRemove)
    {       
        while(health > 0)
        {
            yield return new WaitForSeconds(0.5f);
            health -= healthToRemove;
            InstantiateDamageOutput(healthToRemove);
            //Debug.Log("zabrało" + healthToRemove);
            if(health <= 0) 
            {
                DieAndDrop();
            }
        }
    }

    public void StartDotRoutine(int damage)
    {      
       dotRoutine = StartCoroutine(RemoveHealthGradually(damage));       
    }


    private void DieAndDrop()
    {
        transform.GetChild(0).gameObject.SetActive(true);
        transform.DetachChildren();
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


}
