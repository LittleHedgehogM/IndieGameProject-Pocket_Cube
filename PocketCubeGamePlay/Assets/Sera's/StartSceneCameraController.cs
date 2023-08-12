using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartSceneCameraController : MonoBehaviour
{
    public Transform InitCam;
    public Transform EndCam;
    public float lerpFloat = 0.1f;

    bool gameStart = false;
    GameObject mainCamera;

    private void Start()
    {
        mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
        mainCamera.transform.position = InitCam.position;

    }
    private void Update()
    {
        if (gameStart)
        {
            
            mainCamera.transform.position = Vector3.Lerp(mainCamera.transform.position, EndCam.position, lerpFloat);
        }
            
    }

    public void MoveToLevelScene()
    {
        gameStart = true;
    }
}

 

