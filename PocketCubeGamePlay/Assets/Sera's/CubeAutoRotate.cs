using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeAutoRotate : MonoBehaviour
{

    public float speed = 25;
    
    
    // Update is called once per frame

    public void StartMenuCubeAutoRotate()
    {
        transform.Rotate(speed * Time.deltaTime, speed * Time.deltaTime, speed * Time.deltaTime, Space.Self);
    }
}
