using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Banana : MonoBehaviour
{

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Health health = collision.gameObject.GetComponent<Health>();
            if(health.health < health.maxHealth)
            {
                EventManager.CallOnHealEvent(25);
                StartCoroutine(DestroyBanana());
            }
            
        }        
    }
    private IEnumerator DestroyBanana()
    {
        yield return new WaitForSeconds(0.5f);
        Destroy(gameObject);
    }
}


