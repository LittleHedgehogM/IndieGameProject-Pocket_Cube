using System;
using System.Collections;
using System.Collections.Generic;
using TMPro.Examples;
using UnityEngine;

// a common parent class for both commutation and diagonal
public abstract class SkillManager : MonoBehaviour
{
    protected enum SkillState
    {
        WaitForInput,
        WaitForSelectFirstCube,
        TranslateCamera,
        WaitForSelectSecondCube,
        ApplySkillInProgress,
        TranslateCameraBack,
        SkillFinish
    }

    protected SkillState currentState;

    protected GameObject FirstFaceHit;
    protected GameObject SecondFaceHit;
    protected GameObject FirstCubeHit;
    protected GameObject SecondCubeHit;
    protected Vector3 commomFaceNormalAxis;
    protected CubeState cubeState;
    protected CubePlayCameraController myCameraController;
    RotateWholeCubeManager myRotateWholeCubeManager;

    // Start is called before the first frame update
    void Start()
    {
        cubeState = FindObjectOfType<CubeState>();
        myCameraController = FindObjectOfType<CubePlayCameraController>();
        myRotateWholeCubeManager = FindObjectOfType<RotateWholeCubeManager>();
        ResetValues();
    }

    public void ResetValues()
    {
        currentState = SkillState.WaitForSelectFirstCube;
        commomFaceNormalAxis = Vector3.zero;
        FirstFaceHit = null;
        SecondFaceHit = null;
        FirstCubeHit = null;
        SecondCubeHit = null;
    }

    public bool onStart()
    {
        ResetValues();
        return true;
    }

    // Update is called once per frame
    public void onUpdate()
    {
        if (currentState == SkillState.WaitForSelectFirstCube)
        {
            if (Input.GetMouseButtonDown(1))
            {
                myRotateWholeCubeManager.InitPosition();
            }
            if (Input.GetMouseButton(1)&& !Input.GetMouseButton(0))
            {
                // enable camera rotation
                myRotateWholeCubeManager.RotateWholeCubeForSkill();

            }

            if (!Input.GetMouseButton(1)&&Input.GetMouseButtonUp(0))
            {
                GameObject faceHit = SelectFace.GetMouseRayHitFace(Input.mousePosition);

                if (faceHit != null) 
                {
                    FirstFaceHit = faceHit;               
                    FirstCubeHit = SelectFace.getFaceRelatedCube(faceHit);
                    commomFaceNormalAxis = FindFaceNormal(FirstFaceHit);
                    
                    if (commomFaceNormalAxis == transform.right)
                    {
                        myCameraController.SetTargetCameraToRight();
                    }
                    else if (commomFaceNormalAxis == -transform.right)
                    {
                        myCameraController.SetTargetCameraToLeft();
                    }
                    else if (commomFaceNormalAxis == transform.up)
                    {
                        myCameraController.SetTargetCameraToUp();
                    }
                    else if (commomFaceNormalAxis == -transform.up)
                    {
                        myCameraController.SetTargetCameraToDown();
                    }
                    else if (commomFaceNormalAxis == transform.forward)
                    {
                        myCameraController.SetTargetCameraToBack();
                    }
                    else if (commomFaceNormalAxis == -transform.forward)
                    {
                        myCameraController.SetTargetCameraToFront();
                    }
                    currentState = SkillState.TranslateCamera;
                }

            }
        }
        else if (currentState == SkillState.TranslateCamera)
        {

            StartCoroutine(myCameraController.CameraTranslate());
            currentState = SkillState.WaitForSelectSecondCube;
            
        }
        else if (currentState == SkillState.WaitForSelectSecondCube)
        {
            if (Input.GetMouseButtonUp(0))
            {
                GameObject faceHit = SelectFace.GetMouseRayHitFace(Input.mousePosition);

                if (faceHit != null) // ray Hit Any Cube Piece
                {
                    GameObject secondCandidateCube = SelectFace.getFaceRelatedCube(faceHit);

                    if (CubesAreValid(FirstCubeHit, secondCandidateCube))
                    {
                        SecondFaceHit = faceHit;
                        SecondCubeHit = secondCandidateCube;
                        currentState = SkillState.ApplySkillInProgress;
                    }

                }

            }
        }
        else if (currentState == SkillState.ApplySkillInProgress)
        {
            bool ApplySkillFinih = ApplySkill();
            if (ApplySkillFinih)
            {
                // myCameraController.initTranslationBack();
                myCameraController.InitTargetRotationBack();
                currentState = SkillState.TranslateCameraBack;
            }
            
        }
        else if (currentState == SkillState.TranslateCameraBack)
        {
            StartCoroutine(myCameraController.CameraTranslateBack());
            currentState = SkillState.SkillFinish;
        }
        else if (currentState == SkillState.SkillFinish)
        {
            currentState = SkillState.WaitForInput;
            InvokeFinish();

        }
    }

    protected virtual void onRestart()
    {
        ResetValues();
    }

    protected virtual void InvokeFinish()
    {

    }
    protected virtual bool CubesAreValid(GameObject FirstCube, GameObject SecondCube)
    {
        return false;
    }

    protected virtual bool ApplySkill()
    {
        return true;
    }


    protected Vector3 FindFaceNormal(GameObject FirstCubeFace)
    {
        Vector3 faceNormalAxis = Vector3.zero;
        

        List<List<GameObject>> cubeSides = cubeState.GetAllCubeSidesFaces();
        foreach (List<GameObject> cubeSide in cubeSides)
        {
            if (cubeSide.Contains(FirstCubeFace))
            {
                if (cubeSide == cubeState.front)
                {
                    faceNormalAxis = -transform.forward;
                }
                else if (cubeSide == cubeState.back)
                {
                    faceNormalAxis = transform.forward;
                }
                else if (cubeSide == cubeState.up)
                {
                    faceNormalAxis = transform.up;
                }
                else if (cubeSide == cubeState.down)
                {
                    faceNormalAxis = -transform.up;
                }
                else if (cubeSide == cubeState.left)
                {
                    faceNormalAxis = -transform.right;
                }
                else if (cubeSide == cubeState.right)
                {
                    faceNormalAxis = transform.right;
                }
                break;
            }

        }
        return faceNormalAxis;
    }

}
