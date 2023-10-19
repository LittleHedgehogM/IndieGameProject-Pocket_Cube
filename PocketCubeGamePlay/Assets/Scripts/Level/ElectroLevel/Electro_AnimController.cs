using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Electro_AnimController : MonoBehaviour
{
    public static event Action StarLeftCircuitAnimFinished;
    public static event Action StarRightCircuitAnimFinished;
    public static event Action StarCenterCircuitAnimFinished;

    public static event Action SunLeftCircuitAnimFinished;
    public static event Action SunRightCircuitAnimFinished;
    public static event Action SunCenterCircuitAnimFinished;


    public static event Action MoonLeftCircuitAnimFinished;
    public static event Action MoonRightCircuitAnimFinished;
    public static event Action MoonCenterCircuitAnimFinished;


    public static event Action SunPuzzleSolved;
    public static event Action MoonPuzzleSolved;
    public static event Action StarPuzzleSolved;

    //==================================
    public void StarLeftAnimDone()
    {
        StarLeftCircuitAnimFinished?.Invoke();
    }

    public void StarRightAnimDone()
    {
        StarRightCircuitAnimFinished?.Invoke();
    }

    public void StarCenterAnimDone()
    {
        StarCenterCircuitAnimFinished?.Invoke();
    }

    //==================================
    public void SunLeftAnimDone()
    {
        SunLeftCircuitAnimFinished?.Invoke();
    }

    public void SunRightAnimDone()
    {
        SunRightCircuitAnimFinished?.Invoke();
    }

    public void SunCenterAnimDone()
    {
        SunCenterCircuitAnimFinished?.Invoke();
    }

    //==================================
    public void MoonLeftAnimDone()
    {
        MoonLeftCircuitAnimFinished?.Invoke();
    }

    public void MoonRightAnimDone()
    {
        MoonRightCircuitAnimFinished?.Invoke();
    }

    public void MoonCenterAnimDone()
    {
        MoonCenterCircuitAnimFinished?.Invoke();
    }


    //====================================
    public void SetSunPuzzleSolved()
    {
        SunPuzzleSolved?.Invoke();
    }

    public void SetMoonPuzzleSolved()
    {
        MoonPuzzleSolved?.Invoke();
    }

    public void SetStarPuzzleSolved()
    {
        StarPuzzleSolved.Invoke();
    }

}