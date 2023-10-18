using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Electro_MoonPuzzle : MonoBehaviour
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

        public GameObject logicGateLeft;
        public GameObject logicGateRight;
        public GameObject logicGateCenter;

        public Electro_Switch switch_Or_left;
        public Electro_Switch switch_Or_right;
        public Electro_Switch switch_nand_left;
        public Electro_Switch switch_nand_right;
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
    private GameObject MoonFinishIcon;

    [SerializeField]
    GameObject GateVFX;

    [SerializeField]
    Electro_CollideChecker puzzleRangeCollider;

    [SerializeField] private Transform playerTargetPosPuzzle;

    [SerializeField] private GameObject CenterCubeObject;


    bool isFirstTimeEnter;
    Electro_Camera_Controller myCameraController;
    Electro_PlayerMovement myPlayerMovement;

    bool isLSwitchOrLeftOn;
    bool isLSwitchOrRightOn;
    bool isRSwitchNandLeftOn;
    bool isRSwitchNandRightOn;

    private bool isLeftAnimEnds;
    private bool isRightAnimEnds;

    private bool isLeftCircuitAnimPlaying = false;
    private bool isRightCircuitAnimPlaying = false;
    private bool isCenterCircuitAnimPlaying = false;

    public void setInteractable()
    {
        if (currentState == PuzzleState.NonInteractable)
        {
            currentState = PuzzleState.Interactable;
        }
    }

    private void OnEnable()
    {
        Electro_CollideChecker.EnterMoonRange += EnterRange;
        Electro_CollideChecker.LeaveMoonRange += LeaveRange;
        Electro_Camera_Controller.TranslateCameraFinish += onEnterRangeCameraTranslationFinish;
        Electro_Camera_Controller.ResetCameraFinish += onLeaveRangeCameraResetFinish;
        Electro_AnimController.MoonLeftCircuitAnimFinished += LeftMoonCircuitAnimEnds;
        Electro_AnimController.MoonRightCircuitAnimFinished += RightMoonCircuitAnimEnds;
        Electro_AnimController.MoonCenterCircuitAnimFinished += CenterCircuitAnimEnds;
        Electro_Switch.SwitchColorTranslationFinished += updateCircuit;
    }
    private void OnDisable()
    {
        Electro_CollideChecker.EnterMoonRange -= EnterRange;
        Electro_CollideChecker.LeaveMoonRange -= LeaveRange;
        Electro_Camera_Controller.TranslateCameraFinish -= onEnterRangeCameraTranslationFinish;
        Electro_Camera_Controller.ResetCameraFinish -= onLeaveRangeCameraResetFinish;
        Electro_AnimController.MoonLeftCircuitAnimFinished -= LeftMoonCircuitAnimEnds;
        Electro_AnimController.MoonRightCircuitAnimFinished -= RightMoonCircuitAnimEnds;
        Electro_AnimController.MoonCenterCircuitAnimFinished -= CenterCircuitAnimEnds;
        Electro_Switch.SwitchColorTranslationFinished -= updateCircuit;
    }

    public void Init()
    {
        myCameraController = FindObjectOfType<Electro_Camera_Controller>();
        myPlayerMovement = FindAnyObjectByType<Electro_PlayerMovement>();
        MoonFinishIcon.GetComponent<Renderer>().enabled = false;
        isFirstTimeEnter = true;
        isLeftAnimEnds = false;
        isRightAnimEnds = false;
    }

    public void EnterRange()
    {
        if (currentState == PuzzleState.Interactable)
        {
            myCameraController.showMoonCam();
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
            myCircuit.switch_Or_left.setInteractionEnabled(false);
            myCircuit.switch_Or_right.setInteractionEnabled(false);
            myCircuit.switch_nand_left.setInteractionEnabled(false);
            myCircuit.switch_nand_right.setInteractionEnabled(false);
            // StarPuzzleObject.SetActive(true);
            CenterCubeObject.SetActive(true);

        }
    }

    private void onEnterRangeCameraTranslationFinish()
    {
        if (myCameraController.isMoonCam())
        {
            currentState = PuzzleState.InPuzzle;
            myCircuit.switch_Or_left.setInteractionEnabled(true);
            myCircuit.switch_Or_right.setInteractionEnabled(true);
            myCircuit.switch_nand_left.setInteractionEnabled(true);
            myCircuit.switch_nand_right.setInteractionEnabled(true);
            myPlayerMovement.setEnableMovement(true);
            if (isFirstTimeEnter)
            {
                PlayVFXAt(myCircuit.logicGateLeft.transform);
                PlayVFXAt(myCircuit.logicGateRight.transform);
                PlayVFXAt(myCircuit.logicGateCenter.transform);
                isFirstTimeEnter = false;
            }
            isLSwitchOrLeftOn = myCircuit.switch_Or_left.isElectroSwitchOn();
            isLSwitchOrRightOn = myCircuit.switch_Or_right.isElectroSwitchOn();
            isRSwitchNandLeftOn = myCircuit.switch_nand_left.isElectroSwitchOn();
            isRSwitchNandRightOn = myCircuit.switch_nand_right.isElectroSwitchOn();
        }

    }

    private void onLeaveRangeCameraResetFinish()
    {
        currentState = PuzzleState.Interactable;
    }

    public bool isPuzzleSolved()
    {
        return myCircuit.switch_Or_left.isElectroSwitchOn()
            && myCircuit.switch_Or_right.isElectroSwitchOn();
    }

    private void PlayVFXAt(Transform gateTransform)
    {
        GameObject DisplayGateVFX = Instantiate(GateVFX, gateTransform.position, gateTransform.rotation);
        DisplayGateVFX.transform.SetParent(gateTransform);
        DisplayGateVFX.GetComponent<ParticleSystem>().Play();
    }


    private bool nand(bool a, bool b)
    {
        return !(a&&b);
    }

    private bool or(bool a, bool b)
    {
        return a || b;
    }

    private void updateCircuit()
    {      
        isLSwitchOrLeftOn = myCircuit.switch_Or_left.isElectroSwitchOn();
        isLSwitchOrRightOn = myCircuit.switch_Or_right.isElectroSwitchOn();
        isRSwitchNandLeftOn = myCircuit.switch_nand_left.isElectroSwitchOn();
        isRSwitchNandRightOn = myCircuit.switch_nand_right.isElectroSwitchOn();

    }

    private void cancelEffects()
    {
        centerCircuitAnimator.SetTrigger("GoIdle");
        turnAnimator.SetTrigger("GoIdle");
        MoonFinishIcon.GetComponent<Renderer>().enabled = false;
        lightAnimator.SetTrigger("GoIdle");
    }

    private void PlayCenterAnim()
    {
        centerCircuitAnimator.SetTrigger("PlayAnim");

    }

    public void LeftMoonCircuitAnimEnds()
    {
        isLeftAnimEnds = true;
        //PlayCenterAnim();
    }

    public void RightMoonCircuitAnimEnds()
    {
        isRightAnimEnds = true;
        //PlayCenterAnim();
    }

    private IEnumerator WaitAndPlayLightAnim()
    {
        yield return new WaitForSeconds(1);
        lightAnimator.SetTrigger("PlayAnim");

    }

    public void CenterCircuitAnimEnds()
    {
        MoonFinishIcon.GetComponent<Renderer>().enabled = true;
        turnAnimator.SetTrigger("PlayAnim");
        StartCoroutine(WaitAndPlayLightAnim());
    }

    public void UpdatePuzzle()
    {
        if (currentState == PuzzleState.InPuzzle) 
        {
            bool isLeftCircuitValid = !isLSwitchOrLeftOn && !isLSwitchOrRightOn;
            bool isRightCircuitValid = isRSwitchNandLeftOn && isRSwitchNandRightOn;
            GameObject hitObject = myCameraController.checkCollision();

            if (hitObject != null) 
            {
                if (hitObject == myCircuit.logicGateLeft && isLeftCircuitValid && !isCenterCircuitAnimPlaying)
                {
                    if (isLeftCircuitAnimPlaying)
                    {
                        leftCircuitAnimator.SetTrigger("GoIdle");
                        isLeftCircuitAnimPlaying = false;
                        myCircuit.switch_Or_left.setInteractionEnabled(true);
                        myCircuit.switch_Or_right.setInteractionEnabled(true);
                        cancelEffects();
                    }
                    else
                    {
                        leftCircuitAnimator.SetTrigger("PlayAnim");
                        isLeftCircuitAnimPlaying = true;
                        myCircuit.switch_Or_left.setInteractionEnabled(false);
                        myCircuit.switch_Or_right.setInteractionEnabled(false);
                        PlayVFXAt(myCircuit.logicGateLeft.transform);
                    }
                }
                else if (hitObject == myCircuit.logicGateRight && isRightCircuitValid && !isCenterCircuitAnimPlaying)
                {
                    if (isRightCircuitAnimPlaying)
                    {
                        rightCircuitAnimator.SetTrigger("GoIdle");
                        isRightCircuitAnimPlaying = false;
                        myCircuit.switch_nand_right.setInteractionEnabled(true);
                        myCircuit.switch_nand_left.setInteractionEnabled(true);
                        cancelEffects();
                    }
                    else
                    {
                        rightCircuitAnimator.SetTrigger("PlayAnim");
                        isRightCircuitAnimPlaying = true;
                        myCircuit.switch_nand_right.setInteractionEnabled(false);
                        myCircuit.switch_nand_left.setInteractionEnabled(false);
                        PlayVFXAt(myCircuit.logicGateRight.transform);

                    }
                }
                else if (hitObject == myCircuit.logicGateCenter 
                        && isLeftCircuitValid && isRightCircuitValid && isLeftAnimEnds && isRightAnimEnds)
                {
                    if (isCenterCircuitAnimPlaying)
                    {
                        isCenterCircuitAnimPlaying = false;
                        cancelEffects();
                    }
                    else
                    {
                        isCenterCircuitAnimPlaying = true;
                        PlayVFXAt(myCircuit.logicGateCenter.transform);
                        centerCircuitAnimator.SetTrigger("PlayAnim");
                        myCircuit.switch_nand_right.setInteractionEnabled(false);
                        myCircuit.switch_nand_left.setInteractionEnabled(false);
                        myCircuit.switch_Or_left.setInteractionEnabled(false);
                        myCircuit.switch_Or_right.setInteractionEnabled(false);
                    }
                }
            }
         
                
        }
    }
}