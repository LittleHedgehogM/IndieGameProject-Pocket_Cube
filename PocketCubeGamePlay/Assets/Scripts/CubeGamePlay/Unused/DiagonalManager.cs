//using System;
//using System.Collections.Generic;
//using UnityEngine;

//public class DiagonalManager : MonoBehaviour
//{
//    /*
//     Current Design :
//     Press Keyboard D to enter Diagonal 
//     use mouse to select the first and second cube
//     then the Diagonal starts
//     */

//    enum DiagonalState
//    {
//        WaitForDiagonalInput,
//        TranslateCamera,
//        WaitForSelectFirstCube,
//        WaitForSelectSecondCube,
//        DiagonalInProgress,
//        TranslateCameraBack,
//        DiagonalFinish,
//        CancelDiagonalEffect
//    }

//    DiagonalState currentState;
//    GameObject FirstFaceHit;
//    GameObject SecondFaceHit;
//    GameObject FirstCubeHit;
//    GameObject SecondCubeHit;
//    Vector3 commomFaceNormalAxis;
//    CubeState cubeState;

//    public static event Action onDiagonalFinished;
//    CubePlayCameraController myCameraController;

//    // Start is called before the first frame update
//    void Start()
//    {
//        //selectFace = FindObjectOfType<SelectFace>();
//        cubeState = FindObjectOfType<CubeState>();
//        //readCube = FindObjectOfType<ReadCube>();
//        //myCubePlayManager = FindObjectOfType<CubePlayManager>();
//        myCameraController = FindObjectOfType<CubePlayCameraController>();
//        ResetValues();
//    }


//    private void Awake()
//    {

//    }

//    void ResetValues()
//    {
//        //selectCount = 0;
//        currentState = DiagonalState.WaitForDiagonalInput;
//        commomFaceNormalAxis = Vector3.zero;
//        FirstFaceHit = null;
//        SecondFaceHit = null;
//        FirstCubeHit = null;
//        SecondCubeHit = null;

//    }

//    bool isDiagonalCube(GameObject firstCube, GameObject secondCube)
//    {
//        return Mathf.Abs(Vector3.Distance(firstCube.transform.position, secondCube.transform.position) - Mathf.Sqrt(2) ) < 0.1f;
//    }


//    public bool InitDiagonal()
//    {
//        currentState = DiagonalState.WaitForSelectFirstCube;
//        return true;
//    }

//    public void UpdateDiagonal()
//    {
//        if (currentState == DiagonalState.WaitForSelectFirstCube)
//        {

//            if (Input.GetMouseButtonUp(0))
//            {
//                GameObject faceHit = SelectFace.GetMouseRayHitFace(Input.mousePosition);

//                if (faceHit != null) // ray Hit Any Cube Piece
//                {
//                    FirstFaceHit = faceHit;
//                    //print(DiagonalCount + "First Cube Selected");
//                    FirstCubeHit = SelectFace.getFaceRelatedCube(faceHit);

//                    commomFaceNormalAxis = FindFaceNormal(FirstFaceHit);
//                    myCameraController.initTranslation();
//                    currentState = DiagonalState.TranslateCamera;
//                }

//            }

//        }
//        else if (currentState == DiagonalState.TranslateCamera)
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
//            if (translateFinish)
//            {
//                currentState = DiagonalState.WaitForSelectSecondCube;
//            }
//            //bool translateFinish = myCameraController.TranslateCameraToRight();
//            if (translateFinish)
//            {
//                currentState = DiagonalState.WaitForSelectSecondCube;
//            }
//        }
//        else if (currentState == DiagonalState.WaitForSelectSecondCube)
//        {
//            if (Input.GetMouseButtonUp(0))
//            {
//                GameObject faceHit = SelectFace.GetMouseRayHitFace(Input.mousePosition);

//                if (faceHit != null) // ray Hit Any Cube Piece
//                {
//                    GameObject secondCandidateCube = SelectFace.getFaceRelatedCube(faceHit);

//                    //print("Distance = " + Vector3.Distance(FirstCubeHit.transform.position,
//                    //                       secondCandidateCube.transform.position) + ", side length = " + FirstCubeHit.transform.localScale.sqrMagnitude);

//                    //selectCount++;

//                    //if (!isDiagonalCube(FirstCubeHit, secondCandidateCube))
//                    //{
//                    //    print( "select Count= " + selectCount + "Not Diagonal");
//                    //}


//                    if (isDiagonalCube(FirstCubeHit, secondCandidateCube) /*&& isSameFace*/)
//                    {
//                        SecondFaceHit = faceHit;
//                        SecondCubeHit = secondCandidateCube;
//                        currentState = DiagonalState.DiagonalInProgress;
//                    }
//                    //else
//                    //{
//                    //    print( "select Count= " + selectCount + "please select another one");
//                    //}

//                }
//            }
//        }
//        else if (currentState == DiagonalState.DiagonalInProgress)
//        {
//            FirstCubeHit.transform.RotateAround(Vector3.zero, commomFaceNormalAxis, 180);
//            SecondCubeHit.transform.RotateAround(Vector3.zero, commomFaceNormalAxis, 180);
//            myCameraController.initTranslationBack();
//            currentState = DiagonalState.TranslateCameraBack;

//        }
//        else if (currentState == DiagonalState.TranslateCameraBack)
//        {
//            bool translationBackFinish = myCameraController.TranslateCameraBack();
//            if (translationBackFinish)
//            {
//                currentState = DiagonalState.DiagonalFinish;
//            }
//        }
//        else if (currentState == DiagonalState.DiagonalFinish)
//        {
//            ResetValues();
//            onDiagonalFinished?.Invoke();

//        }
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

//    //Vector3 FindFaceNormal(GameObject FirstCube, GameObject SecondCube)
//    //{
//    //    Vector3 faceNormalAxis = Vector3.zero;
//    //    List<List<GameObject>> cubeSides = cubeState.GetAllCubeSidesFaces();

//    //    foreach (List<GameObject> cubeSide in cubeSides)
//    //    {
//    //        bool containsFirstCube = false;
//    //        bool containsSecondCube = false;
//    //        foreach (GameObject side in cubeSide)
//    //        {
//    //            GameObject cube = SelectFace.getFaceRelatedCube(side);
//    //            if (cube == FirstCube)
//    //            {
//    //                containsFirstCube = true;
//    //            }
//    //            if (cube == SecondCube)
//    //            {
//    //                containsSecondCube = true;
//    //            }
//    //        }
//    //        if (containsFirstCube && containsSecondCube)
//    //        {
//    //            if (cubeSide == cubeState.front)
//    //            {
//    //                print("front");
//    //                faceNormalAxis = -transform.forward;
//    //            }
//    //            else if (cubeSide == cubeState.back)
//    //            {
//    //                print("back");
//    //                faceNormalAxis = transform.forward;
//    //            }
//    //            else if (cubeSide == cubeState.up)
//    //            {
//    //                print("up");
//    //                faceNormalAxis = transform.up;
//    //            }
//    //            else if (cubeSide == cubeState.down)
//    //            {
//    //                print("down");
//    //                faceNormalAxis = -transform.up;
//    //            }
//    //            else if (cubeSide == cubeState.left)
//    //            {
//    //                print("left");
//    //                faceNormalAxis = -transform.right;
//    //            }
//    //            else if (cubeSide == cubeState.right)
//    //            {
//    //                print("right");
//    //                faceNormalAxis = transform.right;
//    //            }
//    //            break;
//    //        }


//    //    }

//    //    print("face normal = " + faceNormalAxis);

//    //    return faceNormalAxis;
//    //}
//}
