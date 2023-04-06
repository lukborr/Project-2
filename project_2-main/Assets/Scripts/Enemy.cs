using System.Collections;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private int damage = 2;
    private Health _health;
    private Coroutine routine;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            _health = collision.GetComponent<Health>();

            routine = StartCoroutine(RemoveHealth(_health, damage));
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(routine!= null)
        StopCoroutine(routine);
    }

    IEnumerator RemoveHealth(Health health, int damage)
    {
        while (health.health > 0)
        {
            yield return new WaitForSeconds(0.5f);
            health.RemoveHealth(damage);

        }
    }
}
