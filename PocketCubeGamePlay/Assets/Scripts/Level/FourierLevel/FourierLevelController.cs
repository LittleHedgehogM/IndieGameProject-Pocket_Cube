using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FourierLevelController : MonoBehaviour
{
    public float lerpTime = 0.8f;
    //[HideInInspector]public bool pushTransitions = false;
    //FourierColorChanger _FCC;

    private void Start()
    {
        //FourierColorChanger[] _FCC = GetComponentsInChildren<FourierColorChanger>();
    }
    public void ModeOne()
    {
        
        FourierColorChanger.transitionOnStart = 0;

    }

    public void ModeTwo()
    {
        FourierColorChanger.transitionOnStart = 1;
    }

    public void ModeThree()
    {
        FourierColorChanger.transitionOnStart = 2;
    }

    /*public void PushTransitions()
    {
        pushTransitions = true;
        
        //print("transition On");
    }*/
}
