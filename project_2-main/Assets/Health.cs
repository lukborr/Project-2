using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    private int health;
    // Start is called before the first frame update
    void Start()
    {
        health = 2;  // tu wstawic scriptableObject

    }

    public void RemoveHealth(int healthToRemove)
    {
        health -=healthToRemove;

        if (health <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Destroy(gameObject);
    }

}
