using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Origin_Axis : MonoBehaviour
{

    [System.Serializable]
    enum Axis{
        left,
        right, 
        up,
        down,
    }

    [SerializeField] Axis axis;
    [SerializeField] Animator animator;

    public static event Action LeftAxisClicked;
    public static event Action RightAxisClicked;
    public static event Action UpAxisClicked;
    public static event Action DownAxisClicked;

    [SerializeField] private Color colorSelected;
    Origin_CursorController myCursorController;

    
    private float edgeLength = 0.003f;
    private GameObject go;
    private Material material;
    bool isInteractable = true;
    public void setActive(bool isActive)
    {
        this.gameObject.SetActive(isActive);
    }

    private void OnEnable()
    {
        Origin_Controller.DisableAllAxis += setNotInteractable;
        Origin_Controller.rotateFinish += setInteractable;
    }

    private void OnDisable()
    {
        Origin_Controller.DisableAllAxis -= setNotInteractable;
        Origin_Controller.rotateFinish -= setInteractable;

    }

    public void setNotInteractable()
    {
        isInteractable = false;
    }

    public void setInteractable()
    {
        isInteractable = true;
    }

    void Start()
    {
        go = this.gameObject;
        material = go.GetComponent<MeshRenderer>().material;
        material.SetColor("_diffusegradient01", Color.white);
        material.SetFloat("_OutlineWidth", 0);

        myCursorController = FindObjectOfType<Origin_CursorController>();
    }

    private void OnMouseEnter()
    {
        myCursorController.setHoverCursor();
        material.SetColor("_diffusegradient01", colorSelected);
        material.SetFloat("_OutlineWidth", edgeLength);

    }

    private void OnMouseExit()
    {
        myCursorController.setNormalCursor();
        material.SetColor("_diffusegradient01", Color.white);
        material.SetFloat("_OutlineWidth", 0);
    }

    private void OnMouseUp()
    {
        if (!isInteractable) {
            return;
        }

        switch (axis) 
        {
            case Axis.left:{
                 Debug.Log("Left Axis Hit");
                 LeftAxisClicked?.Invoke();
                 
                 break;
            }
            case Axis.right:{
                 Debug.Log("Right Axis Hit");

                 RightAxisClicked?.Invoke();
                 break;
            }
            case Axis.up:{
                 Debug.Log("Up Axis Hit");

                 UpAxisClicked?.Invoke();
                 break;
            }
            case Axis.down:{
                Debug.Log("Down Axis Hit");

                 DownAxisClicked?.Invoke();
                 break;
            }
                
        }
        isInteractable = false;
    }

}
