using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    [SerializeField] GameObject playerObject;
   [SerializeField] float speed;

    private void Start()
    {
        playerObject = GameObject.Find("Player");
    }

    private void Update()
    {  
        if (playerObject != null)
        transform.position = Vector2.MoveTowards(this.transform.position, playerObject.transform.position, speed * Time.deltaTime);
    }
    
}
