using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using static CubeInPlayPhase;

public class CubePlayManager : MonoBehaviour
{

    public static CubePlayManager instance;

    [SerializeField] private int frameRate = 30;
    [SerializeField]
    public GameObject pocketCube;

    public enum CubePlay
    {
        Configuration,
        Play,
        Solved
    }

    CubePlay currentCubePlayPhase;
    CubeConfigurationPhase myCubeConfigurationPhase;
    CubeInPlayPhase myCubeInPlayPhase;
    CubeSolvedPhase myCubeSolvedPhase;
    CubePlayUIController myCubeUIController;
    CubePlayTimer myTimer;
    public static Action RestartCubeGame;
    public static Action SolveCubeWithinOneMins;
    public static Action UnsolveCubeAfterEightMinutes;
    public static Action OnlyUseSkillsToSolve;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }

        myCubeConfigurationPhase = FindObjectOfType<CubeConfigurationPhase>();
        myCubeInPlayPhase = FindObjectOfType<CubeInPlayPhase>();
        myCubeSolvedPhase = FindObjectOfType<CubeSolvedPhase>();
        myCubeUIController = FindObjectOfType<CubePlayUIController>();
        currentCubePlayPhase = CubePlay.Configuration;
        myCubeConfigurationPhase.onStart();
        myTimer = FindObjectOfType<CubePlayTimer>();
        Application.targetFrameRate = frameRate;

    }


    public bool CanUseSkill() {

        return currentCubePlayPhase == CubePlay.Play
            && myCubeInPlayPhase.GetCubePlayStatus() == CubePlayStatus.WaitForInput;
    }

    public bool isApplyingDiagonal()
    {
        return currentCubePlayPhase == CubePlay.Play
            && myCubeInPlayPhase.GetCubePlayStatus() == CubePlayStatus.InDiagonal;
    }
    public bool isApplyingCommutation(){
        return currentCubePlayPhase == CubePlay.Play
            && myCubeInPlayPhase.GetCubePlayStatus() == CubePlayStatus.InCommutation;
    }


    public bool CanRestoreDiagonalSkill() {
        return currentCubePlayPhase == CubePlay.Play
               && myCubeInPlayPhase.CanRestoreDiagional();
    }

    public bool CanRestoreCommutationSkill()
    {
        return currentCubePlayPhase == CubePlay.Play
               && myCubeInPlayPhase.CanRestoreCommuation();
    }

    public bool isConfigurationPhase()
    {
        return currentCubePlayPhase == CubePlay.Configuration;
    }

    private void OnEnable()
    {
        CubePlayUIController.onEnterDiagonalState += SetStateToDiagonal;
        CubePlayUIController.onEnterCommutationState += SetStateToCommutation;
        CubePlayUIController.onRestoreCommutationCheckPoint += RestoreCommutationCheckPoint;
        CubePlayUIController.onRestoreDiagonalCheckPoint += RestoreDiagonalCheckPoint;
        ButtonClick.OnMyCallback += HandleCallback;

    }


    private void OnDisable()
    {
        CubePlayUIController.onEnterDiagonalState -= SetStateToDiagonal;
        CubePlayUIController.onEnterCommutationState -= SetStateToCommutation;
        CubePlayUIController.onRestoreCommutationCheckPoint -= RestoreCommutationCheckPoint;
        CubePlayUIController.onRestoreDiagonalCheckPoint -= RestoreDiagonalCheckPoint;
        ButtonClick.OnMyCallback -= HandleCallback;

    }

    void HandleCallback(string message)
    {
        // if showing tutorial panel
        if (currentCubePlayPhase == CubePlay.Configuration ) 
        {
            return;
        }       
        
        if (message.Contains("Restart"))
        {
            onRestart();
        }                                              
        else if (message.Contains("Diagonal"))
        {
            myCubeUIController.clickDiagonalButton();
        }
        else if (message.Contains("Commutation"))
        {
            myCubeUIController.clickCommutationButton();
        }
        else if (message.Contains("ResetCamera"))
        {
            SetStateToResetCamera();
        }

    }


    private void RestoreCommutationCheckPoint()
    {
        if (currentCubePlayPhase == CubePlay.Play)
        {
            myCubeInPlayPhase.RestoreCommutationCheckPoint();

        }

    }

    private void RestoreDiagonalCheckPoint()
    {
        if (currentCubePlayPhase == CubePlay.Play)
        {
            myCubeInPlayPhase.RestoreDiagonalCheckPoint();
        }
    }

    public void SetStateToResetCamera()
    {
        if (currentCubePlayPhase == CubePlay.Play)
        {
           myCubeInPlayPhase.SetStateToResetCamera();
        }
    }

    public void SetStateToCommutation()
    {
        if (currentCubePlayPhase == CubePlay.Play) 
        {
            //store current state
            myCubeInPlayPhase.SetStateToCommutation(); 
        }
    }

    public void SetStateToDiagonal()
    {
        if (currentCubePlayPhase == CubePlay.Play)
        {
            //store current state
            myCubeInPlayPhase.SetStateToDiagonal();
        }
    }

    public void onRestart()
    {
        if (currentCubePlayPhase == CubePlay.Play && myCubeInPlayPhase.canRestart())
        {
            currentCubePlayPhase = CubePlay.Configuration;
            myCubeConfigurationPhase.onRestart();
            myCubeInPlayPhase.onRestart();
            myCubeSolvedPhase.onRestart();
            RestartCubeGame?.Invoke();
        }

    }


    void Update()
    {

        if (currentCubePlayPhase == CubePlay.Configuration)
        {
            bool isConfigurationFinished = myCubeConfigurationPhase.onUpdate();
            if (isConfigurationFinished)
            {
                currentCubePlayPhase = CubePlay.Play;
                myCubeConfigurationPhase.onEnd();
                myCubeInPlayPhase.onStart();
                myTimer.startTimer();
            }
        }
        else if (currentCubePlayPhase == CubePlay.Play)
        {
            bool isCubePlayFinished = myCubeInPlayPhase.onUpdate();
            myTimer.UpdateTimer();
            if (isCubePlayFinished)
            {
                currentCubePlayPhase = CubePlay.Solved;
                myCubeInPlayPhase.onEnd();
                myCubeSolvedPhase.onStart();
                // check timer
                if (myTimer.isSolveMinutesLessThan(1))
                {
                    SolveCubeWithinOneMins?.Invoke();
                }
                else if (myTimer.isSolveMinutesMoreThan(8)) 
                {
                    UnsolveCubeAfterEightMinutes?.Invoke();
                }
                if (myCubeUIController.getCurrentSwipeSteps()==0 && 
                myCubeUIController.getIsCommutationApplied() && myCubeUIController.getIsDiagonalApplied())
                {
                    OnlyUseSkillsToSolve?.Invoke();
                }
               
            }
        }
        else if (currentCubePlayPhase == CubePlay.Solved)
        {
            bool isFinished = myCubeSolvedPhase.onUpdate();
            if  (isFinished)
            {
                myCubeSolvedPhase.onEnd();
            }

        }

    }


}