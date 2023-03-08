using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Health : MonoBehaviour
{
  [HideInInspector] public int health;
    [SerializeField] private GameObject DamageOutput;

    private int counter = 2;
    
    void Start()
    {
        health = 10;  // tu wstawic scriptableObject
    }

    public void RemoveHealth(int healthToRemove)
    {
        health -=healthToRemove;
        InstantiateDamageOutput(healthToRemove);

        if (health <= 0)
        {
            Die();
        }
    }

    public IEnumerator RemoveHealthGradually(int healthToRemove)
    {
        while(health > 0)
        {
            yield return new WaitForSeconds(0.5f);
            health -= healthToRemove;
            InstantiateDamageOutput(healthToRemove);
        }           
    }

    private void Die()
    {
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
