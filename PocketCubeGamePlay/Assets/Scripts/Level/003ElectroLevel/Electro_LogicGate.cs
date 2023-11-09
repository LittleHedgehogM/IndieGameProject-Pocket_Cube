using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Electro_LogicGate : MonoBehaviour
{
    Electro_CursorController myCursorController;
    bool isInteractionEnabled = false;

    private void Start()
    {
        myCursorController  = FindObjectOfType<Electro_CursorController>();
    }

    private void OnMouseEnter()
    {
        if (isInteractionEnabled)
        {
            myCursorController.setSelectCursor();
        }
    }

    private void OnMouseExit()
    {

        myCursorController.setDefaultCursor();

    }
    private void OnMouseDown()
    {
        if (isInteractionEnabled)
        {
            myCursorController.setClickDownCursor();
        }
    }

    private void OnMouseUp()
    {
        if (isInteractionEnabled)
        {
            myCursorController.setSelectCursor();
        }

    }

    public void setInteractionEnabled(bool isEnabled)
    {
        isInteractionEnabled = isEnabled;
    }
}
