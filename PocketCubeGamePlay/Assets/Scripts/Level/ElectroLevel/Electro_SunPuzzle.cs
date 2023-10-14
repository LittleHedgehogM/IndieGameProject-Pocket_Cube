using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.FullSerializer.Internal;
using UnityEngine;

public class Electro_SunPuzzle : MonoBehaviour
{
    [System.Serializable]
    public enum PuzzleState
    {
        NonInteractable,
        Interactable,
        InPuzzle
    }

    public PuzzleState currentState;
    
    [System.Serializable]
    public class Circuit
    {
        public Transform logicGateLeftTransform;
        public Transform logicGateRightTransform;
        public Transform logicGateCenterTransform;
        public Electro_Switch switch_Xor_left;
        public Electro_Switch switch_Xor_right;
        public Electro_Switch switch_not;
    }

    public Circuit myCircuit;

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
    private GameObject SunFinishIcon;

    [SerializeField]
    GameObject GateVFX;

    [SerializeField]
    Electro_CollideChecker puzzleRangeCollider;

    [SerializeField] private Transform playerTargetPosPuzzle;


    [SerializeField] private GameObject StarPuzzleObject;
    [SerializeField] private GameObject CenterCubeObject;


    bool isFirstTimeEnter;
    Electro_Camera_Controller myCameraController;
    Electro_PlayerMovement myPlayerMovement;

    bool isSwitchXorLeftOn; 
    bool isSwitchXorRightOn; 
    bool isSwitchNotOn;

    private bool isLeftAnimEnds;
    private bool isRightAnimEnds;
    public void setInteractable()
    {
        if (currentState == PuzzleState.NonInteractable)
        {
            currentState = PuzzleState.Interactable;
        }
    }

    private void OnEnable()
    {
        Electro_CollideChecker.EnterSunRange += EnterRange;
        Electro_CollideChecker.LeaveSunRange += LeaveRange;
        Electro_Camera_Controller.TranslateCameraFinish += onEnterRangeCameraTranslationFinish;
        Electro_Camera_Controller.ResetCameraFinish     += onLeaveRangeCameraResetFinish;
        Electro_AnimController.SunLeftCircuitAnimFinished   += LeftSunCircuitAnimEnds;
        Electro_AnimController.SunRightCircuitAnimFinished  += RightSunCircuitAnimEnds;
        Electro_AnimController.SunCenterCircuitAnimFinished += CenterCircuitAnimEnds;
        Electro_Switch.SwitchColorTranslationFinished += updateCircuit;

    }
    private void OnDisable()
    {
        Electro_CollideChecker.EnterSunRange -= EnterRange;
        Electro_CollideChecker.LeaveSunRange -= LeaveRange;
        Electro_Camera_Controller.TranslateCameraFinish -= onEnterRangeCameraTranslationFinish;
        Electro_Camera_Controller.ResetCameraFinish -= onLeaveRangeCameraResetFinish;
        Electro_AnimController.SunLeftCircuitAnimFinished   -= LeftSunCircuitAnimEnds;
        Electro_AnimController.SunRightCircuitAnimFinished  -= RightSunCircuitAnimEnds;
        Electro_AnimController.SunCenterCircuitAnimFinished -= CenterCircuitAnimEnds;
        Electro_Switch.SwitchColorTranslationFinished += updateCircuit;

    }

    public void Init()
    {
        myCameraController = FindObjectOfType<Electro_Camera_Controller>();
        myPlayerMovement = FindAnyObjectByType<Electro_PlayerMovement>();
        SunFinishIcon.GetComponent<Renderer>().enabled = false;
        isFirstTimeEnter = true;
        isLeftAnimEnds = false;
        isRightAnimEnds = false;
    }

    public void EnterRange()
    {
        if (currentState == PuzzleState.Interactable)
        {
            myCameraController.showSunCam();
            myPlayerMovement.TranslateTo(playerTargetPosPuzzle);
            myPlayerMovement.setEnableMovement(false);
            //StarPuzzleObject.SetActive(false);
            CenterCubeObject.SetActive(false);
        }

    }

    public void LeaveRange()
    {
        if (currentState == PuzzleState.InPuzzle)
        {
            myCameraController.resetSunCam();
            myCircuit.switch_Xor_left.setInteractionEnabled(false);
            myCircuit.switch_Xor_right.setInteractionEnabled(false);
            myCircuit.switch_not.setInteractionEnabled(false);
           // StarPuzzleObject.SetActive(true);
            CenterCubeObject.SetActive(true);

        }
    }

    private void onEnterRangeCameraTranslationFinish()
    {
        if (myCameraController.isSunCam())
        {
            currentState = PuzzleState.InPuzzle;
            myCircuit.switch_Xor_left.setInteractionEnabled(true);
            myCircuit.switch_Xor_right.setInteractionEnabled(true);
            myCircuit.switch_not.setInteractionEnabled(true);
            myPlayerMovement.setEnableMovement(true);
            if (isFirstTimeEnter)
            {
                PlayVFXAt(myCircuit.logicGateLeftTransform);
                PlayVFXAt(myCircuit.logicGateRightTransform);
                PlayVFXAt(myCircuit.logicGateCenterTransform);
                isFirstTimeEnter = false;
            }
            isSwitchXorLeftOn   = myCircuit.switch_Xor_left.isElectroSwitchOn();
            isSwitchXorRightOn  = myCircuit.switch_Xor_right.isElectroSwitchOn();
            isSwitchNotOn       = myCircuit.switch_not.isElectroSwitchOn();
        }
        
    }

    private void onLeaveRangeCameraResetFinish()
    {
         currentState = PuzzleState.Interactable;
    }

    public bool isPuzzleSolved()
    {
        return myCircuit.switch_Xor_left.isElectroSwitchOn()
            && myCircuit.switch_Xor_right.isElectroSwitchOn();
    }

    private void PlayVFXAt(Transform gateTransform)
    {
        GameObject DisplayGateVFX = Instantiate(GateVFX, gateTransform.position, gateTransform.rotation);
        DisplayGateVFX.transform.SetParent(gateTransform);
        DisplayGateVFX.GetComponent<ParticleSystem>().Play();
    }


    private bool xor(bool a, bool b){
        return !a && !b;
    }
    private void updateCircuit()
    {

        //myCameraController.checkCollision();
        bool currentLeftCircuitResult = xor(myCircuit.switch_Xor_left.isElectroSwitchOn(), myCircuit.switch_Xor_right.isElectroSwitchOn());
        

        if (!xor(isSwitchXorLeftOn, isSwitchXorRightOn) && currentLeftCircuitResult)
        {
            leftCircuitAnimator.SetTrigger("PlayAnim");
            isLeftAnimEnds = false;
        }
        if (xor(isSwitchXorLeftOn, isSwitchXorRightOn) && !currentLeftCircuitResult)
        {
            leftCircuitAnimator.SetTrigger("GoIdle");
            isLeftAnimEnds = false;
            cancelEffects();
        }

        if (isSwitchNotOn == true && !myCircuit.switch_not.isElectroSwitchOn())
        {
            rightCircuitAnimator.SetTrigger("PlayAnim");
            isRightAnimEnds = false;

        }

        if (isSwitchNotOn == false && myCircuit.switch_not.isElectroSwitchOn())
        {
            rightCircuitAnimator.SetTrigger("GoIdle");
            isRightAnimEnds = false;
            cancelEffects();

        }

        isSwitchXorLeftOn = myCircuit.switch_Xor_left.isElectroSwitchOn();
        isSwitchXorRightOn = myCircuit.switch_Xor_right.isElectroSwitchOn();
        isSwitchNotOn = myCircuit.switch_not.isElectroSwitchOn();

    }

    private void cancelEffects()
    {
        centerCircuitAnimator.SetTrigger("GoIdle");
        turnAnimator.SetTrigger("GoIdle");
        SunFinishIcon.GetComponent<Renderer>().enabled = false;
        lightAnimator.SetTrigger("GoIdle");
    }

    private void PlayCenterAnim()
    {
        if (xor(isSwitchXorLeftOn, isSwitchXorRightOn) && !isSwitchNotOn && isLeftAnimEnds && isRightAnimEnds)
        {
            //centerCircuitAnimator.SetTrigger("PlayAnim");
            centerCircuitAnimator.Play("AM_Sun_Middle");
        }
    }

    public void LeftSunCircuitAnimEnds()
    {
        isLeftAnimEnds = true;
        PlayCenterAnim();
    }

    public void RightSunCircuitAnimEnds()
    {
        isRightAnimEnds = true;
        PlayCenterAnim();
    }

    private IEnumerator WaitAndPlayLightAnim()
    {
        yield return new WaitForSeconds(1);
        lightAnimator.SetTrigger("PlayAnim");
        
    }

    public void CenterCircuitAnimEnds()
    {
        //isMiddleAnimEnds = true;
        SunFinishIcon.GetComponent<Renderer>().enabled = true;
        //turnAnimator.SetTrigger("PlayAnim");
        turnAnimator.Play("AM_Sun_Turn");
        StartCoroutine(WaitAndPlayLightAnim());
    }

    public void UpdatePuzzle()
    {
        //switch (currentState)
        //{
        //    case PuzzleState.NonInteractable:
        //        {
        //            break;
        //        }
        //    case PuzzleState.Interactable:
        //        {
        //            break;
        //        }
        //    case PuzzleState.InPuzzle:
        //        {
        //            updateCircuit();
        //            break;
        //        }
        //}
    }

}