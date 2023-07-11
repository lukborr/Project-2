using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CometCall : Skillshot
{
    [SerializeField] private GameObject cometObject;
    private Vector2 offset;

    private void Awake()
    {
        offset = new Vector2(-2, 4);
    }
    private void OnEnable()
    {
        GameObject go = Instantiate(cometObject, (Vector2)transform.position + offset, cometObject.transform.rotation);
        go.GetComponent<Comet>().targetPosition = transform.position;      
    }
}
