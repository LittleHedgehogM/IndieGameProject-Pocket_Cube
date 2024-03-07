using System;
using System.Collections;
using System.Collections.Generic;
using TMPro.Examples;
using UnityEngine;


public class CubeSolvedPhase : GameplayPhase
{

    enum CubeSolveState
    {
        PlayAnimation,
        WaitForQuit
    }

    CubeState cubeState;

    CubeSolveState currentState;
    CubePlayCameraController myCameraController;
    CubeVFXManager myVFXManager;
    CubeCursorController myCursorController;
    [SerializeField] private AnimationCurve cubeTranslationCurve;
    [SerializeField][Range(0, 1)] private float cubeTranslationTime;

    [SerializeField][Range(0, 100)] private float rotateSpeed;
    [SerializeField][Range(0.5f, 1)] private float mouseSenstivity;

    private CubeIceController myCubeIceController;

    Vector3 previousMousePosition;
    public override void onStart()
    {
        cubeState = FindObjectOfType<CubeState>();
        myCameraController = FindObjectOfType<CubePlayCameraController>();
        myVFXManager = FindObjectOfType<CubeVFXManager>();
        //translateBackFinish = false;
        myCubeIceController = FindObjectOfType<CubeIceController>();
        myCursorController = FindObjectOfType<CubeCursorController>();
        myCursorController.setNormalCursor();

    }

    //private IEnumerator  playFinishAnimation()
    //{
    //    // find where the front face is
    //    //GameObject pocketCube = CubePlayManager.instance.pocketCube;


    //    //Vector3 targetForward = -pocketCube.transform.forward;
    //    //Vector3 currentFrontFaceForward = cubeState.getFrontFaceDirection();

    //    //Vector3 RotationAxis = Vector3.zero;
    //    //float RotationDegree = 0;

    //    //if (currentFrontFaceForward == Vector3.up)
    //    //{
    //    //    RotationAxis = Vector3.right;
    //    //    RotationDegree = -90;
    //    //}
    //    //else if (currentFrontFaceForward == -Vector3.up)
    //    //{
    //    //    RotationAxis = Vector3.right;
    //    //    RotationDegree = 90;
    //    //}
    //    //else if (currentFrontFaceForward == Vector3.forward)
    //    //{
    //    //    //pocketCube.transform.Rotate(Vector3.up, 180);
    //    //    RotationAxis = Vector3.up;
    //    //    RotationDegree = 180;
    //    //}
    //    //else if (currentFrontFaceForward == Vector3.right)
    //    //{
    //    //    //pocketCube.transform.Rotate(Vector3.up, 90);

    //    //    RotationAxis = Vector3.up;
    //    //    RotationDegree = 90;
    //    //}
    //    //else if (currentFrontFaceForward == -Vector3.right)
    //    //{
    //    //    //pocketCube.transform.Rotate(Vector3.up, -90);

    //    //    RotationAxis = Vector3.up;
    //    //    RotationDegree = -90;
    //    //}


    //    //float t = 0;
    //    //float currentUsedTime = 0;
    //    //float currentRotationDegree = 0;
    //    ////float translationTime = 1;

    //    //while (t < 1)
    //    //{
    //    //    currentUsedTime += Time.deltaTime;
    //    //    t = currentUsedTime / cubeTranslationTime;

    //    //    float angle = Mathf.Lerp(0, RotationDegree, cubeTranslationCurve.Evaluate(t));
    //    //    float deltaAngle = angle - currentRotationDegree;
    //    //    pocketCube.transform.RotateAround(pocketCube.transform.position, RotationAxis, deltaAngle);
    //    //    currentRotationDegree = angle;

    //    //    yield return null;
    //    //}


    //    //// camera translate back to initial Camera
    //    //myCameraController.InitCameraResetTranslation();

    //    //while (!myCameraController.ResetCamera())
    //    //{
    //    //    yield return null;
    //    //}


    //    //translateBackFinish = true;


    //    //float t = 0;
    //    //float currentUsedTime = 0;
    //    //float translationTime = 1;

    //    //Camera mainCam = myCameraController.getMainCam();
    //    //while (t < 1)
    //    //{
    //    //    currentUsedTime += Time.deltaTime;
    //    //    t = currentUsedTime / translationTime;

    //    //    mainCam.transform.position = Vector3.Lerp(mainCam.transform.position, initialCamWithCube.transform.position, t);
    //    //    //mainCam.transform.rotation = Quaternion.Lerp(mainCam.transform.rotation, initialCamWithCube.transform.rotation, t);
    //    //    mainCam.transform.LookAt(CubePlayManager.instance.pocketCube.transform);
    //    //    yield return null;
    //    //}
    //    translateBackFinish = true;
    //    //myCubeIceController.HideIce();
    //    yield return null;
    //}

    // return true if is finished
    public override bool onUpdate()
    {
        if (currentState == CubeSolveState.PlayAnimation)
        {            
            StartCoroutine(myCubeIceController.HideIceAnim());
            myVFXManager.PlayFinishVFX();
            currentState = CubeSolveState.WaitForQuit;
        }
        else if (currentState == CubeSolveState.WaitForQuit)
        {
            GameObject pocketCube = CubePlayManager.instance.pocketCube;
            Camera mainCam = myCameraController.getMainCam();

            if (Input.GetMouseButtonDown(1) && !Utils.isMouseOverUI())
            {
                previousMousePosition = myCameraController.ScreenToViewportPoint(Input.mousePosition);
            }
            else if (Input.GetMouseButton(1) && !Utils.isMouseOverUI())
            {
                myCursorController.setRotationCursor();
                Vector3 direction = previousMousePosition - myCameraController.ScreenToViewportPoint(Input.mousePosition);
                direction *= mouseSenstivity;
                myCameraController.RotateMainCamera(direction);
                previousMousePosition = myCameraController.ScreenToViewportPoint(Input.mousePosition); ;
            }
            else 
            {
                myCursorController.setNormalCursor();
                if (Input.GetMouseButton(0) && !Utils.isMouseOverUI())
                {
                    myCursorController.setSwipeCursor();
                }
                mainCam.transform.RotateAround(pocketCube.transform.position, mainCam.transform.up, Time.deltaTime * rotateSpeed);
                
            }

        }
        return true;
    }

    public override void onEnd()
    {

    }


}