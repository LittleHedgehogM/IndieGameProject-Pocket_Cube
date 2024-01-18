using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FourierKeyCube : CubeClickEvent
{

    FourierCursorController myCursorController;
    public static Action CubeShow;
    [SerializeField] Material passMat;
    private GameObject audioPlayer;

    private void Awake()
    {
        audioPlayer = GameObject.Find("WwiseFourier");
    }

    private void Start()
    {
        gameObject.GetComponent<BoxCollider>().enabled = false;
        myCursorController = FindObjectOfType<FourierCursorController>();
    }

    private void OnEnable()
    {
        FourierLevelManager.CubeUnlocked += FourierPass;
    }

    private void OnDisable()
    {
        FourierLevelManager.CubeUnlocked -= FourierPass;
    }

    private void FourierPass()
    {
        CubeShow?.Invoke();
        gameObject.GetComponent<Renderer>().material = passMat;
        gameObject.GetComponent<BoxCollider>().enabled = true;
    }

    private void OnMouseEnter()
    {      
        myCursorController.setSelectCursor();
    }

    private void OnMouseExit()
    {       
        myCursorController.setDefaultCursor();
    }

    private void OnMouseDown()
    {      
            myCursorController.setClickDownCursor();    
    }
    private void OnMouseUp()
    {       
            myCursorController.setSelectCursor();
            //FindObjectOfType<LevelLoaderScript>().LoadNextLevel();
            CubeClick?.Invoke();
        AkSoundEngine.SetSwitch("Fourier_Level", "none_beat", audioPlayer);
    }
    
}
