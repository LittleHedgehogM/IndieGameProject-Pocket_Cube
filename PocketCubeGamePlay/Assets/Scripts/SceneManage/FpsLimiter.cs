using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FpsLimiter : MonoBehaviour
{
    public enum LimitType
    {
        Nolimit = -1,
        Limit30 = 30,
        Limit60 = 60,
        Limit120 = 120
    }

    public LimitType Limit;

    private void Awake()
    {
        Application.targetFrameRate = (int)Limit;
    }
}
