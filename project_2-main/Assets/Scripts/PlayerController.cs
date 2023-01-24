using System.Collections;
using System.Collections.Generic;
using UnityEditor.UIElements;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    Rigidbody2D playerb;
    [SerializeField] private float playerSpeed;
    private Camera mainCam;

    float horizontalInput, verticalInput;
    void Start()
    {
        playerb = GetComponent<Rigidbody2D>();
        mainCam = Camera.main;
    }

    private void Update()
    {
        GetInput();
        GetPointerInput();
    }

    private void FixedUpdate()
    {       
        MovePlayer();      
    }


    private void GetInput()
    {
         horizontalInput = Input.GetAxisRaw("Horizontal");
         verticalInput = Input.GetAxisRaw("Vertical");       
    }

    private void MovePlayer()
    {
        Vector2 move = new Vector2(horizontalInput * playerSpeed * Time.deltaTime, verticalInput * playerSpeed * Time.deltaTime);
        playerb.MovePosition(playerb.position + move);
    }

    private void GetPointerInput()
    {
        var mousePos = mainCam.ScreenToWorldPoint(Input.mousePosition);
        EventManager.CallPointerEvent(mousePos);       
    }

 
}
