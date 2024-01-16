using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RhythmCallBack : MonoBehaviour
{
    public static Action<int> Rhythm_Bar;
    public static Action Rhythm_Beat;
    private int beat = 0;
    //private bool pushTransitions = false;
    private int musicBeat = 0;
    public void Push_Rhythm_Bar()
    {
        
        if (musicBeat == 4)
        {
            musicBeat = 0;
        }
        musicBeat ++;

        //print("MusicBeat"+ musicBeat);

        Rhythm_Bar?.Invoke(musicBeat);

    }

    public void Push_Rhythm_Beat()
    {
        //print("playBeat" + beat);
        Rhythm_Beat?.Invoke();
        if (PlayPointBehaviour.inLevel == 2)
        {
            beat++;
            print(beat);
            if (beat == 5)
            {
                beat = 0;
            }
        }
        
    }
}
