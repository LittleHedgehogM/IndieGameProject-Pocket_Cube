using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartSceneCameraController : MonoBehaviour
{
    public Transform InitCam;
    public Transform EndCam;
    public float lerpFloat = 0.1f;
    public bool gameStart = false;
    GameObject mainCamera;

    //MapCube Auto Rotate
    private GameObject mapCube;
    private CubeAutoRotate cubeAutoRotate;


    private void Start()
    {
        mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
        mainCamera.transform.position = InitCam.position;

        //MapCube Auto Rotate
        mapCube = GameObject.Find("MapCube");
        cubeAutoRotate = mapCube.GetComponent<CubeAutoRotate>();
    }
    private void Update()
    {
        if (!gameStart)
        {
            //MapCube Auto Rotate
            cubeAutoRotate.StartMenuCubeAutoRotate();
        }
        else if(gameStart)
        {
            //Camera Zoom In
            mainCamera.transform.position = Vector3.Lerp(mainCamera.transform.position, EndCam.position, lerpFloat);
        }
            
    }

    public void MoveToLevelScene()
    {
        gameStart = true;
    }
}

 

