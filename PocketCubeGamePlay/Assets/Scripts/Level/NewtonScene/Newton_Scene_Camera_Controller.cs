using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static System.TimeZoneInfo;
using static UnityEngine.GraphicsBuffer;

public class Scene_Newton_Camera_Controller : MonoBehaviour
{
    [SerializeField] private Camera mainCam;
    [SerializeField] private Camera Cam_LookAt_Eye_L;
    [SerializeField] private Camera Cam_LookAt_Eye_R;
    [SerializeField] private Camera Cam_LookAt_Scale_L;
    [SerializeField] private Camera Cam_LookAt_Scale_R;
    [SerializeField] private float Camera_Sensitivity;
    [SerializeField] Vector3 CameraLookAtTarget;
    [SerializeField] private float translationTime;

    [SerializeField][Range (4, 6)] private float cameraMoveRange;
    
    public static event Action zoomInFinish;
    public static event Action ResetFinish;

    Vector3 MainCamInitPosition;
    Quaternion MainCamInitRotation;
    float MainCamInitScale;

    // -------------- Audio
    [SerializeField] private AK.Wwise.Event cameraZoomIn;
    [SerializeField] private AK.Wwise.Event cameraZoomOut;

    private void Start()
    {
        mainCam.transform.LookAt(CameraLookAtTarget, Vector3.up);
        MainCamInitPosition = mainCam.transform.position;
        MainCamInitRotation = mainCam.transform.rotation;
        MainCamInitScale = mainCam.GetComponent<Camera>().orthographicSize;
    }
    public bool isMainCam()
    {
        return mainCam.enabled;
    }

    public Camera getCurrentCamera()
    {
        return mainCam;
    }

    public Camera GetScaleLeftCamera()
    {
        return Cam_LookAt_Scale_L;
    }

    public Camera GetScaleRightCamera()
    {
        return Cam_LookAt_Scale_R;
    }

    public void onUpdateCameraWithPlayerMovement(Vector3 playerMovementVector)
    {
        float dist = Vector3.Distance(mainCam.transform.position, MainCamInitPosition);
        if (dist < cameraMoveRange)
        {
            mainCam.transform.position += Camera_Sensitivity * playerMovementVector;
            mainCam.transform.LookAt(CameraLookAtTarget, Vector3.up);
        }
        else
        {
            mainCam.transform.position += Camera_Sensitivity * (MainCamInitPosition-mainCam.transform.position);
            mainCam.transform.LookAt(CameraLookAtTarget, Vector3.up);
        }    
    }

    public void showLeftEye()
    {
        //mainCam.enabled = false;
        //Cam_LookAt_Eye_L.enabled = true;
        //Cam_LookAt_Scale_L.enabled = true;
        StartCoroutine(ZoomInTo(Cam_LookAt_Eye_L));
    }

    public void showRightEye()
    {
        //mainCam.enabled = false;
        //Cam_LookAt_Eye_R.enabled = true;
        //Cam_LookAt_Scale_R.enabled = true;

        StartCoroutine(ZoomInTo(Cam_LookAt_Eye_R));
    }

    public void showRightScale()
    {
        StartCoroutine(ZoomInTo(Cam_LookAt_Scale_R));
        
    }

    public void showLeftcale()
    {
        StartCoroutine(ZoomInTo(Cam_LookAt_Scale_L));
    }

    private IEnumerator ZoomInTo(Camera targetCam)
    {  
        

        float currentUsedTime = 0;
        float t = 0;
        Vector3 startPosition = mainCam.transform.position;
        Quaternion startRotation = mainCam.transform.rotation;
        float startSize = mainCam.GetComponent<Camera>().orthographicSize;
        float endSize = targetCam.GetComponent<Camera>().orthographicSize;

        // -------------- Audio
        //print(targetCam.name + startPosition + Cam_LookAt_Scale_R.transform.position);
        if (targetCam.name == "Camera_LookAt_Eye_R" & startPosition == Cam_LookAt_Scale_R.transform.position )
        {
            cameraZoomOut.Post(gameObject);
            //print("back to eyes");
        }
        else if (targetCam.name == "Camera_LookAt_Eye_L" & startPosition == Cam_LookAt_Scale_L.transform.position)
        {
            cameraZoomOut.Post(gameObject);
            //print("back to eyes");
        }
        else
        {
            cameraZoomIn.Post(gameObject);
        }
        // -------------- Audio

        while (t < 1)
        {
            currentUsedTime += Time.deltaTime;
            t = currentUsedTime / translationTime;

            

            mainCam.transform.position = Vector3.Lerp(startPosition, targetCam.transform.position, t);
            mainCam.transform.rotation = Quaternion.Lerp(startRotation, targetCam.transform.rotation, t);
            mainCam.GetComponent<Camera>().orthographicSize = Mathf.Lerp(startSize, endSize, t);

            //print("ZoomIn" + targetCam.name);

            yield return null;
        }

        zoomInFinish?.Invoke();
    }

    public IEnumerator TranslateBackToInitPosition()
    {
        // -------------- Audio
        cameraZoomOut.Post(gameObject);
        // -------------- Audio

        float currentUsedTime = 0;
        float t = 0;
        Vector3 startPosition = mainCam.transform.position;
        Quaternion startRotation = mainCam.transform.rotation;
        float size = mainCam.GetComponent<Camera>().orthographicSize;

        

        while (t < 1)
        {
            currentUsedTime += Time.deltaTime;
            t = currentUsedTime / translationTime;

            

            mainCam.transform.position = Vector3.Lerp(startPosition, MainCamInitPosition, t);
            mainCam.transform.rotation = Quaternion.Lerp(startRotation, MainCamInitRotation, t);
            mainCam.GetComponent<Camera>().orthographicSize = Mathf.Lerp(size, MainCamInitScale, t);

            

            yield return null;
        }

        ResetFinish?.Invoke();
    }



    public void resetCam()
    {
        //mainCam.enabled = true;
        //Cam_LookAt_Eye_L.enabled = false;
        //Cam_LookAt_Eye_R.enabled = false;
        //Cam_LookAt_Scale_L.enabled= false;
        //Cam_LookAt_Scale_R.enabled= false;
        StartCoroutine(TranslateBackToInitPosition());
    }
}
