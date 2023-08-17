using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.PlayerSettings;
using static UnityEngine.GraphicsBuffer;

public class CubePlayCameraController : MonoBehaviour
{

    [SerializeField] private Camera mainCam;
    [SerializeField] private Vector3 initialCameraPosition = new Vector3(0, 0, -10);
    [SerializeField][Range(0, 1)] private float initialCameraX = 0.1f;
    [SerializeField][Range(0, 1)] private float initialCameraY = 0.1f;
    [SerializeField] AnimationCurve translationCurve;

    [SerializeField] private Camera rightCam;
    [SerializeField] private Camera leftCam;
    [SerializeField] private Camera upCam;
    [SerializeField] private Camera downCam;
    [SerializeField] private Camera frontCam;
    [SerializeField] private Camera backCam;

    Vector3 startPosition;
    Quaternion startRotation;

    Vector3 backStartPosition;
    Quaternion backStartRotation;

    float currentUsedTime;
    [SerializeField][Range(0, 1)]  float totalTime;
    // Start is called before the first frame update
    void Start()
    {
        InitCameraPosition();
        mainCam.transform.LookAt(this.transform.position);
        mainCam.orthographic = true;
        currentUsedTime = 0;
    }


    void InitCameraPosition()
    {
        mainCam.transform.position = Vector3.zero;
        RotateCamera(mainCam,   new Vector2(initialCameraX, initialCameraY));
        RotateCamera(rightCam,  new Vector2(0.5f, 0));
        RotateCamera(leftCam,   new Vector2(-0.5f, 0));
        RotateCamera(upCam,     new Vector2(0, 0.5f));
        RotateCamera(downCam,   new Vector2(0, -0.5f));
        RotateCamera(frontCam,  new Vector2(0, 0));
        RotateCamera(backCam,   new Vector2(1, 0));
    }


    public void RotateMainCamera(Vector2 direction)
    {
        RotateCamera(mainCam, direction);
    }

    private void RotateCamera(Camera cam, Vector2 direction)
    {
        cam.transform.position = Vector3.zero;
        cam.transform.Rotate(new Vector3(1, 0, 0), direction.y * 180);
        cam.transform.Rotate(new Vector3(0, 1, 0), -direction.x * 180, Space.World);
        cam.transform.Translate(initialCameraPosition);

    }

    public void initTranslation()
    {
        currentUsedTime = 0;
        startPosition = mainCam.transform.position;
        startRotation = mainCam.transform.rotation;
    }
    public void initTranslationBack()
    {
        currentUsedTime = 0;
        backStartPosition = mainCam.transform.position;
        backStartRotation = mainCam.transform.rotation;
    }

    public bool TranslateCameraToRight()
    {
        return TranslateCameraTo(rightCam);
    }

    public bool TranslateCameraToLeft()
    {
        return TranslateCameraTo(leftCam);
    }

    public bool TranslateCameraToFront()
    {
        return TranslateCameraTo(frontCam);
    }

    public bool TranslateCameraToBack()
    {
        return TranslateCameraTo(backCam);
    }


    public bool TranslateCameraToUp()
    {
        return TranslateCameraTo(upCam);
    }

    public bool TranslateCameraToDown()
    {
        return TranslateCameraTo(downCam);
    }

    private bool TranslateCameraTo(Camera targetCamera)
    {
        currentUsedTime += Time.deltaTime;
        float t = currentUsedTime / totalTime;

        mainCam.transform.position = Vector3.Slerp(startPosition, targetCamera.transform.position, translationCurve.Evaluate(t));
        mainCam.transform.rotation = Quaternion.Slerp(startRotation, targetCamera.transform.rotation, translationCurve.Evaluate(t));                   
        return t >= 1;

    }

    public bool TranslateCameraBack()
    {
        currentUsedTime += Time.deltaTime;
        float t = currentUsedTime / totalTime;

        mainCam.transform.position = Vector3.Slerp(backStartPosition, startPosition, t);
        mainCam.transform.rotation = Quaternion.Slerp(backStartRotation, startRotation, t);
        return t >= 1;
    }

    public Vector3 ScreenToViewportPoint(Vector3 position)
    {
        return mainCam.ScreenToViewportPoint(position);
    }

}
