using System.Collections;
using UnityEngine;

public class Thunderbolt : Skillshot  
{

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        base.OnTriggerEnter2D(collision);
       
        ShockStun(collision);      
    }
}
