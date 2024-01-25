using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class EndingVideo : MonoBehaviour
{
    [SerializeField] VideoPlayer vp;

    public static Action<string> ACHIEVEMENT_01;
    public static Action<string> ACHIEVEMENT_06;
    //setting btn ctl
    public static Action EndingVideoStart;
    private bool vidPause = false;



    private void Awake()
    {
        vp.Play();
        vp.loopPointReached += ActionAfterVideoPlayed;
        PlayerPrefs.SetInt("Level", 4);
        ACHIEVEMENT_01?.Invoke("ACHIEVEMENT_01");
        EndingVideoStart?.Invoke();
    }
    

    

    private void ActionAfterVideoPlayed(VideoPlayer vp)
    {
        SceneManager.LoadScene("StartGame");
        ACHIEVEMENT_06?.Invoke("ACHIEVEMENT_06");
    }

    void OnApplicationFocus(bool isFocus)
    {
        if (isFocus)
        {
            if (vidPause)
            {
                vidPause = false;
                vp.Play();
                AkSoundEngine.PostEvent("Resume", gameObject);
                Debug.Log("游戏开始");
            }
        }
        else
        {
            vp.Pause();
            AkSoundEngine.PostEvent("Pause", gameObject);
            vidPause = true;
            Debug.Log("游戏暂停");
        }
    }

}
