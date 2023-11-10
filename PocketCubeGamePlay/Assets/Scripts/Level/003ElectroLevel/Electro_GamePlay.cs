
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Json;
using TMPro.Examples;
using UnityEngine;

public class Electro_GamePlay : MonoBehaviour
{
    Electro_Camera_Controller myCameraController;
    Electro_PlayerMovement myPlayerMovement;

    [SerializeField] private Electro_Puzzle starPuzzle;
    [SerializeField] private Electro_SunPuzzle sunPuzzle;
    [SerializeField] private Electro_MoonPuzzle moonPuzzle;
    [SerializeField] private Animator centerAnimator;
    [SerializeField] private Animator cubeAnimator;
    [SerializeField] private Electro_MeshControl rightWallMesh;
    [SerializeField] private Electro_MeshControl leftWallMesh;
    [SerializeField] private Electro_MeshControl centerMesh;

    private bool isScenePuzzleSolved;

    void Start()
    {
        myCameraController  = FindObjectOfType<Electro_Camera_Controller>();
        myPlayerMovement    = FindObjectOfType<Electro_PlayerMovement>();
        starPuzzle.Init();
        sunPuzzle.Init();
        moonPuzzle.Init();
        isScenePuzzleSolved = false;
    }


    private void OnEnable()
    {
        Electro_Camera_Controller.ResetCameraFinish += onResetCameraFinish;
    }

    private void OnDisable()
    {
        Electro_Camera_Controller.ResetCameraFinish -= onResetCameraFinish;

    }

    private void onResetCameraFinish()
    {
        if (isScenePuzzleSolved) 
        {
            myPlayerMovement.setEnableMovement(true);
        }
    }

    void Update()
    {
        if (!isScenePuzzleSolved)
        {                      
            if (starPuzzle.getIsPuzzleSolved() && sunPuzzle.getIsPuzzleSolved() && moonPuzzle.getIsPuzzleSolved())
            {
                  rightWallMesh.Show();
                  leftWallMesh.Show();
                  centerMesh.Show();
                  starPuzzle.setNotInteractable();
                  sunPuzzle.setNotInteractable();
                  moonPuzzle.setNotInteractable();
                  myCameraController.resetCam();
                  isScenePuzzleSolved = true;
                  centerAnimator.Play("AM_Center_Normal");
                  cubeAnimator.Play("AM_CenterCube_Finsh");
                  return;
            }
            else 
            {
                 myPlayerMovement.OnUpdate();
                 if (!starPuzzle.isInPuzzle() && !sunPuzzle.isInPuzzle() && !moonPuzzle.isInPuzzle())
                 {
                    Vector3 playerMovementVector = myPlayerMovement.getMovementDirection();
                    myCameraController.onUpdateCameraWithPlayerMovement(playerMovementVector);
                 } 
                 starPuzzle.UpdatePuzzle();
                 sunPuzzle.UpdatePuzzle();
                 moonPuzzle.UpdatePuzzle();
            }

        }
        else 
        {
            myPlayerMovement.OnUpdate();
            Vector3 playerMovementVector = myPlayerMovement.getMovementDirection();
            myCameraController.onUpdateCameraWithPlayerMovement(playerMovementVector);
        }

    }


}
