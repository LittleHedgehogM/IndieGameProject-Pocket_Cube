using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FourierKeyCube : CubeClickEvent
{
    //[SerializeField] private Vector3 _rotation;
    //[SerializeField] private float _speed;

    //[SerializeField] private bool levelPass = false;
    //
    //[SerializeField] private ParticleSystem cubeFx;

    //private bool played = false;


    FourierCursorController myCursorController;
    public static Action CubeShow;
    [SerializeField] Material passMat;

    private void Start()
    {
        gameObject.GetComponent<BoxCollider>().enabled = false;
        myCursorController = FindObjectOfType<FourierCursorController>();
    }

    private void OnEnable()
    {
        FourierLevelManager.CubeUnlocked += FourierPass;
    }

    private void FourierPass()
    {
        CubeShow?.Invoke();
        gameObject.GetComponent<Renderer>().material = passMat;
        gameObject.GetComponent<BoxCollider>().enabled = true;
    }
    /*private void CubeAutoRotate()
    {
        //transform.Rotate(_rotation * _speed * Time.deltaTime);
        transform.Rotate(_rotation * _speed * Time.deltaTime, Space.World);
        if (!played)
        {
            CubeShow?.Invoke();
            cubeFx.Play();
            played = true;
        }

        
    }*/

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
    }
    
}
