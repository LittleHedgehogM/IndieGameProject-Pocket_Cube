using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaneController : MonoBehaviour
{
    [Header("1:Bob 2:Straight 3:Lerp")]
    public int floatMode;
    Vector3 initialPosition;

    [Header("Bob Mode Settings")]
    public float bobSpeed;
    public Vector3 bobDirection;

    [Header("Straight Mode Settings")]
    public Vector3 straightDirection;
    public float straightSpeed = 1f;


    [Header("Lerp Mode Settings")]
    public Vector3 lerpDirection;
    public float lerpFloat = 0.1f;

    // Start is called before the first frame update
    void Start()
    {
        
            initialPosition = transform.position;
        
        
    }

    // Update is called once per frame
    void Update()
    {
        switch (floatMode)
        {
            case 1:

                transform.position = initialPosition  + bobDirection/2 * (-Mathf.Cos(Time.time * bobSpeed) + 1);
                break;
            case 2:

                transform.position = initialPosition + straightDirection * Mathf.PingPong(Time.time * straightSpeed, 1);
                break;
            case 3:
                
                transform.position = Vector3.Lerp(initialPosition, initialPosition + lerpDirection, Mathf.PingPong(Time.time, lerpFloat));
                break;   
        }
        //transform.position = bobInitPosition + bobDirection * Mathf.Sin(Time.time * bobFrequency) * bobMagnitude;
    }
}
