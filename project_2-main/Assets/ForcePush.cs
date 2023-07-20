using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForcePush : Skillshot
{
    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        base.OnTriggerEnter2D(collision);

        // pushnac w przeciwna strone do reki
    }
}
