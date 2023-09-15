using System;
using System.Collections;
using System.Collections.Generic;
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

    [SerializeField] private AnimationCurve cubeTranslationCurve;
    [SerializeField][Range(0, 1)] private float cubeTranslationTime;

    [SerializeField][Range(0, 100)] private float rotateSpeed;



    bool translateBackFinish;
    public override void onStart()
    {
        cubeState = FindObjectOfType<CubeState>();
        myCameraController = FindObjectOfType<CubePlayCameraController>();
        myVFXManager = FindObjectOfType<CubeVFXManager>();
        translateBackFinish = false;
    }

    private IEnumerator  playFinishAnimation()
    {
        // find where the front face is
        GameObject pocketCube = CubePlayManager.instance.pocketCube;


        Vector3 targetForward = -pocketCube.transform.forward;
        Vector3 currentFrontFaceForward = cubeState.getFrontFaceDirection();

        Vector3 RotationAxis = Vector3.zero;
        float RotationDegree = 0;

        if (currentFrontFaceForward == Vector3.up)
        {
            //pocketCube.transform.Rotate(Vector3.right, -90);
            RotationAxis = Vector3.right;
            RotationDegree = -90;
        }
        else if (currentFrontFaceForward == -Vector3.up)
        {
            //pocketCube.transform.Rotate(Vector3.right, 90);
            RotationAxis = Vector3.right;
            RotationDegree = 90;
        }
        else if (currentFrontFaceForward == Vector3.forward)
        {
            //pocketCube.transform.Rotate(Vector3.up, 180);
            RotationAxis = Vector3.up;
            RotationDegree = 180;
        }
        else if (currentFrontFaceForward == Vector3.right)
        {
            //pocketCube.transform.Rotate(Vector3.up, 90);

            RotationAxis = Vector3.up;
            RotationDegree = 90;
        }
        else if (currentFrontFaceForward == -Vector3.right)
        {
            //pocketCube.transform.Rotate(Vector3.up, -90);

            RotationAxis = Vector3.up;
            RotationDegree = -90;
        }


        float t = 0;
        float currentUsedTime = 0;
        float currentRotationDegree = 0;
        //float translationTime = 1;

        while (t < 1)
        {
            currentUsedTime += Time.deltaTime;
            t = currentUsedTime / cubeTranslationTime;

            float angle = Mathf.Lerp(0, RotationDegree, cubeTranslationCurve.Evaluate(t));
            float deltaAngle = angle - currentRotationDegree;
            pocketCube.transform.RotateAround(pocketCube.transform.position, RotationAxis, deltaAngle);
            currentRotationDegree = angle;

            yield return null;
        }


        // camera translate back to initial Camera
        myCameraController.InitCameraResetTranslation();

        while (!myCameraController.ResetCamera())
        {
            yield return null;
        }

        translateBackFinish = true;

        yield return null;
    }

    // return true if is finished
    public override bool onUpdate()
    {
        if (currentState == CubeSolveState.PlayAnimation)
        {
            
            translateBackFinish = false;
            StartCoroutine(playFinishAnimation());
            currentState = CubeSolveState.WaitForQuit;
        }
        else if (currentState == CubeSolveState.WaitForQuit)
        {
            // some animation....
            if (translateBackFinish)
            {
                GameObject pocketCube = CubePlayManager.instance.pocketCube;
                pocketCube.transform.RotateAround(pocketCube.transform.position, Vector3.up, Time.deltaTime*rotateSpeed);
                myVFXManager.PlayFinishVFX();
            }
            // wait for user input

        }
        return true;
    }

    public override void onEnd()
    {

    }


}