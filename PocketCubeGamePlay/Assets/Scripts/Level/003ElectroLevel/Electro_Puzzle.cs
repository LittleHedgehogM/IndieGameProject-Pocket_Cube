using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Electro_Puzzle : MonoBehaviour
{

    [System.Serializable]
    public enum GateLogic
    {
        AND,
        OR,
        NOT,
        NAND,
        NOR
    }

    [System.Serializable]
    public class Circuit
    {
        public GateLogic logic;
        public GameObject logicGateHolder;
        public Electro_Switch switch_Left;
        public Electro_Switch switch_right;
    }

    public Circuit myCircuit;

    [System.Serializable]
    public enum PuzzleState
    {
        NonInteractable,
        Interactable,
        InPuzzle
    }

    public PuzzleState currentState;

    [SerializeField]
    private Animator leftCircuitAnimator;

    [SerializeField]
    private Animator rightCircuitAnimator;

    [SerializeField]
    private Animator centerCircuitAnimator;

    [SerializeField]
    private Animator lightAnimator;

    [SerializeField]
    private Animator turnAnimator;

    [SerializeField]
    private GameObject StarFinishIcon;

    [SerializeField]
    GameObject GateVFX;

    [SerializeField]
    Electro_CollideChecker puzzleRangeCollider;

    [SerializeField] 
    private Transform playerTargetPosPuzzle;

    [SerializeField]
    private Electro_MeshControl starMeshControl;

    bool isFirstTimeEnter;
    Electro_Camera_Controller myCameraController;
    Electro_PlayerMovement myPlayerMovement;
    Electro_VFX_ObjectPool myVFXObjectPool;

    bool isLeftSwitchOn; //= myCircuit.switch_Left.isElectroSwitchOn();
    bool isRightSwitchOn; //= myCircuit.switch_right.isElectroSwitchOn();

    //private bool isLeftAnimEnds;
    //private bool isRightAnimEnds;

    private bool isCircuitAnimPlaying;
    private bool isPuzzleSolved = false;


    private void setGateAndSwitchNotInteractable(bool enable)
    {
        myCircuit.switch_Left.setInteractionEnabled(enable);
        myCircuit.switch_right.setInteractionEnabled(enable);
        myCircuit.logicGateHolder.GetComponentInChildren<Electro_LogicGate>().setInteractionEnabled(enable);
    }

    public void setNotInteractable()
    {
        starMeshControl.Show();
        setGateAndSwitchNotInteractable(false);
        currentState = PuzzleState.NonInteractable;
    }
    public bool isInPuzzle()
    {
        return currentState == PuzzleState.InPuzzle;
    }
    public void setIsPuzzleSolved()
    {
        isPuzzleSolved = true;
    }
    public bool getIsPuzzleSolved()
    {
        return isPuzzleSolved;
    }

    private void OnEnable()
    {
        Electro_CollideChecker.EnterStarRange += EnterRange;
        Electro_CollideChecker.LeaveStarRange += LeaveRange;
        Electro_Camera_Controller.TranslateCameraFinish += onEnterRangeCameraTranslationFinish;
        Electro_Camera_Controller.ResetCameraFinish += onLeaveRangeCameraResetFinish;
        Electro_AnimController.StarLeftCircuitAnimFinished += LeftStarCircuitAnimEnds;
        Electro_AnimController.StarRightCircuitAnimFinished  += RightStarCircuitAnimEnds;
        Electro_AnimController.StarCenterCircuitAnimFinished += CenterCircuitAnimEnds;
        Electro_Switch.SwitchColorTranslationFinished += updateCircuit;
        Electro_AnimController.StarPuzzleSolved += setIsPuzzleSolved;
    }

    private void OnDisable()
    {
        Electro_CollideChecker.EnterStarRange -= EnterRange;
        Electro_CollideChecker.LeaveStarRange -= LeaveRange;
        Electro_Camera_Controller.TranslateCameraFinish -= onEnterRangeCameraTranslationFinish;
        Electro_Camera_Controller.ResetCameraFinish -= onLeaveRangeCameraResetFinish;
        Electro_AnimController.StarLeftCircuitAnimFinished -= LeftStarCircuitAnimEnds;
        Electro_AnimController.StarRightCircuitAnimFinished -= RightStarCircuitAnimEnds;
        Electro_AnimController.StarCenterCircuitAnimFinished -= CenterCircuitAnimEnds;
        Electro_Switch.SwitchColorTranslationFinished += updateCircuit;
        Electro_AnimController.StarPuzzleSolved -= setIsPuzzleSolved;

    }


    public void Init()
    {
        myCameraController = FindObjectOfType<Electro_Camera_Controller>();
        myPlayerMovement = FindAnyObjectByType<Electro_PlayerMovement>();
        myVFXObjectPool = FindObjectOfType<Electro_VFX_ObjectPool>();
        StarFinishIcon.GetComponent<Renderer>().enabled = false;
        isFirstTimeEnter = true;
        setGateAndSwitchNotInteractable(false);


        //isLeftAnimEnds = false;
        //isRightAnimEnds = false;
    }

    public void EnterRange()
    {
        if (currentState == PuzzleState.Interactable)
        {
            currentState = PuzzleState.InPuzzle;
            myCameraController.showStarCam();
            myPlayerMovement.TranslateTo(playerTargetPosPuzzle);
            myPlayerMovement.setEnableMovement(false);
        }
            
    }

    public void LeaveRange()
    {
        if (currentState == PuzzleState.InPuzzle)
        {
            myCameraController.resetCam();
            setGateAndSwitchNotInteractable(false);
            currentState = PuzzleState.Interactable;
        }     
    }
    
    private void onEnterRangeCameraTranslationFinish()
    {
        if (myCameraController.isStarCam())
        { 
            currentState = PuzzleState.InPuzzle;
            setGateAndSwitchNotInteractable(true);

            myPlayerMovement.setEnableMovement(true);
            if (isFirstTimeEnter)
            {
                myVFXObjectPool.PlayVFXAt(myCircuit.switch_Left.gameObject.transform);
                myVFXObjectPool.PlayVFXAt(myCircuit.switch_right.gameObject.transform);
                isFirstTimeEnter = false;
            }
            isLeftSwitchOn  = myCircuit.switch_Left.isElectroSwitchOn();
            isRightSwitchOn = myCircuit.switch_right.isElectroSwitchOn();
        
        }
        
    }

    private void onLeaveRangeCameraResetFinish()
    {
        if (currentState == PuzzleState.InPuzzle)
        {
            currentState = PuzzleState.Interactable;
        }
        myPlayerMovement.setEnableMovement(true);
    }


    private void updateCircuit()
    {
        isLeftSwitchOn  = myCircuit.switch_Left.isElectroSwitchOn();
        isRightSwitchOn = myCircuit.switch_right.isElectroSwitchOn();
    }

    private void cancelEffects()
    {
        leftCircuitAnimator.SetTrigger("GoIdle");
        rightCircuitAnimator.SetTrigger("GoIdle");
        centerCircuitAnimator.SetTrigger("GoIdle");
        turnAnimator.SetTrigger("GoIdle");
        lightAnimator.SetTrigger("GoIdle");
        StarFinishIcon.GetComponent<Renderer>().enabled = false;
    }

    private void PlayCenterAnim()
    {
        if (isLeftSwitchOn && isRightSwitchOn)
        {
            centerCircuitAnimator.SetTrigger("PlayAnim");
        }
    }

    public void LeftStarCircuitAnimEnds()
    {
        PlayCenterAnim();
    }

    public void RightStarCircuitAnimEnds()
    {
        PlayCenterAnim();
    }

    private IEnumerator WaitAndPlayLightAnim()
    {
        yield return new WaitForSeconds(1);
        lightAnimator.SetTrigger("PlayAnim");
    }

    public void CenterCircuitAnimEnds()
    {
        StarFinishIcon.GetComponent<Renderer>().enabled = true;
        turnAnimator.SetTrigger("PlayAnim");
        StartCoroutine(WaitAndPlayLightAnim());
    }

    public void UpdatePuzzle()
    {
       
        if (isLeftSwitchOn && isRightSwitchOn && currentState == PuzzleState.InPuzzle)
        {
            GameObject hitObject = myCameraController.checkCollision();
            if (hitObject != null && hitObject == myCircuit.logicGateHolder)
            {
                if (isCircuitAnimPlaying)
                {
                    isCircuitAnimPlaying = false;             
                    myCircuit.switch_Left.setInteractionEnabled(true);
                    myCircuit.switch_right.setInteractionEnabled(true);
                    myCircuit.logicGateHolder.GetComponentInChildren<Electro_LogicGate>().setInteractionEnabled(true);


                    cancelEffects();
                    isPuzzleSolved = false;
                }
                else
                {
                    isCircuitAnimPlaying = true;
                    myCircuit.switch_Left.setInteractionEnabled(false);
                    myCircuit.switch_right.setInteractionEnabled(false);
                    leftCircuitAnimator.SetTrigger("PlayAnim");
                    rightCircuitAnimator.SetTrigger("PlayAnim");
                    myVFXObjectPool.PlayVFXAt(myCircuit.logicGateHolder.transform);
                }
                
            }

        }
       
    }

}