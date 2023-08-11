using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeAutoRotate : MonoBehaviour
{
    public float speed = 25;

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(speed * Time.deltaTime, speed * Time.deltaTime, speed * Time.deltaTime, Space.Self);
    }
}
