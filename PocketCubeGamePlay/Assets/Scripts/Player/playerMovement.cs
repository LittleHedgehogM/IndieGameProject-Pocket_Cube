using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public Rigidbody myRigidbody;

    public float playerMoveSpeed = 5.0f;

    public float jumpStrength;
    public float fallStrength = 2.5f;

    public bool useGravity;
    public Vector3 gravity;


    void Start()
    {
       //gravity = Physics.gravity;
    }


    public Vector3 getPlayerCurrentPosition()
    {
        return transform.position;
    }

    // Update is called once per frame
    void Update() 
    {
        // jump
        if (Input.GetKeyDown(KeyCode.Space) && myRigidbody.velocity.y == 0)
        {
            myRigidbody.velocity = Vector3.up * jumpStrength;
        }
        if (myRigidbody.velocity.y < 0)
        {
            myRigidbody.velocity += Vector3.up  * Physics.gravity.y * (fallStrength - 1) * Time.deltaTime;
        }

        // move
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");
        transform.Translate(Vector3.right * horizontalInput * Time.deltaTime * playerMoveSpeed);
        transform.Translate(Vector3.forward * verticalInput * Time.deltaTime * playerMoveSpeed);
    }

    private void FixedUpdate()
    {
        // gravity adjust
        myRigidbody.useGravity = useGravity;
        
        myRigidbody.AddForce(gravity);
        
        //Physics.gravity = gravity;
    }
}
