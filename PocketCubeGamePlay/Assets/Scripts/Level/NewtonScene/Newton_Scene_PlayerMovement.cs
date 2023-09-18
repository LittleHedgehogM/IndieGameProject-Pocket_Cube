using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Newton_Scene_PlayerMovement : MonoBehaviour
{
    //[SerializeField] Rigidbody myRigidbody;
    [SerializeField][Range(0,5)] float playerMoveSpeed = 5.0f;
    [SerializeField] float rotationSpeed = 720f;

    //public bool useGravity;
    //public Vector3 gravity;


    [SerializeField] private Transform targetTransformLeft;
    [SerializeField] private Transform targetTransformRight;
    [SerializeField] private Transform EyeLeft;
    [SerializeField] private Transform EyeRight;
    [SerializeField] float translationTime;

    Vector3 currentmoveDirection;

    private bool isMovementEnabled = true;
    public void setEnableMovement(bool enable)
    {
        isMovementEnabled = enable;
    }

    public void TranslateToLeftEye()
    {
        StartCoroutine(TranslateToTarget(targetTransformLeft));

    }

    public void TranslateToRightEye()
    {
        StartCoroutine(TranslateToTarget(targetTransformRight));
    }

    private IEnumerator TranslateToTarget(Transform target)
    {
        float currentUsedTime = 0;
        float t = 0;
        Vector3 startPosition = transform.position;
        Vector3 targetPosition = new Vector3(target.transform.position.x, transform.position.y, target.transform.position.z);

        Quaternion startRotation = transform.rotation;
        while (t < 1)
        {
            currentUsedTime += Time.deltaTime;
            t = currentUsedTime / translationTime;

            transform.position = Vector3.Lerp(startPosition, targetPosition, t);
            transform.rotation = Quaternion.Lerp(startRotation, target.rotation, t);

            yield return null;
        }
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
    public void OnUpdate()
    {
        if (isMovementEnabled)
        {
            // move
            float horizontalInput = Input.GetAxis("Horizontal");
            float verticalInput = Input.GetAxis("Vertical");


            Vector3 movementDirection = new Vector3(horizontalInput, 0, verticalInput);
            movementDirection.Normalize();

            currentmoveDirection = movementDirection;

            transform.Translate(movementDirection*playerMoveSpeed*Time.deltaTime, Space.World);

            if (movementDirection != Vector3.zero)
            {
                Quaternion toRotation = Quaternion.LookRotation(movementDirection, Vector3.up);
                transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, rotationSpeed*Time.deltaTime);
            }
        }

    }

    //private void FixedUpdate()
    //{
    //    // gravity adjust
    //    //myRigidbody.useGravity = useGravity;

    //    //myRigidbody.AddForce(gravity);

    //    //Physics.gravity = gravity;
    //}
}
