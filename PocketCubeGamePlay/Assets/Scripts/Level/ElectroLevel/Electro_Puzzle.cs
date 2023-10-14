using System;
using System.Collections;
using System.Collections.Generic;
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
        public Transform logicGateTransform;
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

    [SerializeField] private Transform playerTargetPosPuzzle;

    bool isFirstTimeEnter;
    Electro_Camera_Controller myCameraController;
    Electro_PlayerMovement myPlayerMovement;

    bool isLeftSwitchOn; //= myCircuit.switch_Left.isElectroSwitchOn();
    bool isRightSwitchOn; //= myCircuit.switch_right.isElectroSwitchOn();

    private bool isLeftAnimEnds;
    private bool isRightAnimEnds;
    public void setInteractable()
    {
        if (currentState == PuzzleState.NonInteractable )
        {
            currentState = PuzzleState.Interactable;
        }
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

    }


    public void Init()
    {
        myCameraController = FindObjectOfType<Electro_Camera_Controller>();
        myPlayerMovement = FindAnyObjectByType<Electro_PlayerMovement>();
        StarFinishIcon.GetComponent<Renderer>().enabled = false;
        isFirstTimeEnter = true;
        isLeftAnimEnds = false;
        isRightAnimEnds = false;
        //isMiddleAnimEnds = false;
    }

    public void EnterRange()
    {
        if (currentState == PuzzleState.Interactable)
        {
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
            myCircuit.switch_Left.setInteractionEnabled(false);
            myCircuit.switch_right.setInteractionEnabled(false);
        }     
    }
    
    private void onEnterRangeCameraTranslationFinish()
    {
        if (myCameraController.isStarCam())
        { 
            currentState = PuzzleState.InPuzzle;
            myCircuit.switch_Left.setInteractionEnabled(true);
            myCircuit.switch_right.setInteractionEnabled(true);
            myPlayerMovement.setEnableMovement(true);
            if (isFirstTimeEnter)
            {
                PlayVFXAt(myCircuit.logicGateTransform);
                isFirstTimeEnter = false;
            }
            isLeftSwitchOn  = myCircuit.switch_Left.isElectroSwitchOn();
            isRightSwitchOn = myCircuit.switch_right.isElectroSwitchOn();
        
        }

        
    }

    private void onLeaveRangeCameraResetFinish()
    {
        currentState = PuzzleState.Interactable;
    }

    public bool isPuzzleSolved()
    {
        return myCircuit.switch_Left.isElectroSwitchOn() 
            && myCircuit.switch_right.isElectroSwitchOn();
    }

    private void PlayVFXAt(Transform gateTransform)
    {
        GameObject DisplayGateVFX = Instantiate(GateVFX, gateTransform.position, gateTransform.rotation);
        DisplayGateVFX.transform.SetParent(gateTransform);
        DisplayGateVFX.GetComponent<ParticleSystem>().Play();
    }


    private void updateCircuit()
    {

        if (!isLeftSwitchOn && myCircuit.switch_Left.isElectroSwitchOn())
        {
            leftCircuitAnimator.SetTrigger("PlayAnim");
        }


        if (!isRightSwitchOn && myCircuit.switch_right.isElectroSwitchOn())
        {
             rightCircuitAnimator.SetTrigger("PlayAnim");
        }

        if (isRightSwitchOn && !myCircuit.switch_right.isElectroSwitchOn())
        {
             rightCircuitAnimator.SetTrigger("GoIdle");
             isRightAnimEnds = false;
             cancelEffects();
        }
        
        if (isLeftSwitchOn && !myCircuit.switch_Left.isElectroSwitchOn())
        {
            leftCircuitAnimator.SetTrigger("GoIdle");
            isLeftAnimEnds = false;
            cancelEffects();
        }
        isLeftSwitchOn  = myCircuit.switch_Left.isElectroSwitchOn();
        isRightSwitchOn = myCircuit.switch_right.isElectroSwitchOn();
    }

    private void cancelEffects()
    {

        centerCircuitAnimator.SetTrigger("GoIdle");
        turnAnimator.SetTrigger("GoIdle");
        StarFinishIcon.GetComponent<Renderer>().enabled = false;
        lightAnimator.SetTrigger("GoIdle");
    }

    private void PlayCenterAnim()
    {
        if (isLeftSwitchOn && isRightSwitchOn && isLeftAnimEnds && isRightAnimEnds)
        {
            centerCircuitAnimator.SetTrigger("PlayAnim");
        }
    }

    public void LeftStarCircuitAnimEnds()
    {
        isLeftAnimEnds = true;
        PlayCenterAnim();
    }

    public void RightStarCircuitAnimEnds()
    {
        isRightAnimEnds = true;
        PlayCenterAnim();
    }

    private IEnumerator WaitAndPlayLightAnim()
    {
        yield return new WaitForSeconds(1);
        lightAnimator.Play("AM_Star_Light");
    }

    public void CenterCircuitAnimEnds()
    {
        //isMiddleAnimEnds = true;
        StarFinishIcon.GetComponent<Renderer>().enabled = true;
        //turnAnimator.Play("AM_Star_Turn", -1, 0f);
        turnAnimator.SetTrigger("PlayAnim");
        // wait 1 sec and play lightAnim
        StartCoroutine(WaitAndPlayLightAnim());
    }

    public void UpdatePuzzle()
    {
        //switch(currentState)
        //{
        //    case PuzzleState.NonInteractable:
        //    {
        //        break;
        //    }
        //    case PuzzleState.Interactable:
        //    {
        //        break;
        //    }
        //    case PuzzleState.InPuzzle:
        //    {
        //        updateCircuit();         
        //        break;
        //    }
        //}
    }

}