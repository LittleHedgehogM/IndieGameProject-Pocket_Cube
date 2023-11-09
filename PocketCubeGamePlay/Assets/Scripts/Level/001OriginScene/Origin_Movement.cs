using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Origin_Movement : MonoBehaviour
{

    [SerializeField] Camera mainCam;
    [SerializeField] Transform Sphere;
    [SerializeField][Range(0, 3)] float playerMoveSpeed;
    [SerializeField][Range(0, 0.5f)] float rotateSensitivity;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        Vector3 movementDirection = new Vector3(horizontalInput, 0, verticalInput);
        movementDirection.Normalize();
        transform.Translate(movementDirection * playerMoveSpeed * Time.deltaTime, Space.World);


        Sphere.rotation = Quaternion.Euler(movementDirection.z * rotateSensitivity, -movementDirection.x * rotateSensitivity, 0) * Sphere.rotation;
    }
}
