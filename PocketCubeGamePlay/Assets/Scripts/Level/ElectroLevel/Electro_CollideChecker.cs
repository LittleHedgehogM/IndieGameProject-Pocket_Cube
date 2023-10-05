using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Electro_CollideChecker : MonoBehaviour
{
    public static event Action EnterStarRange;
    public static event Action LeaveStarRange;
    //public static event Action EnterSwitchControlPos;
    //public static event Action LeaveSwitchControlPos;
    //Electro_PlayerMovement myPlayerMovement;

    private void Start()
    {
        //myPlayerMovement = FindObjectOfType<Electro_PlayerMovement>();

    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Area" && collision.gameObject.name == "StarRange")
        {
            Debug.Log("Entered collision with Star Range");
            EnterStarRange?.Invoke();
        }
        //else if (collision.gameObject.tag == "SwitchControlPos")
        //{
        //    //EnterSwitchControlPos?.Invoke();
        //    // translate to 
        //    myPlayerMovement.TranslateTo(collision.gameObject.transform);
        //}
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.tag == "Area" && collision.gameObject.name == "StarRange")
        {
            Debug.Log("Leave collision with Star Range");
            LeaveStarRange?.Invoke();
        }
        //else if (collision.gameObject.tag == "SwitchControlPos")
        //{

        //    LeaveSwitchControlPos?.Invoke();
        //    // translate to 
        //}
    }

   
}
