using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CubePlayManager : MonoBehaviour
{
    // Start is called before the first frame update

    public enum CubePlayStatus
    {
        WaitForInput,
        InRotation,
        InSwipe,
        InCommutation,
        InDiagonal,
        CubeSolved,
        InRetart,
    };

    private CubePlayStatus currentPlayStatus;

    public bool isCommutationUnlocked;
    public bool isDiagonalUnlocked;
    public int maxCommutation;
    public int maxDiagonal;
    public int suggestedStepCount;

    SwipeFaceManager mySwipeFaceManager;
    //CommutationManager myCommutationManager;

    CommutationSkill myCommutationSkill;
    DiagonalSkill myDiagonalSkill;

    //DiagonalManager myDiagonalManager;
    RotateWholeCubeManager myRotateWholeCubeManager;
    CubePlayUIController myUIController;
    CubeState cubeState;
    ReadCube readCube;

    Vector3 initalMousePressPos;
    Vector3 endMousePressPos;
    void Start()
    {
    }

    private void Awake()
    {
        
        mySwipeFaceManager          = FindObjectOfType<SwipeFaceManager>();
        //myCommutationManager        = FindObjectOfType<CommutationManager>();
        //myDiagonalManager           = FindObjectOfType<DiagonalManager>();

        myCommutationSkill          = FindObjectOfType<CommutationSkill>();
        myDiagonalSkill             = FindObjectOfType<DiagonalSkill>();

        myRotateWholeCubeManager    = FindObjectOfType<RotateWholeCubeManager>();
        cubeState                   = FindObjectOfType<CubeState>();
        readCube                    = FindObjectOfType<ReadCube>();
        myUIController              = FindObjectOfType<CubePlayUIController>();
        Initialize();
    }

    private void Initialize()
    {
        currentPlayStatus = CubePlayStatus.WaitForInput;
        myUIController.SwipeCount = 0;
        myUIController.DiagonalCount = 0;
        myUIController.CommutationCount = 0;
        myUIController.SolveResult = false;
        
    }

    private void OnEnable()
    {
        SwipeFaceManager.onSwipeFinished += onSwipeEnd;
        //DiagonalManager.onDiagonalFinished += onDiagonalFinished;
        //CommutationManager.onCommutationFinished += onCommutationFinished;
        RotateWholeCubeManager.onRotateWholeCubeFinished += onRotationFinished;
        DiagonalSkill.onDiagonalFinished += onDiagonalFinished;
        CommutationSkill.onCommutataionFinished += onCommutationFinished;
    }

    private void OnDisable()
    {
        SwipeFaceManager.onSwipeFinished -= onSwipeEnd;
        //DiagonalManager.onDiagonalFinished -= onDiagonalFinished;
        //CommutationManager.onCommutationFinished -= onCommutationFinished;
        RotateWholeCubeManager.onRotateWholeCubeFinished -= onRotationFinished;
        DiagonalSkill.onDiagonalFinished -= onDiagonalFinished;
        CommutationSkill.onCommutataionFinished -= onCommutationFinished;
    }

    void onRotationFinished()
    {
        SetCubePlayStatus(CubePlayStatus.WaitForInput);
    }
    void onCommutationFinished()
    {
        myUIController.CommutationCount++;
        SetCubePlayStatus(CubePlayStatus.WaitForInput);
    }

    void onDiagonalFinished()
    {
        myUIController.DiagonalCount++;
        SetCubePlayStatus(CubePlayStatus.WaitForInput);
    }
    void onSwipeEnd()
    {
        SetCubePlayStatus(CubePlayStatus.WaitForInput);
        myUIController.SwipeCount++;
    }

    public CubePlayStatus GetCubePlayStatus() 
    {
        return currentPlayStatus;
    }

    void SetCubePlayStatus(CubePlayStatus newStatus) 
    {
        currentPlayStatus = newStatus;
    }


    // check if cube is solved
    bool CheckCubeSolved()
    {        
        //readCube.ReadState();
        bool cubeSolved = cubeState.isCubeSolved();

        myUIController.SolveResult = cubeSolved;
        return cubeSolved;

    }

    private void ReloadScene()
    {
        Initialize();
        Scene scene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(scene.name);
    }
    
    public void SetStateToCommutation()
    {
        if (currentPlayStatus == CubePlayStatus.WaitForInput && isCommutationUnlocked)
        {
            if (/*myCommutationManager.InitCommutation()*/ myCommutationSkill.InitSkill())
            {              
                currentPlayStatus = CubePlayStatus.InCommutation;
            }            
        }        
    }

    public void SetStateToDiagonal()
    {
        if (currentPlayStatus == CubePlayStatus.WaitForInput && isDiagonalUnlocked)
        {
            if (/*myDiagonalManager.InitDiagonal()*/ myDiagonalSkill.InitSkill())
            {
                currentPlayStatus = CubePlayStatus.InDiagonal;
            }            
        }
    }

    public void Restart()
    {
        if (currentPlayStatus != CubePlayStatus.CubeSolved)
        {
            currentPlayStatus = CubePlayStatus.InRetart;
        }
    }

    public void SetStateTo(CubePlayStatus status)
    {
        currentPlayStatus = status;
    }

    // Update is called once per frame
    void Update()
    {
        // Temp disable
        CheckCubeSolved();
        if (currentPlayStatus == CubePlayStatus.CubeSolved)
        {
            // Temp Design : Restart the game
            // ReloadScene();

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

            if ( (isMouseScrollWheelForward || isMouseScrollWheelBackward) && !isLeftMouseHold && !isRightMouseHold)
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
        else if(currentPlayStatus == CubePlayStatus.InSwipe)
        {
            mySwipeFaceManager.UpdateSwipe();

        }
        else if (currentPlayStatus == CubePlayStatus.InCommutation)
        {
            //myCommutationManager.UpdateCommutation();
            myCommutationSkill.UpdateSkill();
            
        }
        else if (currentPlayStatus == CubePlayStatus.InDiagonal)
        {
            //myDiagonalManager.UpdateDiagonal();
            myDiagonalSkill.UpdateSkill();
        }
        else if (currentPlayStatus == CubePlayStatus.InRetart)
        {
            ReloadScene();
        }
    }
}
