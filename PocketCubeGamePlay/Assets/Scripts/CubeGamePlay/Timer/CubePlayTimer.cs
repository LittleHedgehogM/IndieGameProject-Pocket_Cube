using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubePlayTimer : MonoBehaviour
{

    private float cubePlayTime;

    private void Start()
    {
        cubePlayTime = -1;
    }

    public void startTimer()
    { 
        if (cubePlayTime==-1) 
        { 
            cubePlayTime=0;
        }
    
    }

    public void UpdateTimer()
    {
        if (cubePlayTime >=0)
        {
            cubePlayTime += Time.deltaTime;
        }
    }

    private float SecToMin(float playTime)
    {
        if (playTime >= 0) 
        {
            return playTime / 60.0f;
        }
        return 0;
    }

    public bool isSolveMinutesMoreThan(float minutes)
    {
        return SecToMin(cubePlayTime) > minutes;
    }

    public bool isSolveMinutesLessThan(float minutes)
    {
        return SecToMin(cubePlayTime) <= minutes;
    }
}
