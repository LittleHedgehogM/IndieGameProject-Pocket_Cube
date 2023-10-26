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


    private float edgeLength = 0.003f;
    private GameObject go;
    private Material material;
    bool isCurrentAxisActive = false;

    public void setActive(bool isActive)
    {
        this.gameObject.SetActive(isActive);
    }

    public GameObject getGameObject()
    {
        return go;
    }

    void Start()
    {
        go = this.gameObject;
        material = go.GetComponent<MeshRenderer>().material;
        material.SetColor("_diffusegradient01", Color.white);
        material.SetFloat("_OutlineWidth", 0);
    }

    private void OnMouseEnter()
    {
        material.SetColor("_diffusegradient01", Color.gray);
        material.SetFloat("_OutlineWidth", edgeLength);

    }

    private void OnMouseExit()
    {
        //material.SetFloat("_BaseCellOffset002", 0.5f);
        material.SetColor("_diffusegradient01", Color.white);
        material.SetFloat("_OutlineWidth", 0);

    }

    private void OnMouseUp()
    {
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
    }

}
