using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
  [HideInInspector] public int health;
    
    void Start()
    {
        health = 10;  // tu wstawic scriptableObject
    }

    public void RemoveHealth(int healthToRemove)
    {
        health -=healthToRemove;

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
            Debug.Log("removed " + healthToRemove);
        }           
    }

    private void Die()
    {
        Destroy(gameObject);
    }

}
