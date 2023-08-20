//using System;
//using System.Collections;
//using System.Collections.Generic;
//using System.Diagnostics;
//using TMPro.Examples;
//using UnityEngine;

//public class CommutationManager : MonoBehaviour
//{

//    /*
//     Current Design :
//     Press Keyboard C to enter Commutate 
//     use mouse to select the first and second cube
//     then the commutation starts
//     */

//    enum CommutationState
//    {
//        WaitForCommutationInput,
//        WaitForSelectFirstCube,
//        TranslateCamera,
//        WaitForSelectSecondCube,
//        CommutationInProgress,
//        TranslateCameraBack,
//        CommutationFinish,
//        CancelCommutationEffect
//    }

//    CommutationState currentState;
//    GameObject FirstFaceHit;
//    GameObject SecondFaceHit;
//    GameObject FirstCubeHit;
//    GameObject SecondCubeHit;
//    Vector3 commomFaceNormalAxis;
//    //SelectFace selectFace;
//    CubeState cubeState;
//    int selectCount;
//    int commutationCount = 0;
//    CubePlayCameraController myCameraController;

//    public static event Action onCommutationFinished;

//    // Start is called before the first frame update
//    void Start()
//    {
//        cubeState = FindObjectOfType<CubeState>();
//        myCameraController = FindObjectOfType<CubePlayCameraController>();
//        ResetValues();
//    }

//    private void Awake()
//    {
            
//    }

//    void ResetValues()
//    {
//        selectCount = 0;
//        currentState = CommutationState.WaitForCommutationInput;
//        commomFaceNormalAxis = Vector3.zero;
//        FirstFaceHit = null;
//        SecondFaceHit = null;
//        FirstCubeHit = null;
//        SecondCubeHit = null;
//    }

//    bool isAdjacentCube(GameObject firstCube, GameObject secondCube) 
//    {
        
//        return Mathf.Abs(Vector3.Distance(firstCube.transform.position, secondCube.transform.position) - 1) < 0.001f; ; 
//    }


//    public bool InitCommutation()
//    {
//        currentState = CommutationState.WaitForSelectFirstCube;
//        return true;
//    }

//    public void UpdateCommutation()
//    {
//        if (currentState == CommutationState.WaitForSelectFirstCube)
//        {

//            if (Input.GetMouseButtonUp(0))
//            {
//                GameObject faceHit = SelectFace.GetMouseRayHitFace(Input.mousePosition);

//                if (faceHit != null) // ray Hit Any Cube Piece
//                {
//                    FirstFaceHit = faceHit;
//                    print(commutationCount + "First Cube Selected");
//                    FirstCubeHit = SelectFace.getFaceRelatedCube(faceHit);
//                    commomFaceNormalAxis = FindFaceNormal(FirstFaceHit);
//                    myCameraController.initTranslation();

//                    currentState = CommutationState.TranslateCamera;
//                }

//            }
//        }
//        else if (currentState == CommutationState.TranslateCamera)
//        {
//            bool translateFinish = false;
//            if (commomFaceNormalAxis == transform.right)
//            {
//                translateFinish = myCameraController.TranslateCameraToRight();
//            }
//            else if (commomFaceNormalAxis == -transform.right)
//            {
//                translateFinish = myCameraController.TranslateCameraToLeft();

//            }
//            else if (commomFaceNormalAxis == transform.up)
//            {
//                translateFinish = myCameraController.TranslateCameraToUp();

//            }
//            else if (commomFaceNormalAxis == -transform.up)
//            {
//                translateFinish = myCameraController.TranslateCameraToDown();

//            }
//            else if (commomFaceNormalAxis == transform.forward)
//            {
//                translateFinish = myCameraController.TranslateCameraToBack();

//            }
//            else if (commomFaceNormalAxis == -transform.forward)
//            {
//                translateFinish = myCameraController.TranslateCameraToFront();

//            }


//            //bool translateFinish = myCameraController.TranslateCameraToRight();
//            if (translateFinish)
//            {
//                currentState = CommutationState.WaitForSelectSecondCube;
//            }
            
            
//        }
//        else if (currentState == CommutationState.WaitForSelectSecondCube)
//        {
//            if (Input.GetMouseButtonUp(0))
//            {
//                GameObject faceHit = SelectFace.GetMouseRayHitFace(Input.mousePosition);

//                if (faceHit != null) // ray Hit Any Cube Piece
//                {
//                    GameObject secondCandidateCube = SelectFace.getFaceRelatedCube(faceHit);

//                    print(commutationCount + ". Distance = " + Vector3.Distance(FirstCubeHit.transform.position,
//                                           secondCandidateCube.transform.position) + ", side length = " + FirstCubeHit.transform.localScale.sqrMagnitude);

//                    selectCount++;


//                    bool isSameFace = getCubeSide(FirstFaceHit) == getCubeSide(faceHit);

//                    if (!isSameFace)
//                    {
//                        print(commutationCount + "select Count= " + selectCount + "Not on the same face");
//                    }
//                    if (!isAdjacentCube(FirstCubeHit, secondCandidateCube))
//                    {
//                        print(commutationCount + "select Count= " + selectCount + "Not Adjacent");
//                    }


//                    if (isAdjacentCube(FirstCubeHit, secondCandidateCube) && isSameFace)
//                    {
//                        SecondFaceHit = faceHit;
//                        SecondCubeHit = secondCandidateCube;
//                        currentState = CommutationState.CommutationInProgress;
//                    }
//                    else
//                    {
//                        print(commutationCount + "select Count= " + selectCount + "please select another one");
//                    }

//                }

//            }



//        }
//        else if (currentState == CommutationState.CommutationInProgress)
//        {
//            // start Commutation Animation
//            /* 
//             * 1. switch position (switch parents)
//             * 2. rotate 90 degrees
//             */

//            // get parent

//            print("start commutating ..." + FirstCubeHit.name + ", " + SecondCubeHit.name);
//            Vector3 FirstDestinationVec = SecondCubeHit.transform.position - FirstCubeHit.transform.position;

//            // get face normal of First and Second Cube
//            commomFaceNormalAxis = FindFaceNormal(FirstFaceHit);
//            Vector3 rotationalAxis = Vector3.Cross(FirstDestinationVec.normalized, commomFaceNormalAxis);
//            FirstCubeHit.transform.RotateAround(Vector3.zero, rotationalAxis, -90);
//            SecondCubeHit.transform.RotateAround(Vector3.zero, rotationalAxis, 90);

//            myCameraController.initTranslationBack();
//            currentState = CommutationState.TranslateCameraBack;

//            // update CubeMap
//            // readCube.ReadState();
//        }
//        else if (currentState == CommutationState.TranslateCameraBack)
//        {
//            // translate
//            bool translationBackFinish = myCameraController.TranslateCameraBack();
//            if (translationBackFinish)
//            {
//                currentState = CommutationState.CommutationFinish;
//            }
//        }
//        else if (currentState == CommutationState.CommutationFinish)
//        {
//            //restart 
//            print("end commutating");
//            ResetValues();
//            commutationCount++;
//            onCommutationFinished?.Invoke();
//            //myCubePlayManager.SetCubePlayStatus(CubePlayManager.CubePlayStatus.WaitForInput);
//            //currentState = CommutationState.WaitForCommutationInput;

//        }
//    }

//    List<GameObject> getCubeSide(GameObject FirstCubeFace)
//    {
//        List<GameObject> cubeSide = new List<GameObject>();
        
//        List<List<GameObject>> cubeSides = cubeState.GetAllCubeSidesFaces();
//        foreach (List<GameObject> side in cubeSides)
//        {
//            if (side.Contains(FirstCubeFace))
//            {
//                cubeSide = side;
//                break;
//            }
//        }

//        return cubeSide;
                      
//    }

//    Vector3 FindFaceNormal(GameObject FirstCubeFace)
//    {
//        Vector3 faceNormalAxis = Vector3.zero;

//        List<List<GameObject>> cubeSides = cubeState.GetAllCubeSidesFaces();
//        foreach (List<GameObject> cubeSide in cubeSides)
//        { 
//            if (cubeSide.Contains(FirstCubeFace))
//            {
//                if (cubeSide == cubeState.front)
//                {
//                    faceNormalAxis = -transform.forward;
//                }
//                else if (cubeSide == cubeState.back)
//                {
//                    faceNormalAxis = transform.forward;
//                }
//                else if (cubeSide == cubeState.up)
//                {
//                    faceNormalAxis = transform.up;
//                }
//                else if (cubeSide == cubeState.down)
//                {
//                    faceNormalAxis = -transform.up;
//                }
//                else if (cubeSide == cubeState.left)
//                {
//                    faceNormalAxis = -transform.right;
//                }
//                else if (cubeSide == cubeState.right)
//                {
//                    faceNormalAxis = transform.right;
//                }
//                break;
//            }
//            //print("face normal = " + faceNormalAxis);

//        }
//        return faceNormalAxis;
//    }

//}
