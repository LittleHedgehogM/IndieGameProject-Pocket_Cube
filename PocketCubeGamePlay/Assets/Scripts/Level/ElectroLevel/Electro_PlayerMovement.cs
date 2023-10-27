using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Electro_PlayerMovement : MonoBehaviour
{
    //Electro_Camera_Controller myCameraController;

    [SerializeField][Range(0, 5)] float playerMoveSpeed = 5.0f;
    [SerializeField] float rotationSpeed = 720f;
    [SerializeField] float translationTime = 1.0f;
    [SerializeField]  Transform orientation;
    Vector3 currentmoveDirection;
    public static event Action TranslationFinish;




    private bool isMovementEnabled = true;
    public void setEnableMovement(bool enable)
    {
        isMovementEnabled = enable;
    }

    public void TranslateTo(Transform target)
    {
        StartCoroutine(TranslateToTarget(target));
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

        TranslationFinish?.Invoke();
    }

    void Start()
    {
        currentmoveDirection = Vector3.zero;
        //myCameraController = FindObjectOfType<Electro_Camera_Controller>();

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

            Vector3 viewDir = transform.position - new Vector3(Camera.main.transform.position.x, 
                                transform.position.y, Camera.main.transform.position.z);
            orientation.forward = viewDir.normalized;
            float horizontalInput = Input.GetAxis("Horizontal");
            float verticalInput = Input.GetAxis("Vertical");

            Vector3 movementDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;/*new Vector3(horizontalInput, 0, verticalInput);*/

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
