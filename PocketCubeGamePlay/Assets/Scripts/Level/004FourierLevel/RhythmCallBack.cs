using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RhythmCallBack : MonoBehaviour
{
    public static Action<int> Rhythm_Bar;
    public static Action<int> Rhythm_Beat;
    private int beatCount = 3;
    private int musicBeat = 0;
    //private bool pushTransitions = false;

    public void Push_Rhythm_Bar()
    {
        
        if (musicBeat == 4)
        {
            musicBeat = 0;
        }
        musicBeat ++;
        //print("playBeat" + beatCount);
        print("MusicBeat"+ musicBeat);

        Rhythm_Bar?.Invoke(musicBeat);
        //pushTransitions = true;
        //print("transition On");
    }

    public void Push_Rhythm_Beat()
    {
        if (beatCount == 12)
        {
            beatCount = 0;
        }
        beatCount++;
        print("playBeat" + beatCount);
        Rhythm_Beat?.Invoke(beatCount);
    }
}
