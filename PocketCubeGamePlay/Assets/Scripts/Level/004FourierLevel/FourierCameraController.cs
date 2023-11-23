using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FourierCameraController : CameraZoomInHelper
{
    [SerializeField] private Camera mainCam;
    [SerializeField][Range(4, 6)] private float cameraMoveRange;
    [SerializeField] private float Camera_Sensitivity;
    [SerializeField] private float Axis_X_Scaler;
    [SerializeField] private float Axis_Y_Scaler;

    [SerializeField] private FourierPlayer playerMovement;

    //[SerializeField] Transform camInitPos;
    [SerializeField] Vector3 CameraLookAtTarget;
    [SerializeField] Vector3 cameraMovement;
    [SerializeField] float cameraBeat;
    [SerializeField] GameObject camPerform;

    [SerializeField] private Transform CubeTransform;

    float time;
    Vector3 MainCamInitPosition;
    //Quaternion MainCamInitRotation;

    bool disableCameraMovement = false;

    private void OnEnable()
    {
        CubeClickEvent.CubeClick += zoomInCube;
    }

    private void OnDisable()
    {
        CubeClickEvent.CubeClick -= zoomInCube;
    }


    private void Start()
    {
        mainCam.transform.LookAt(CameraLookAtTarget, Vector3.up);
        MainCamInitPosition = this.transform.position;
        //MainCamInitRotation = mainCam.transform.rotation;
    }

    void Update()
    {

        if (disableCameraMovement) 
        {
            return;        
        }
        if (camPerform.activeSelf)
        {
            onUpdateCameraWithPlayerMovement(playerMovement.getMovementDirection());
        }
        else
        {
            MainCamInitPosition = camPerform.transform.position;
            onUpdateCameraWithPlayerMovementWithFloat(playerMovement.getMovementDirection());
        }
        

        //mainCam.transform.position = Mathf.Sin(Camera_Sensitivity * Time.deltaTime) * cameraMovement;
        //mainCam.transform.LookAt(CameraLookAtTarget, Vector3.up);

    }

    public void onUpdateCameraWithPlayerMovement(Vector3 playerMovementVector)
    {
        float dist = Vector3.Distance(mainCam.transform.position, MainCamInitPosition);
        if (dist < cameraMoveRange)
        {
            
            playerMovementVector.y *= Axis_Y_Scaler;
            playerMovementVector.x *= Axis_X_Scaler;
            //float x = 0;
            time += Time.deltaTime;
            //x = time;
            //print(Mathf.Sin(x) * cameraMovement);
            mainCam.transform.position += Camera_Sensitivity * playerMovementVector;
            mainCam.transform.LookAt(CameraLookAtTarget, Vector3.up);
        }
        else
        {
            
            mainCam.transform.position += Camera_Sensitivity * (MainCamInitPosition - mainCam.transform.position);
            mainCam.transform.LookAt(CameraLookAtTarget, Vector3.up);
        }
    }

    public void onUpdateCameraWithPlayerMovementWithFloat(Vector3 playerMovementVector)
    {
        float dist = Vector3.Distance(mainCam.transform.position, MainCamInitPosition);
        if (dist < cameraMoveRange)
        {

            playerMovementVector.y *= Axis_Y_Scaler;
            playerMovementVector.x *= Axis_X_Scaler;
            float x = 0;
            time += Time.deltaTime;
            x = time;
            //print(Mathf.Sin(x) * cameraMovement);
            mainCam.transform.position += Camera_Sensitivity * (playerMovementVector + Mathf.Sin(x * cameraBeat) * cameraMovement);
            mainCam.transform.LookAt(CameraLookAtTarget, Vector3.up);
        }
        else
        {

            mainCam.transform.position += Camera_Sensitivity * (MainCamInitPosition - mainCam.transform.position);
            mainCam.transform.LookAt(CameraLookAtTarget, Vector3.up);
        }
    }

    public override void zoomInCube()
    {
        disableCameraMovement = true;
        zoomInCube(mainCam, CubeTransform, CameraLookAtTarget);

    }
}
