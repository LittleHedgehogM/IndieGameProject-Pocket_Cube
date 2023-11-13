using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static System.TimeZoneInfo;

public class Electro_Camera_Controller : MonoBehaviour
{

    [SerializeField] private Camera mainCam;
    [SerializeField] private Camera sunCam;
    [SerializeField] private Camera moonCam;
    [SerializeField] private Camera starCam;
    [SerializeField] [Range(4, 6)] private float cameraMoveRange;
    [SerializeField] private float translationTime;
    [SerializeField] private float Camera_Sensitivity;
    [SerializeField] Vector3 CameraLookAtTarget;

    public static event Action TranslateCameraFinish;
    public static event Action ResetCameraFinish;

    Vector3     MainCamInitPosition;
    Quaternion  MainCamInitRotation;
    float       MainCamInitScale;
    bool isResettingCam;

    //-------Audio
    [Header("Audio")]
    [SerializeField] private AK.Wwise.Event btn01Sound;
    [SerializeField] private AK.Wwise.Event btn02Sound;
    private void Start()
    {
        mainCam.transform.LookAt(CameraLookAtTarget, Vector3.up);
        MainCamInitPosition = mainCam.transform.position;
        MainCamInitRotation = mainCam.transform.rotation;
        MainCamInitScale = mainCam.GetComponent<Camera>().orthographicSize;
        isResettingCam = false;
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
            mainCam.transform.position += Camera_Sensitivity * (MainCamInitPosition - mainCam.transform.position);
            mainCam.transform.LookAt(CameraLookAtTarget, Vector3.up);
        }
    }


    public GameObject checkCollision()
    {
        GameObject hitObject = null;
        RaycastHit hit;
        Ray ray;
        ray = mainCam.ScreenPointToRay(Input.mousePosition);

        if (Input.GetMouseButtonUp(0) && Physics.Raycast(ray, out hit))
        {
            hitObject = hit.collider.gameObject;
            print("Hit Object" + hitObject.name);
            //Audio
            if (hitObject.tag == "Level2Btn01")
            {
                btn01Sound.Post(gameObject);
            }
            else if (hitObject.tag == "Level2Btn02")
            {
                btn02Sound.Post(gameObject);
            }
        }
        return hitObject;
    }


    public bool isStarCam()
    {
        return Vector3.Distance(mainCam.transform.position, starCam.transform.position) < 0.001f;
    }

    public bool isSunCam()
    {
        return Vector3.Distance(mainCam.transform.position, sunCam.transform.position) < 0.001f;
    }

    public bool isMoonCam()
    {
        return Vector3.Distance(mainCam.transform.position, moonCam.transform.position) < 0.001f;
    }

    public void showStarCam()
    {
        StartCoroutine(TranslateTo(starCam, false));
    }

    public void showSunCam()
    {
        StartCoroutine(TranslateTo(sunCam, false));
    }

    public void showMoonCam()
    {
        StartCoroutine(TranslateTo(moonCam, false));
    }


    public void resetSunCam()
    {
        StartCoroutine(TranslateBackToInitPosition(false));
    }

    public void resetMoonCam()
    {
        StartCoroutine(TranslateBackToInitPosition(false));
    }

    public void resetCam()
    {
        if (!isResettingCam){   
            StartCoroutine(TranslateBackToInitPosition(false));
            isResettingCam = true;
        }
        
    }

    private IEnumerator TranslateTo(Camera targetCam, bool isLookAtTarget)
    {

        float currentUsedTime = 0;
        float t = 0;
        Vector3 startPosition = mainCam.transform.position;
        Quaternion startRotation = mainCam.transform.rotation;
        float startSize = mainCam.GetComponent<Camera>().orthographicSize;
        float endSize = targetCam.GetComponent<Camera>().orthographicSize;

        // -------------- Audio
        
        // -------------- Audio

        while (t < 1)
        {
            currentUsedTime += Time.deltaTime;
            t = currentUsedTime / translationTime;
            mainCam.transform.position = Vector3.Slerp(startPosition, targetCam.transform.position, t);
            mainCam.transform.rotation = Quaternion.Slerp(startRotation, targetCam.transform.rotation, t);
            if (isLookAtTarget)
            {
                mainCam.transform.LookAt(CameraLookAtTarget);
            }
            
            mainCam.GetComponent<Camera>().orthographicSize = Mathf.Lerp(startSize, endSize, t);
            yield return null;
        }

        TranslateCameraFinish?.Invoke();
    }

    public IEnumerator TranslateBackToInitPosition(bool isLookAtTarget)
    {
        // -------------- Audio
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
            mainCam.transform.position = Vector3.Slerp(startPosition, MainCamInitPosition, t);
            mainCam.transform.rotation = Quaternion.Slerp(startRotation, MainCamInitRotation, t);
            mainCam.GetComponent<Camera>().orthographicSize = Mathf.Lerp(size, MainCamInitScale, t);

            if (isLookAtTarget)
            {
                mainCam.transform.LookAt(CameraLookAtTarget);
            }
            yield return null;
        }

        ResetCameraFinish?.Invoke();
        isResettingCam = false;
    }
}

