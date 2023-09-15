using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircuitLevel_PlayerMovement : MonoBehaviour
{


    //[SerializeField] Rigidbody myRigidbody;
    [SerializeField][Range(0, 5)] float playerMoveSpeed = 5.0f;
    [SerializeField] float rotationSpeed = 720f;


    Vector3 currentmoveDirection;

    private bool isMovementEnabled = true;
    public void setEnableMovement(bool enable)
    {
        isMovementEnabled = enable;
    }

    void Start()
    {
        currentmoveDirection = Vector3.zero;
    }

    public Vector3 getPlayerCurrentPosition()
    {
        return transform.position;
    }

    public Vector3 getMovementDirection()
    {
        return currentmoveDirection;
    }

    // Update is called once per frame
    public void Update()
    {
        if (isMovementEnabled)
        {
            // move
            float horizontalInput = Input.GetAxis("Horizontal");
            float verticalInput = Input.GetAxis("Vertical");


            Vector3 movementDirection = new Vector3(horizontalInput, 0, verticalInput);
            movementDirection.Normalize();

            currentmoveDirection = movementDirection;

            transform.Translate(movementDirection * playerMoveSpeed * Time.deltaTime, Space.World);

            if (movementDirection != Vector3.zero)
            {
                Quaternion toRotation = Quaternion.LookRotation(movementDirection, Vector3.up);
                transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, rotationSpeed * Time.deltaTime);
            }
        }

    }
}
