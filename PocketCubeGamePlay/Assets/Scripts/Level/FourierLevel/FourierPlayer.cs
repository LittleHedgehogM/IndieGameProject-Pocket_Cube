using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FourierPlayer : MonoBehaviour
{
    [SerializeField] float rotationSpeed = 720f;

    public Rigidbody myRigidbody;

    public float playerMoveSpeed = 5.0f;

    public float jumpStrength;
    public float fallStrength = 2.5f;

    public bool useGravity;
    public Vector3 gravity;
    Vector3 currentmoveDirection = Vector3.zero;
    Vector3 playerPrevPosition = Vector3.zero;

    //audio
    public AK.Wwise.Event jumpSFX;
    [SerializeField] private ParticleSystem jumpPS;



    void Start()
    {
        gravity = Physics.gravity;
        jumpPS.Stop();
        playerPrevPosition = transform.position;

    }

    public Vector3 getMovementDirection()
    {
        return currentmoveDirection;
    }


    public Vector3 getPlayerCurrentPosition()
    {
        return transform.position;
    }

    private void FixedUpdate()
    {
        // gravity adjust
        myRigidbody.useGravity = useGravity;

        // fall

        if (myRigidbody.velocity.y < 0)
        {
            myRigidbody.velocity += Vector3.up * Physics.gravity.y * (fallStrength - 1) * Time.deltaTime;


        }

        // move
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");


        Vector3 movementDirection = new Vector3(horizontalInput, 0, verticalInput);
        movementDirection.Normalize();

        transform.Translate(movementDirection * playerMoveSpeed * Time.deltaTime, Space.World);


        currentmoveDirection = (transform.position - playerPrevPosition).normalized;
        playerPrevPosition = transform.position;


        if (movementDirection != Vector3.zero)
        {
            Quaternion toRotation = Quaternion.LookRotation(movementDirection, Vector3.up);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, rotationSpeed * Time.deltaTime);
        }
    }

    
    private void OnTriggerStay(Collider col)
    {
        if (col.gameObject.CompareTag("Area"))
        {
            if (Input.GetKeyDown(KeyCode.Space) && myRigidbody.velocity.y <= 0.1)
            {
                jumpPS.Play();
                jumpSFX.Post(gameObject);

                //jump
                myRigidbody.velocity = Vector3.up * jumpStrength;
            }
        }
    }






}
