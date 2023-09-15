using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchController : MonoBehaviour
{
    GameObject go;
    Material[] materials;
    [SerializeField] private GameObject player;

    [SerializeField] Color Color_On;
    [SerializeField] Color InCollisionColor;
    [SerializeField] Color Color_Off;

    [SerializeField]  private bool isSwitchOn;
    public static event Action onSwitchChanged;


    void Start()
    {
        go = this.gameObject;
        //material = go.GetComponent<MeshRenderer>().material;
        materials = go.GetComponent<MeshRenderer>().materials;
        ResetMaterial();
    }


    public void ResetMaterial()
    {
        foreach (Material mat in materials)
        {
            if(isSwitchOn) 
            {
                mat.SetColor("_Color", Color_On);
            }
            else
            {
                mat.SetColor("_Color", Color_Off);
            }
            
        }

    }

    public bool getIsSwitchOn()
    {
        return isSwitchOn;
    }

    // Gets called at the start of the collision 
    void OnCollisionEnter(Collision collision)
    {        
        if (player && collision.gameObject == player)
        {
            Debug.Log("Entered collision with player");
        }

    }

    // Gets called during the collision
    void OnCollisionStay(Collision collision)
    {
        if (player && collision.gameObject == player)
        {
            Debug.Log("In collision with player");
        }
    }

    // Gets called when the object exits the collision
    void OnCollisionExit(Collision collision)
    {


        if (player && collision.gameObject == player)
        {
            Debug.Log("Exit collision with player");
            isSwitchOn = !isSwitchOn;
            foreach (Material mat in materials)
            {
                if (isSwitchOn)
                {
                    mat.SetColor("_Color", Color_On);
                }
                else
                {
                    mat.SetColor("_Color", Color_Off);
                }

            }
            onSwitchChanged?.Invoke();
        }
    }
}
