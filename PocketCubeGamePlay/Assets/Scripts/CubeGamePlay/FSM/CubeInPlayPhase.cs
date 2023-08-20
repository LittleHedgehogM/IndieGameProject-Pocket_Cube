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
        //InRetart,
    };

    [SerializeField] private bool enableFinish;

    private CubePlayStatus currentPlayStatus;

    SwipeFaceManager mySwipeFaceManager;
    CommutationSkill myCommutationSkill;
    DiagonalSkill myDiagonalSkill;
    RotateWholeCubeManager myRotateWholeCubeManager;
    CubePlayUIController myUIController;
    CubePlayCameraController myCubePlayCameraController;
    CubeState cubeState;
    ReadCube readCube;

    Vector3 initalMousePressPos;
    Vector3 endMousePressPos;

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


        currentPlayStatus = CubePlayStatus.WaitForInput;
        myUIController.InitCubePlayUIElements();
    }


    private void OnEnable()
    {
        SwipeFaceManager.onSwipeFinished += onAnyCubeStateChanged;
        RotateWholeCubeManager.onRotateWholeCubeFinished += onRotationFinished;
        DiagonalSkill.onDiagonalFinished += onAnyCubeStateChanged;
        CommutationSkill.onCommutataionFinished += onAnyCubeStateChanged;
    }

    private void OnDisable()
    {
        SwipeFaceManager.onSwipeFinished -= onAnyCubeStateChanged;
        RotateWholeCubeManager.onRotateWholeCubeFinished -= onRotationFinished;
        DiagonalSkill.onDiagonalFinished -= onAnyCubeStateChanged;
        CommutationSkill.onCommutataionFinished -= onAnyCubeStateChanged;
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

    public void SetStateToCommutation()
    {
        if (currentPlayStatus == CubePlayStatus.WaitForInput)
        {
            if (myCommutationSkill.onStart())
            {
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
                currentPlayStatus = CubePlayStatus.InDiagonal;
            }
        }
    }

    public override void onRestart()
    {
        base.onRestart();
        currentPlayStatus = CubePlayStatus.WaitForInput;
        myUIController.onRestart();
        myDiagonalSkill.ResetValues();
        myCommutationSkill.ResetValues();
        myCubePlayCameraController.onRestart();
        

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
            // Temp Design : Restart the game
            // ReloadScene();

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
            readCube.ReadState();
            // priority : Commutation = Diagonal >  swipe > rotation
            bool isMouseScrollWheelForward = Input.GetAxis("Mouse ScrollWheel") > 0f;
            bool isMouseScrollWheelBackward = Input.GetAxis("Mouse ScrollWheel") < 0f;
            bool isRightMouseClickDown = Input.GetMouseButtonDown(1);
            bool isRightMouseHold = Input.GetMouseButton(1);
            bool isLeftMouseHold = Input.GetMouseButton(0);
            bool isLeftMouseClickDown = Input.GetMouseButtonDown(0);
            bool isLeftMouseClickUp = Input.GetMouseButtonUp(0);

            if ((isMouseScrollWheelForward || isMouseScrollWheelBackward) && !isLeftMouseHold && !isRightMouseHold)
            {
                if (mySwipeFaceManager.InitSwipeMouseScroll(isMouseScrollWheelForward, Input.mousePosition))
                {
                    currentPlayStatus = CubePlayStatus.InSwipe;
                }

            }
            else if (isLeftMouseClickDown)
            {
                initalMousePressPos = Input.mousePosition;
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

        return false;
    }



    public override void onEnd()
    {

    }


}