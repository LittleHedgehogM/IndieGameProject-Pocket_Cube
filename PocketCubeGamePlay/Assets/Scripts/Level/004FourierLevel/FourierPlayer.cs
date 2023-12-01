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
    Vector3 movementDirection;

    //audio
    [SerializeField] AK.Wwise.Event jumpSFX;
    [SerializeField] AK.Wwise.Event landSFX;
    [SerializeField] private ParticleSystem jumpPS;
    bool playerMovementEnabled;

    private bool jumpSwitch = false ;

    [SerializeField] Animator playerAni;

    void Start()
    {
        gravity = Physics.gravity;
        jumpPS.Stop();
        playerPrevPosition = transform.position;
        playerMovementEnabled = false;

    }

    private void OnEnable()
    {
        SceneTutorialController.TutorialEnds += enableMovement;
    }


    private void OnDisable()
    {
        SceneTutorialController.TutorialEnds -= enableMovement;
    }

    private void enableMovement()
    {
        playerMovementEnabled = true;
    }

    public Vector3 getMovementDirection()
    {
        return currentmoveDirection;
    }


    public Vector3 getPlayerCurrentPosition()
    {
        return transform.position;
    }

    private void Update()
    {
        
        if (!playerMovementEnabled) 
        {
            return;
        }
        if (jumpSwitch)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                //playerAni.SetTrigger("isJumping");
                //print("Space Down");
                jumpPS.Play();
                jumpSFX.Post(gameObject);
                landSFX.Post(gameObject);

                //jump
                myRigidbody.velocity = Vector3.up * jumpStrength;
                
            }
        }
             
        if  (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D))
        {
            playerAni.SetTrigger("isWalking");
            //print("walk");
            

        }
        else 
        if (!Input.GetKey(KeyCode.W) && !Input.GetKey(KeyCode.S) && !Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.D))
        {
            playerAni.SetTrigger("isIdle");
            //print("stop");
        }

    }

    void FixedUpdate()
    {
        if (!playerMovementEnabled)
        {
            return;
        }
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


        movementDirection = new Vector3(horizontalInput, 0, verticalInput);
        movementDirection.Normalize();

        transform.Translate(movementDirection * playerMoveSpeed * Time.deltaTime, Space.World);


        currentmoveDirection = (transform.position - playerPrevPosition).normalized;
        playerPrevPosition = transform.position;


        if (movementDirection != Vector3.zero)
        {
            Quaternion toRotation = Quaternion.LookRotation(movementDirection, Vector3.up);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, rotationSpeed * Time.deltaTime);
        }

        //jump
        /*if (Input.GetKeyDown(KeyCode.Space) && jumpSwitch)
        {
            jumpPS.Play();
            jumpSFX.Post(gameObject);

            //jump
           *//* myRigidbody.velocity = Vector3.up * jumpStrength;
        }*/

        
    }
    
    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.CompareTag("Area"))
        {
            jumpSwitch = true;
            print("jumpSwitch = true");
                /*if (Input.GetKeyDown(KeyCode.Space))
                {
                print("Space Down");
                    jumpPS.Play();
                    jumpSFX.Post(gameObject);

                    //jump
                    myRigidbody.velocity = Vector3.up * jumpStrength;
                }*/
        }
    }

    private void OnTriggerExit(Collider other)
    {
        jumpSwitch = false;
    }








}
