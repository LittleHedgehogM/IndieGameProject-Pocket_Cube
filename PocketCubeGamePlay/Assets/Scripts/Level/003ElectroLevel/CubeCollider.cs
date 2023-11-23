using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CubeCollider : CubeClickEvent
{

    bool isEnabled = false;
    Electro_CursorController myCursorController;

    private void Start()
    {
        myCursorController = FindObjectOfType<Electro_CursorController>();
    }

    private void OnEnable()
    {
        Electro_CubeController.onCubeEnabled += enableCubeCollide;
    }

    private void OnDisable()
    {
        Electro_CubeController.onCubeEnabled -= enableCubeCollide;

    }

    public void enableCubeCollide(){
       isEnabled = true;
    }

    private void OnMouseEnter()
    {
        if (isEnabled)
        { 
            myCursorController.setSelectCursor();
        }
    }

    private void OnMouseExit()
    {
        if (isEnabled)
        {
            myCursorController.setDefaultCursor();
        }

    }
    private void OnMouseDown()
    {
        if (isEnabled)
        {
            myCursorController.setClickDownCursor();
        }
    }

    // Start is called before the first frame update
    private void OnMouseUp()
    {
        
        if (isEnabled)
        {
            myCursorController.setSelectCursor();
            CubeClick?.Invoke();
            isEnabled = false;
        }
    }
}
