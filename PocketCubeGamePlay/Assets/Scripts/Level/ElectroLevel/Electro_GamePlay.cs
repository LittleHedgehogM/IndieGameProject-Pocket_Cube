using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Electro_GamePlay : MonoBehaviour
{
    Electro_Camera_Controller myCameraController;
    Electro_PlayerMovement myPlayerMovement;
    [SerializeField] private Transform playerTargetPosStarPuzzle;
    StarPuzzleController myStarPuzzleController;
    
    enum Electro_PlayStatus
    {
        Wandering,
        InTranslation,
        InPuzzle,
    }

    Electro_PlayStatus currentStatus;


    // Start is called before the first frame update
    void Start()
    {
        myCameraController  = FindObjectOfType<Electro_Camera_Controller>();
        myPlayerMovement    = FindObjectOfType<Electro_PlayerMovement>();
        myStarPuzzleController = FindObjectOfType<StarPuzzleController>();
        currentStatus       = Electro_PlayStatus.Wandering;
    }
    private void OnEnable()
    {
        Electro_CollideChecker.EnterStarRange += onEnterStarRange;
        Electro_CollideChecker.LeaveStarRange += onExitStarRange;

        //Electro_CollideChecker.EnterSwitchControlPos += onEnterSwitchControlRange;
        //Electro_CollideChecker.LeaveSwitchControlPos += onLeaveSwitchControlRange;
        //Electro_PlayerMovement.TranslationFinish += onPlayerTranslationFinish;
        Electro_Camera_Controller.TranslateCameraFinish += onTranslateCameraFinish;
        Electro_Camera_Controller.ResetCameraFinish += onResetCameraFinish;

    }
    private void OnDisable()
    {
        Electro_CollideChecker.EnterStarRange -= onEnterStarRange;
        Electro_CollideChecker.LeaveStarRange -= onExitStarRange;
        Electro_Camera_Controller.TranslateCameraFinish -= onTranslateCameraFinish;
        Electro_Camera_Controller.ResetCameraFinish     -= onResetCameraFinish;

    }

    private void onTranslateCameraFinish()
    {
        currentStatus = Electro_PlayStatus.InPuzzle;
        myStarPuzzleController.setInteractionEnabled(true);
    }

    private void onResetCameraFinish()
    {
        currentStatus = Electro_PlayStatus.Wandering;
        myStarPuzzleController.setInteractionEnabled(false);

    }

    //private void onPlayerTranslationFinish()
    //{
    //    currentStatus = Electro_PlayStatus.InPuzzle;

    //}


    //private void onEnterSwitchControlRange()
    //{
    //    currentStatus = Electro_PlayStatus.InPuzzle;
    //    myCameraController.showStarCam();
    //    //myPlayerMovement.TranslateTo(playerTargetPosStarPuzzle);
    //}

    //private void onLeaveSwitchControlRange()
    //{
    //    currentStatus = Electro_PlayStatus.InPuzzle;
    //    myCameraController.showStarCam();
    //    //myPlayerMovement.TranslateTo(playerTargetPosStarPuzzle);
    //}

    private void onEnterStarRange()
    {
        currentStatus = Electro_PlayStatus.InTranslation;
        myCameraController.showStarCam();
        myPlayerMovement.TranslateTo(playerTargetPosStarPuzzle);
    }

    private void onExitStarRange()
    {
        currentStatus = Electro_PlayStatus.InTranslation;
        myCameraController.resetCam();
    }




    // Update is called once per frame
    void Update()
    {
        if (currentStatus == Electro_PlayStatus.Wandering)
        {
            myPlayerMovement.OnUpdate();
            Vector3 playerMovementDirection = myPlayerMovement.getMovementDirection();
            myCameraController.onUpdateCameraWithPlayerMovement(playerMovementDirection);
        }
        else if (currentStatus == Electro_PlayStatus.InPuzzle)
        {
            myPlayerMovement.OnUpdate();
        }

    }
}