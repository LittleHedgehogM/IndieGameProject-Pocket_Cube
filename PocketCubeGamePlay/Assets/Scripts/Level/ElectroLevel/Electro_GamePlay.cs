using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Electro_GamePlay : MonoBehaviour
{
    Electro_Camera_Controller myCameraController;
    Electro_PlayerMovement myPlayerMovement;

    [SerializeField] private Electro_Puzzle starPuzzle;
    [SerializeField] private Electro_SunPuzzle sunPuzzle;
    [SerializeField] private Electro_MoonPuzzle moonPuzzle;
    enum Electro_PlayStatus
    {
        NoPuzzle,// wandering
        PuzzleStar,
        PuzzleSun,
        PuzzleMoon,
        PuzzleFinish
    }

    Electro_PlayStatus currentStatus;

    // Start is called before the first frame update
    void Start()
    {
        myCameraController  = FindObjectOfType<Electro_Camera_Controller>();
        myPlayerMovement    = FindObjectOfType<Electro_PlayerMovement>();
        currentStatus       = Electro_PlayStatus.NoPuzzle;
        starPuzzle.Init();
        sunPuzzle.Init();
        moonPuzzle.Init();
    }

    //private void OnEnable()
    //{

    //    Electro_Camera_Controller.TranslateCameraFinish += onTranslateCameraFinish;
    //    Electro_Camera_Controller.ResetCameraFinish += onResetCameraFinish;

    //}
    //private void OnDisable()
    //{
    //    Electro_Camera_Controller.TranslateCameraFinish -= onTranslateCameraFinish;
    //    Electro_Camera_Controller.ResetCameraFinish -= onResetCameraFinish;

    //}

    //private void onTranslateCameraFinish()
    //{
    //    //currentStatus = Electro_PlayStatus.NoPuzzle;
    //    //myStarPuzzleController.setInteractionEnabled(true);
    //}

    //private void onResetCameraFinish()
    //{
    //    //currentStatus = Electro_PlayStatus.NoPuzzle;
    //    //myStarPuzzleController.setInteractionEnabled(false);

    //}

    //private void onEnterStarRange()
    //{
    //    //currentStatus = Electro_PlayStatus.InTranslation;
    //    myCameraController.showStarCam();
    //   // myPlayerMovement.TranslateTo(playerTargetPosStarPuzzle);
    //}

    //private void onExitStarRange()
    //{
    //    //currentStatus = Electro_PlayStatus.InTranslation;
    //    myCameraController.resetCam();
    //}

    // Update is called once per frame
    void Update()
    {
        //if (currentStatus == Electro_PlayStatus.Wandering)
        //{
        myPlayerMovement.OnUpdate();
        starPuzzle.UpdatePuzzle();
        sunPuzzle.UpdatePuzzle();
        moonPuzzle.UpdatePuzzle();
        //Vector3 playerMovementDirection = myPlayerMovement.getMovementDirection();
        //myCameraController.onUpdateCameraWithPlayerMovement(playerMovementDirection);
        //}
        //else if (currentStatus == Electro_PlayStatus.InPuzzle)
        //{
        //    myPlayerMovement.OnUpdate();
        //}
        //switch (currentStatus)
        //{
        //    case Electro_PlayStatus.PuzzleStar:
        //        {
        //            starPuzzle.UpdatePuzzle();
        //            if (starPuzzle.isPuzzleSolved())
        //            {
        //                currentStatus = Electro_PlayStatus.PuzzleSun;
        //            }
        //            break;
        //        }
        //}
    }
}
