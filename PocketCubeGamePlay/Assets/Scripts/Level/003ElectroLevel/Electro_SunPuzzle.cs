using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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
        public GameObject logicGateLeft;
        public GameObject logicGateRight;
        public GameObject logicGateCenter;
        public Electro_Switch switch_Nand_left;
        public Electro_Switch switch_Nand_right;
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

    [SerializeField] private Electro_MeshControl RightWall;
    [SerializeField] private Electro_MeshControl StarPuzzle;
    [SerializeField] private Electro_MeshControl MoonPuzzle;
    [SerializeField] private Electro_MeshControl SunPuzzleMeshControl;
    [SerializeField] private Electro_MeshControl centerMeshControl;

    bool isFirstTimeEnter;
    Electro_Camera_Controller myCameraController;
    Electro_PlayerMovement myPlayerMovement;
    Electro_VFX_ObjectPool myVFXObjectPool;

    bool isSwitchNandLeftOn; 
    bool isSwitchNandRightOn; 
    bool isSwitchNotOn;

    private bool isLeftAnimEnds;
    private bool isRightAnimEnds;

    private bool isLeftCircuitAnimPlaying = false;
    private bool isRightCircuitAnimPlaying = false;
    private bool isCenterCircuitAnimPlaying = false;

    private bool isPuzzleSolved;


    private void setGateAndSwitchNotInteractable(bool enable)
    {
        myCircuit.switch_Nand_left.setInteractionEnabled(enable);
        myCircuit.switch_Nand_right.setInteractionEnabled(enable);
        myCircuit.switch_not.setInteractionEnabled(enable);
        myCircuit.logicGateRight.GetComponentInChildren<Electro_LogicGate>().setInteractionEnabled(enable);
        myCircuit.logicGateLeft.GetComponentInChildren<Electro_LogicGate>().setInteractionEnabled(enable);
        myCircuit.logicGateCenter.GetComponentInChildren<Electro_LogicGate>().setInteractionEnabled(enable);
    }


    public void setNotInteractable()
    {
        SunPuzzleMeshControl.Show();
        currentState = PuzzleState.NonInteractable;
        setGateAndSwitchNotInteractable(false);

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
        Electro_CollideChecker.EnterSunRange += EnterRange;
        Electro_CollideChecker.LeaveSunRange += LeaveRange;
        Electro_Camera_Controller.TranslateCameraFinish += onEnterRangeCameraTranslationFinish;
        Electro_Camera_Controller.ResetCameraFinish     += onLeaveRangeCameraResetFinish;
        Electro_AnimController.SunLeftCircuitAnimFinished   += LeftSunCircuitAnimEnds;
        Electro_AnimController.SunRightCircuitAnimFinished  += RightSunCircuitAnimEnds;
        Electro_AnimController.SunCenterCircuitAnimFinished += CenterCircuitAnimEnds;
        Electro_Switch.SwitchColorTranslationFinished += updateCircuit;
        Electro_AnimController.SunPuzzleSolved += setIsPuzzleSolved;

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
        Electro_AnimController.SunPuzzleSolved -= setIsPuzzleSolved;


    }

    public void Init()
    {
        myCameraController = FindObjectOfType<Electro_Camera_Controller>();
        myPlayerMovement = FindAnyObjectByType<Electro_PlayerMovement>();
        SunFinishIcon.GetComponent<Renderer>().enabled = false;
        myVFXObjectPool = FindObjectOfType<Electro_VFX_ObjectPool>();
        isFirstTimeEnter = true;
        isLeftAnimEnds = false;
        isRightAnimEnds = false;
        setGateAndSwitchNotInteractable(false);

    }

    public void EnterRange()
    {
        if (currentState == PuzzleState.Interactable)
        {
            currentState = PuzzleState.InPuzzle;

            myCameraController.showSunCam();
            myPlayerMovement.TranslateTo(playerTargetPosPuzzle);
            myPlayerMovement.setEnableMovement(false);
            //CenterCubeObject.SetActive(false);
            centerMeshControl.Hide();

        }

    }

    public void LeaveRange()
    {
        if (currentState == PuzzleState.InPuzzle)
        {
            //myPlayerMovement.setEnableMovement(false);
            myCameraController.resetSunCam();
            setGateAndSwitchNotInteractable(false);
            //CenterCubeObject.SetActive(true);
            centerMeshControl.Show();
            RightWall.Show();
            StarPuzzle.Show();
            MoonPuzzle.Show();
        }
    }

    private void onEnterRangeCameraTranslationFinish()
    {
        if (myCameraController.isSunCam())
        {
            currentState = PuzzleState.InPuzzle;
            setGateAndSwitchNotInteractable(true);
            myPlayerMovement.setEnableMovement(true);
            if (isFirstTimeEnter)
            {
                //PlayVFXAt(myCircuit.logicGateLeft.transform);
                //PlayVFXAt(myCircuit.logicGateRight.transform);
                //PlayVFXAt(myCircuit.logicGateCenter.transform);
                myVFXObjectPool.PlayVFXAt(myCircuit.switch_Nand_left.gameObject.transform);
                myVFXObjectPool.PlayVFXAt(myCircuit.switch_Nand_right.gameObject.transform);
                myVFXObjectPool.PlayVFXAt(myCircuit.switch_not.gameObject.transform);
                isFirstTimeEnter = false;
            }

            if (isPuzzleSolved) 
            {
                myCircuit.switch_Nand_left.setInteractionEnabled(false);
                myCircuit.switch_Nand_right.setInteractionEnabled(false);
                myCircuit.switch_not.setInteractionEnabled(false);
            }

            isSwitchNandLeftOn = myCircuit.switch_Nand_left.isElectroSwitchOn();
            isSwitchNandRightOn = myCircuit.switch_Nand_right.isElectroSwitchOn();
            isSwitchNotOn = myCircuit.switch_not.isElectroSwitchOn();

            // hide 
            RightWall.Hide();
            StarPuzzle.Hide();
            MoonPuzzle.Hide();

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


    private void PlayVFXAt(Transform gateTransform)
    {
        GameObject DisplayGateVFX = Instantiate(GateVFX, gateTransform.position, gateTransform.rotation);
        DisplayGateVFX.transform.SetParent(gateTransform);
        DisplayGateVFX.GetComponent<ParticleSystem>().Play();
    }


    //private bool xor(bool a, bool b){
    //    return !a && !b;
    //}

    private void updateCircuit()
    {
        isSwitchNandLeftOn   = myCircuit.switch_Nand_left.isElectroSwitchOn();
        isSwitchNandRightOn  = myCircuit.switch_Nand_right.isElectroSwitchOn();
        isSwitchNotOn       = myCircuit.switch_not.isElectroSwitchOn();

    }

    private void cancelEffects()
    {
        centerCircuitAnimator.SetTrigger("GoIdle");
        turnAnimator.SetTrigger("GoIdle");
        SunFinishIcon.GetComponent<Renderer>().enabled = false;
        lightAnimator.SetTrigger("GoIdle");
    }

    public void LeftSunCircuitAnimEnds()
    {
        isLeftAnimEnds = true;
    }

    public void RightSunCircuitAnimEnds()
    {
        isRightAnimEnds = true;
    }

    private IEnumerator WaitAndPlayLightAnim()
    {
        yield return new WaitForSeconds(1);
        lightAnimator.SetTrigger("PlayAnim");
        
    }

    public void CenterCircuitAnimEnds()
    {
        SunFinishIcon.GetComponent<Renderer>().enabled = true;
        turnAnimator.Play("AM_Sun_Turn");
        StartCoroutine(WaitAndPlayLightAnim());
    }

    public void UpdatePuzzle()
    {
        if (currentState == PuzzleState.InPuzzle) 
        {
            GameObject hitObject = myCameraController.checkCollision();

            if (hitObject != null)
            {
                if (hitObject == myCircuit.logicGateLeft && !isSwitchNandLeftOn && !isSwitchNandRightOn && !isCenterCircuitAnimPlaying)
                {
                    if (isLeftCircuitAnimPlaying)
                    {
                        //leftCircuitAnimator.SetTrigger("GoIdle");
                        //isLeftCircuitAnimPlaying = false;
                        //myCircuit.switch_Nand_right.setInteractionEnabled(true);
                        //myCircuit.switch_Nand_left.setInteractionEnabled(true);
                    }
                    else
                    {
                        leftCircuitAnimator.SetTrigger("PlayAnim");
                        isLeftCircuitAnimPlaying = true;
                        myCircuit.switch_Nand_right.setInteractionEnabled(false);
                        myCircuit.switch_Nand_left.setInteractionEnabled(false);
                        myVFXObjectPool.PlayVFXAt(myCircuit.logicGateLeft.transform);
                    }
                }
                if (hitObject == myCircuit.logicGateRight && !isSwitchNotOn && !isCenterCircuitAnimPlaying)
                {
                    if (isRightCircuitAnimPlaying)
                    {
                        //rightCircuitAnimator.SetTrigger("GoIdle");
                        //isRightCircuitAnimPlaying = false;
                        //myCircuit.switch_not.setInteractionEnabled(true);
                    }
                    else
                    {
                        rightCircuitAnimator.SetTrigger("PlayAnim");
                        isRightCircuitAnimPlaying = true;
                        myCircuit.switch_not.setInteractionEnabled(false);
                        myVFXObjectPool.PlayVFXAt(myCircuit.logicGateRight.transform);

                    }

                }
                if (hitObject == myCircuit.logicGateCenter && !isSwitchNandLeftOn && !isSwitchNandRightOn && !isSwitchNotOn
                                        && isLeftAnimEnds && isRightAnimEnds)        
                {
                    if (isCenterCircuitAnimPlaying)
                    {
                        //centerCircuitAnimator.SetTrigger("GoIdle");
                        //isCenterCircuitAnimPlaying = false;
                        //cancelEffects();
                        //isPuzzleSolved = false;
                        //myCircuit.logicGateRight.GetComponentInChildren<Electro_LogicGate>().setInteractionEnabled(true);
                        //myCircuit.logicGateLeft.GetComponentInChildren<Electro_LogicGate>().setInteractionEnabled(true);

                    }
                    else
                    {
                        centerCircuitAnimator.SetTrigger("PlayAnim");
                        isCenterCircuitAnimPlaying = true;
                        myVFXObjectPool.PlayVFXAt(myCircuit.logicGateCenter.transform);
                        myCircuit.switch_not.setInteractionEnabled(false);
                        myCircuit.switch_Nand_right.setInteractionEnabled(false);
                        myCircuit.switch_Nand_left.setInteractionEnabled(false);
                        myCircuit.logicGateRight.GetComponentInChildren<Electro_LogicGate>().setInteractionEnabled(false);
                        myCircuit.logicGateLeft.GetComponentInChildren<Electro_LogicGate>().setInteractionEnabled(false);

                    }
                }
            }
            
        }
    }

}