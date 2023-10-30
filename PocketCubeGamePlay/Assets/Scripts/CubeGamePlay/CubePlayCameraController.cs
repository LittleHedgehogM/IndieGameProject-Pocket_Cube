using System;
using System.Collections;
using System.Collections.Generic;
//using System.Numerics;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class CubePlayCameraController : MonoBehaviour
{

    [SerializeField] private Camera mainCam;
    [SerializeField] private Vector3 initialCameraPosition = new Vector3(0, 0, -10);
    [SerializeField][Range(0, 1)] private float initialCameraX = 0.1f;
    [SerializeField][Range(0, 1)] private float initialCameraY = 0.1f;

    [SerializeField] private AnimationCurve translationCurve;
    [SerializeField][Range(0, 3)] private float translationTimePhase1;


    [SerializeField] private AnimationCurve resetCameraCurve;
    [SerializeField][Range(0, 3)] private float resetCameraTime;

    [SerializeField] private Camera rightCam;
    [SerializeField] private Camera leftCam;
    [SerializeField] private Camera upCam;
    [SerializeField] private Camera downCam;
    [SerializeField] private Camera frontCam;
    [SerializeField] private Camera backCam;
    [SerializeField] private Camera initialCam;


    [SerializeField] private Transform translateHelper;
    [SerializeField] private Transform translateBackHelper;

    [SerializeField] private GameObject PocketCube;

    Vector3 startPosition;
    Quaternion startRotation;
    //Quaternion targetQuaternion;
    float currentUsedTime;
    float currentRotationDegree = 0;
    Vector3 translationRotateAxis;
    float translationRotateDegree = 0;

    float currentCameraX;
    float currentCameraY;

    //List<Vector3> candidateUpAxis;

    // Start is called before the first frame update
    void Start()
    {
        InitCameraPosition();

    }

    private void OnEnable()
    {
        CubeInPlayPhase.onCubeSolved += CubeSolved;
    }

    private void OnDisable()
    {
        CubeInPlayPhase.onCubeSolved -= CubeSolved;
    }


    public void onRestart()
    {
        InitCameraPosition();
    }

    void InitCameraPosition()
    {

        mainCam.transform.rotation  = Quaternion.identity;
        leftCam.transform.rotation  = Quaternion.identity;
        rightCam.transform.rotation = Quaternion.identity;
        upCam.transform.rotation    = Quaternion.identity;
        downCam.transform.rotation  = Quaternion.identity;
        frontCam.transform.rotation = Quaternion.identity;
        backCam.transform.rotation  = Quaternion.identity;
        initialCam.transform.rotation = Quaternion.identity;

        RotateCamera(mainCam,    new Vector2(initialCameraX, initialCameraY));
        RotateCamera(initialCam, new Vector2(initialCameraX, initialCameraY));
        RotateCamera(rightCam,   new Vector2(0.5f, 0));
        RotateCamera(leftCam,   new Vector2(-0.5f, 0));
        RotateCamera(upCam,     new Vector2(0, 0.5f));
        RotateCamera(downCam,   new Vector2(0, -0.5f));
        RotateCamera(frontCam,  new Vector2(0, 0));
        RotateCamera(backCam,   new Vector2(1, 0));
        mainCam.transform.LookAt(PocketCube.transform.position);
        mainCam.orthographic = true;
        currentUsedTime = 0;

        currentCameraX = initialCameraX;
        currentCameraY = initialCameraY;

    }

    //private Vector3 getClosestUpAxis(List<Vector3> candidateUpAxis)
    //{
    //    Vector3 cameraUp = mainCam.transform.up;

    //    float minAngle = Mathf.Infinity;
    //    Vector3 targetUpAxis = Vector3.zero;
    //    foreach (Vector3 axis in candidateUpAxis)
    //    {
    //        float angle = Vector3.Angle(cameraUp, axis);
    //        if (angle < minAngle)
    //        {
    //            targetUpAxis = axis;
    //        }
    //    }

    //    // also find the minAngle

    //    return targetUpAxis;

    //}

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

        currentCameraX += direction.x;
        currentCameraY += direction.y;

        if (currentCameraX >1) {
            currentCameraX -= 2 ;
        }
        else if (currentCameraX <-1){
            currentCameraX += 2;
        }


        if (currentCameraY >1) {
            currentCameraY -= 2;
        }
        else if(currentCameraY <-1)
        {
            currentCameraY += 2;
        }
    }

    public void InitCameraResetTranslation()
    {

        currentUsedTime = 0;
        startPosition = mainCam.transform.position;
        startRotation = mainCam.transform.rotation;
        currentRotationDegree = 0;


        Vector2 currentPos = new Vector2(currentCameraX, currentCameraY);
        Vector2 initPos = new Vector2(initialCameraX, initialCameraY);
        resetCameraTime = Mathf.Clamp( 0.5f * (Vector2.Distance(currentPos, initPos))/0.3f, 0, 0.7f);


    }

    public bool ResetCamera()
    {
        return ResetCameraTo(initialCam);
    }

    public void SetTargetCameraToRight()
    {
        InitTargetRotation(rightCam);
    }

    public void SetTargetCameraToLeft()
    {
        InitTargetRotation(leftCam);
    }


    public void SetTargetCameraToFront()
    {
        InitTargetRotation(frontCam);
    }

    public void SetTargetCameraToBack()
    {
        InitTargetRotation(backCam);
    }

    public void SetTargetCameraToUp()
    {
        InitTargetRotation(upCam);
    }

    public void SetTargetCameraToDown()
    {
        InitTargetRotation(downCam);
    }


    private void CubeSolved()
    {
        // deactivate other bottons
        // display finish button 
    }

    private bool ResetCameraTo(Camera targetCamera)
    {
        
        currentUsedTime += Time.deltaTime;
        float t = currentUsedTime / resetCameraTime;

        float targetCameraX = Mathf.Lerp(currentCameraX, initialCameraX, resetCameraCurve.Evaluate(t));
        float targetCameraY = Mathf.Lerp(currentCameraY, initialCameraY, resetCameraCurve.Evaluate(t));

        float deltaCameraX  = targetCameraX - currentCameraX;
        float deltaCameraY = targetCameraY - currentCameraY;

        mainCam.transform.position = Vector3.zero;
        mainCam.transform.Rotate(new Vector3(1, 0, 0), deltaCameraY * 180);
        mainCam.transform.Rotate(new Vector3(0, 1, 0), -deltaCameraX * 180, Space.World);
        mainCam.transform.Translate(initialCameraPosition);

        currentCameraX = targetCameraX;
        currentCameraY = targetCameraY;

        //Debug.Log("camera direction x= " + currentCameraX + ", y = " + currentCameraY);

        //mainCam.transform.position = Vector3.Lerp(startPosition, targetCamera.transform.position, resetCameraCurve.Evaluate(t));
        //mainCam.transform.LookAt(PocketCube.transform, Vector3.up);

        return t >= 1;
    }

    public void InitTargetRotation(Camera targetCamera)
    {
        currentUsedTime = 0;
        currentRotationDegree = 0;

        startPosition = mainCam.transform.position;

        Vector3 cubeToMainCameraVec   = (PocketCube.transform.position - mainCam.transform.position).normalized;
        Vector3 cubeToTargetCameraVec = (PocketCube.transform.position - targetCamera.transform.position).normalized;

        translationRotateAxis   = Vector3.Cross(cubeToMainCameraVec, cubeToTargetCameraVec).normalized;
        translationRotateDegree = Vector3.Angle(cubeToMainCameraVec, cubeToTargetCameraVec);

        translateHelper.transform.position = mainCam.transform.position;
        translateHelper.transform.rotation = mainCam.transform.rotation;

        //translateHelper.transform.RotateAround(PocketCube.transform.position, translationRotateAxis, translationRotateDegree);
        //float selfRotationDegree = 0;
        //List<Vector3> candidateAxes = new List<Vector3>();
        //candidateAxes.Add(PocketCube.transform.up);
        //candidateAxes.Add(-PocketCube.transform.up);
        //candidateAxes.Add(PocketCube.transform.right);
        //candidateAxes.Add(-PocketCube.transform.right);
        //candidateAxes.Add(PocketCube.transform.forward);
        //candidateAxes.Add(-PocketCube.transform.forward);

        //float minAngle = Mathf.Infinity;
        //foreach (Vector3 axis in candidateAxes)
        //{
        //    float angleDiff = Vector3.Angle(mainCam.transform.up, axis);
        //    //print("angle Diff " + angleDiff);
        //    if (angleDiff < minAngle)
        //    {
        //        minAngle = angleDiff;
        //    }
        //}
        //selfRotationDegree = minAngle;

        //Vector3 selfRotationAxis = translateHelper.transform.position - PocketCube.transform.position;
        //translateHelper.transform.RotateAround(translateHelper.transform.position, selfRotationAxis, selfRotationDegree);

    }

    public IEnumerator CameraTranslate()
    {

        float t = 0;
        currentUsedTime = 0;
        currentRotationDegree = 0;
       // translationTimePhase1 *= translationRotateDegree / 90;

        while (t < 1)
        {
            currentUsedTime += Time.deltaTime;
            t = currentUsedTime / translationTimePhase1;

            float angle = Mathf.Lerp(0, translationRotateDegree, translationCurve.Evaluate(t));
            float deltaAngle = angle - currentRotationDegree;
            mainCam.transform.RotateAround(PocketCube.transform.position, translationRotateAxis, deltaAngle);
            currentRotationDegree = angle;

            //float selfRotationAngle = Mathf.Lerp(0, selfRotationDegree, translationCurve.Evaluate(t));
            //float deltaSelfRotationAngle = selfRotationDegree - currentSelfRotationAngle;
            //mainCam.transform.RotateAround(mainCam.transform.position, mainCam.transform.forward, deltaSelfRotationAngle);
            //currentSelfRotationAngle = selfRotationAngle;

            yield return null;
        }        

    }

    public void InitTargetRotationBack()
    {
        currentUsedTime = 0;
        currentRotationDegree = 0;

        Vector3 cubeToMainCameraVec = (PocketCube.transform.position - mainCam.transform.position).normalized;
        Vector3 cubeToTargetCameraVec = (PocketCube.transform.position - startPosition).normalized;

        translationRotateAxis = Vector3.Cross(cubeToMainCameraVec, cubeToTargetCameraVec).normalized;
        translationRotateDegree = Vector3.Angle(cubeToMainCameraVec, cubeToTargetCameraVec);
    }

    public IEnumerator CameraTranslateBack()
    {
        float t = 0;

        //translateBackHelper.transform.position = mainCam.transform.position;
        //translateBackHelper.transform.rotation = mainCam.transform.rotation;

        while (t < 1)
        {
            currentUsedTime += Time.deltaTime;
            t = currentUsedTime / translationTimePhase1;

            float angle = Mathf.Lerp(0, translationRotateDegree, translationCurve.Evaluate(t));

            float deltaAngle = angle - currentRotationDegree;
            mainCam.transform.RotateAround(PocketCube.transform.position, translationRotateAxis, deltaAngle);
            currentRotationDegree = angle;

            yield return null;
        }
    }


    public Vector3 ScreenToViewportPoint(Vector3 position)
    {
        return mainCam.ScreenToViewportPoint(position);
    }

}
