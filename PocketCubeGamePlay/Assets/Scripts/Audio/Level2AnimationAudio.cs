using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level2AnimationAudio : MonoBehaviour
{
    //public AK.Wwise.Event Star_Middle;
    //public GameObject AudioPlayer;

    void Side_LeftRight_Play()
    {
        AkSoundEngine.PostEvent("Play_Side_Unlock01", gameObject);
    }

    void Side_Middle_Play()
    {
        AkSoundEngine.PostEvent("Play_Side_Unlock02", gameObject);
    }

    void Light_Play()
    {
        AkSoundEngine.PostEvent("Play_Shine", gameObject);
    }

    void Mid_Play()
    {
        //print("Play_Mid_Unlock");
        AkSoundEngine.PostEvent("Play_Mid_Unlock", gameObject);
    }

    void Cube_Unlock()
    {
        AkSoundEngine.PostEvent("Play_Final", gameObject);
    }

    void PlayWalkLoop()
    {
        AkSoundEngine.PostEvent("Play_Move_Loop", gameObject);
    }

    void StopPlayWalkLoop()
    {
        AkSoundEngine.PostEvent("Stop_Move_Loop", gameObject);
    }
}
