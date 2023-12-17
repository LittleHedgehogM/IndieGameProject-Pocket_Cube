using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CubeInPlayPhase : GameplayPhase
{

    public enum CubePlayStatus
    {
        WaitForInput,
        InResetCamera,
        InRotation,
        InSwipe,
        InCommutation,
        InDiagonal,
        CubeSolved,
        InRestoreCheckPoint,
    };

    [SerializeField] private bool enableFinish;

    private CubePlayStatus currentPlayStatus;

    SwipeFaceManager mySwipeFaceManager;
    CommutationSkill myCommutationSkill;
    DiagonalSkill myDiagonalSkill;
    RotateWholeCubeManager myRotateWholeCubeManager;
    CubePlayUIController myUIController;
    CubePlayCameraController myCubePlayCameraController;
    CubeCursorController myCubeCursorController;
    CubeState cubeState;
    ReadCube readCube;

    Vector3 initalMousePressPos;
    Vector3 endMousePressPos;

    [SerializeField]
    [Range(0, 1)]
    float restoreAnimationTime;
    [SerializeField] private AnimationCurve animationCurve;
    bool restoreFinish;

    public static event Action onCubeSolved;

    public override void onStart()
    {
        mySwipeFaceManager = FindObjectOfType<SwipeFaceManager>();
        myCommutationSkill = FindObjectOfType<CommutationSkill>();
        myDiagonalSkill = FindObjectOfType<DiagonalSkill>();

        myRotateWholeCubeManager = FindObjectOfType<RotateWholeCubeManager>();
        cubeState = FindObjectOfType<CubeState>();
        readCube = FindObjectOfType<ReadCube>();
        myUIController = FindObjectOfType<CubePlayUIController>();
        myCubePlayCameraController = FindObjectOfType<CubePlayCameraController>();
        myCubeCursorController = FindObjectOfType<CubeCursorController>();
        myCubeCursorController.setNormalCursor();
        currentPlayStatus = CubePlayStatus.WaitForInput;
        myUIController.InitCubePlayUIElements();

        
    }


    private void OnEnable()
    {
        SwipeFaceManager.onSwipeFinished += onAnyCubeStateChanged;
        SwipeFaceManager.SwipeException += SwipeExceptionHandler;
        RotateWholeCubeManager.onRotateWholeCubeFinished += onRotationFinished;
        DiagonalSkill.onDiagonalFinished += onAnyCubeStateChanged;
        CommutationSkill.onCommutataionFinished += onAnyCubeStateChanged;

    }

    private void OnDisable()
    {
        SwipeFaceManager.onSwipeFinished -= onAnyCubeStateChanged;
        SwipeFaceManager.SwipeException -= SwipeExceptionHandler;
        RotateWholeCubeManager.onRotateWholeCubeFinished -= onRotationFinished;
        DiagonalSkill.onDiagonalFinished -= onAnyCubeStateChanged;
        CommutationSkill.onCommutataionFinished -= onAnyCubeStateChanged;
    }

    private void SwipeExceptionHandler()
    {
        if (currentPlayStatus == CubePlayStatus.InSwipe)
        {
            currentPlayStatus = CubePlayStatus.WaitForInput;
        }
    }

    private void onAnyCubeStateChanged()
    {
        readCube.ReadState();
        print("current state = " + cubeState.GetStateString());
        bool isCubeSolved = CheckCubeSolved();
        if (isCubeSolved && enableFinish)
        {
            if (enableFinish)
            {
                onCubeSolved?.Invoke();
                SetCubePlayStatus(CubePlayStatus.CubeSolved);
            }
        }
        else
        {
            SetCubePlayStatus(CubePlayStatus.WaitForInput);
        }

    }

    void onRotationFinished()
    {
        SetCubePlayStatus(CubePlayStatus.WaitForInput);
    }

    public CubePlayStatus GetCubePlayStatus()
    {
        return currentPlayStatus;
    }

    private void SetCubePlayStatus(CubePlayStatus newStatus)
    {
        currentPlayStatus = newStatus;
    }


    // check if cube is solved
    bool CheckCubeSolved()
    {
        bool isCubeSolved = cubeState.isCubeSolved();
        myUIController.SolveResult = isCubeSolved;
        return isCubeSolved;
    }


    public void SetStateToResetCamera()
    {
        if (currentPlayStatus == CubePlayStatus.WaitForInput)
        {
            myCubePlayCameraController.InitCameraResetTranslation();
            currentPlayStatus = CubePlayStatus.InResetCamera;
        }
    }

    public bool CanRestoreCommuation()
    {
        myCubeCursorController.setNormalCursor();
        return currentPlayStatus == CubePlayStatus.WaitForInput || currentPlayStatus == CubePlayStatus.InCommutation;
    }

    public bool CanRestoreDiagional()
    {
        myCubeCursorController.setNormalCursor();
        return currentPlayStatus == CubePlayStatus.WaitForInput || currentPlayStatus == CubePlayStatus.InDiagonal;
    }

    public void RestoreCommutationCheckPoint()
    {
        if (CanRestoreCommuation())
        {
            CubePlayCheckPoint.instance.loadCurrentStateCommutation();
            myCubePlayCameraController.instantResetCam();
            restoreFinish = false;
            StartCoroutine(startRestoreAnimation());
            currentPlayStatus = CubePlayStatus.InRestoreCheckPoint;
            myCommutationSkill.EraseOutline();
            // recover audio
            AkSoundEngine.PostEvent("Play_cube_ani", gameObject);
        }

    }

    public void RestoreDiagonalCheckPoint()
    {
        if (CanRestoreDiagional())
        {
            CubePlayCheckPoint.instance.loadCurrentStateDiagonal();
            myCubePlayCameraController.instantResetCam();
            StartCoroutine(startRestoreAnimation());
            currentPlayStatus = CubePlayStatus.InRestoreCheckPoint;
            restoreFinish = false;
            myDiagonalSkill.EraseOutline();
            // recover audio
            AkSoundEngine.PostEvent("Play_cube_ani", gameObject);
        }
    }


    public void SetStateToCommutation()
    {
        if (currentPlayStatus == CubePlayStatus.WaitForInput)
        {
            // play animation
            if (myCommutationSkill.onStart())
            {
                CubePlayCheckPoint.instance.saveCurrentStateCommutation();
                currentPlayStatus = CubePlayStatus.InCommutation;
            }
        }
    }

    public void SetStateToDiagonal()
    {
        if (currentPlayStatus == CubePlayStatus.WaitForInput)
        {
            if (myDiagonalSkill.onStart())
            {
                CubePlayCheckPoint.instance.saveCurrentStateDiagonal();
                currentPlayStatus = CubePlayStatus.InDiagonal;
            }
        }
    }


    public override void onRestart()
    {
        base.onRestart();
        currentPlayStatus = CubePlayStatus.WaitForInput;
        restoreFinish = false;
        myUIController.onRestart();
        myDiagonalSkill.ResetValues();
        myCommutationSkill.ResetValues();
        myCubePlayCameraController.onRestart();
        myCubeCursorController.setNormalCursor();

    }

    public void SetStateTo(CubePlayStatus status)
    {
        currentPlayStatus = status;
    }

    // Update is called once per frame
    public override bool onUpdate()
    {
        if (currentPlayStatus == CubePlayStatus.CubeSolved)
        {
            
            return true;
        }
        else if (currentPlayStatus == CubePlayStatus.InResetCamera)
        {
            bool resetFinish = myCubePlayCameraController.ResetCamera();
            if (resetFinish)
            {
                currentPlayStatus = CubePlayStatus.WaitForInput;
            }
        }
        else if (currentPlayStatus == CubePlayStatus.WaitForInput)
        {
            myCubeCursorController.setNormalCursor();
            readCube.ReadState();
            // priority : Commutation = Diagonal >  swipe > rotation
            //bool isMouseScrollWheelForward = Input.GetAxis("Mouse ScrollWheel") > 0f;
            //bool isMouseScrollWheelBackward = Input.GetAxis("Mouse ScrollWheel") < 0f;
            bool isRightMouseClickDown = Input.GetMouseButtonDown(1);
            bool isRightMouseHold = Input.GetMouseButton(1);
            bool isLeftMouseHold = Input.GetMouseButton(0);
            bool isLeftMouseClickDown = Input.GetMouseButtonDown(0);
            bool isLeftMouseClickUp = Input.GetMouseButtonUp(0);

            //if ((isMouseScrollWheelForward || isMouseScrollWheelBackward) && !isLeftMouseHold && !isRightMouseHold)
            //{
            //    if (mySwipeFaceManager.InitSwipeMouseScroll(isMouseScrollWheelForward, Input.mousePosition))
            //    {
            //        currentPlayStatus = CubePlayStatus.InSwipe;
            //    }

            //}
            //else 
            
            if (isLeftMouseClickDown)
            {                
                initalMousePressPos = Input.mousePosition;
            }
            else if (isLeftMouseHold)
            {
                myCubeCursorController.setSwipeCursor();
            }
            else if (isLeftMouseClickUp)
            {
                endMousePressPos = Input.mousePosition;
                if (mySwipeFaceManager.InitSwipeMouseDrag(initalMousePressPos, endMousePressPos))
                {    
                    currentPlayStatus = CubePlayStatus.InSwipe;
                }
            }
            else if (isRightMouseClickDown)
            {
                if (myRotateWholeCubeManager.InitPosition())
                {
                    myCubeCursorController.setRotationCursor();
                    currentPlayStatus = CubePlayStatus.InRotation;
                }
            }
        }
        else if (currentPlayStatus == CubePlayStatus.InRotation)
        {
            myRotateWholeCubeManager.UpdateRotateWholeCube();
        }
        else if (currentPlayStatus == CubePlayStatus.InSwipe)
        {
            mySwipeFaceManager.UpdateSwipe();

        }
        else if (currentPlayStatus == CubePlayStatus.InCommutation)
        {
            myCommutationSkill.onUpdate();

        }
        else if (currentPlayStatus == CubePlayStatus.InDiagonal)
        {
            myDiagonalSkill.onUpdate();
        }

        else if (currentPlayStatus == CubePlayStatus.InRestoreCheckPoint)
        {
            if (restoreFinish)
            {
                currentPlayStatus = CubePlayStatus.WaitForInput;

            }
        }

        return false;
    }





    public override void onEnd()
    {
        // set UI button invisible

    }


    private IEnumerator startRestoreAnimation()
    {
        
        GameObject pocketCube = CubePlayManager.instance.pocketCube;

        float currentUsedTime = 0;
        float currentRotationDegree = 0;
        float t = 0;

        Vector3 startScale = Vector3.zero; 
        Vector3 endScale = pocketCube.transform.localScale;
        while (t < 1)
        {
            currentUsedTime += Time.deltaTime;
            t = currentUsedTime / restoreAnimationTime;

            float angle = Mathf.Lerp(0, 360, animationCurve.Evaluate(t));
            float deltaAngle = angle - currentRotationDegree;
            currentRotationDegree = angle;
            pocketCube.transform.RotateAround(pocketCube.transform.position, Vector3.up, deltaAngle);
            pocketCube.transform.localScale = Vector3.Lerp(startScale, endScale, animationCurve.Evaluate(t));

            // restore camera at the same time

            yield return null;

        }
        restoreFinish = true;

    }

}