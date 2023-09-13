using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FourierLevelController : MonoBehaviour
{
    public GameObject jumpFX;
    

    private void Start()
    {
        
    }
    public void ModeOne()
    {
        
        FourierColorChanger.transitionOnStart = true;

    }

    public void ModeTwo()
    {
        FourierColorChanger.transitionOnStart = false;
    }
}
