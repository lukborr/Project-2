using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElectricBall : Skillshot
{
    [SerializeField] private GameObject rotationPoint;

    private void FixedUpdate()
    {
       rotationPoint.transform.Rotate(new Vector3(0, 0, 1) * skillSpeed * Time.fixedDeltaTime);
    }


    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        base.OnTriggerEnter2D(collision);  
    }


}
