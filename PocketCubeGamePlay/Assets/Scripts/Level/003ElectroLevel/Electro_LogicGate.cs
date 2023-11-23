using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Electro_LogicGate : MonoBehaviour
{
    Electro_CursorController myCursorController;
    bool isInteractionEnabled = false;
    Vector3 scale = Vector3.one;
    Vector3 smallScale = Vector3.one;
    private void Start()
    {
        myCursorController  = FindObjectOfType<Electro_CursorController>();
        scale = this.transform.localScale;
        smallScale = scale * 0.9f;
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
            this.transform.localScale = smallScale;
        }
    }

    private void OnMouseUp()
    {
        if (isInteractionEnabled)
        {
            myCursorController.setSelectCursor();
            this.transform.localScale = scale;


        }

    }

    public void setInteractionEnabled(bool isEnabled)
    {
        isInteractionEnabled = isEnabled;
    }
}
