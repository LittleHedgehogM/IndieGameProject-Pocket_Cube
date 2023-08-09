using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerMovement : MonoBehaviour
{
    public float playerMoveSpeed = 5.0f;

    
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        transform.Translate(Vector3.right * horizontalInput * Time.deltaTime * playerMoveSpeed);
        transform.Translate(Vector3.forward * verticalInput * Time.deltaTime * playerMoveSpeed);
    }
}
