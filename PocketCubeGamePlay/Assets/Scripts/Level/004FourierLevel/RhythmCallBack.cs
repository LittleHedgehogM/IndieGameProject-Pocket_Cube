using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RhythmCallBack : MonoBehaviour
{
    public static Action<int> Rhythm_Bar;
    private int beatCount = 0;
    //private bool pushTransitions = false;

    public void Push_Rhythm_Bar()
    {
        if (beatCount == 4)
        {
            beatCount = 0;
        }
        beatCount++;
        //print(beatCount);

        Rhythm_Bar?.Invoke(beatCount);
        //pushTransitions = true;
        //print("transition On");
    }
}
