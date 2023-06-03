using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ignite : Skillshot
{
    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        base.OnTriggerEnter2D(collision);
        if (collision.CompareTag("Enemy"))
        {
            GameObject burn = collision.transform.Find("burn").gameObject;
            burn.SetActive(true);
            
        }
        // odpalic rutyne konczaca dot
    }

    protected override void OnTriggerExit2D(Collider2D collision)
    {
        
    }



}
