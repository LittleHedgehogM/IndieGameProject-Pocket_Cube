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

    // Start is called before the first frame update
    void Start()
    {
        cubeState = FindObjectOfType<CubeState>();
        myCameraController = FindObjectOfType<CubePlayCameraController>();
        ResetValues();
    }

    void ResetValues()
    {
        currentState = SkillState.WaitForInput;
        commomFaceNormalAxis = Vector3.zero;
        FirstFaceHit = null;
        SecondFaceHit = null;
        FirstCubeHit = null;
        SecondCubeHit = null;
    }

    public bool InitSkill()
    {
        currentState = SkillState.WaitForSelectFirstCube;
        return true;
    }

    // Update is called once per frame
    public void UpdateSkill()
    {
        if (currentState == SkillState.WaitForSelectFirstCube)
        {

            if (Input.GetMouseButtonUp(0))
            {
                GameObject faceHit = SelectFace.GetMouseRayHitFace(Input.mousePosition);

                if (faceHit != null) 
                {
                    FirstFaceHit = faceHit;               
                    FirstCubeHit = SelectFace.getFaceRelatedCube(faceHit);
                    commomFaceNormalAxis = FindFaceNormal(FirstFaceHit);
                    
                    myCameraController.initTranslation();
                    currentState = SkillState.TranslateCamera;
                }

            }
        }
        else if (currentState == SkillState.TranslateCamera)
        {
            bool translateFinish = false;
            if (commomFaceNormalAxis == transform.right)
            {
                translateFinish = myCameraController.TranslateCameraToRight();
            }
            else if (commomFaceNormalAxis == -transform.right)
            {
                translateFinish = myCameraController.TranslateCameraToLeft();

            }
            else if (commomFaceNormalAxis == transform.up)
            {
                translateFinish = myCameraController.TranslateCameraToUp();

            }
            else if (commomFaceNormalAxis == -transform.up)
            {
                translateFinish = myCameraController.TranslateCameraToDown();

            }
            else if (commomFaceNormalAxis == transform.forward)
            {
                translateFinish = myCameraController.TranslateCameraToBack();

            }
            else if (commomFaceNormalAxis == -transform.forward)
            {
                translateFinish = myCameraController.TranslateCameraToFront();

            }

            if (translateFinish)
            {
                currentState = SkillState.WaitForSelectSecondCube;
            }
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
                myCameraController.initTranslationBack();
                currentState = SkillState.TranslateCameraBack;
            }
            
        }
        else if (currentState == SkillState.TranslateCameraBack)
        {
            bool translationBackFinish = myCameraController.TranslateCameraBack();
            if (translationBackFinish)
            {
                currentState = SkillState.SkillFinish;
            }
        }
        else if (currentState == SkillState.SkillFinish)
        {
            ResetValues();
            InvokeFinish();

        }
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
