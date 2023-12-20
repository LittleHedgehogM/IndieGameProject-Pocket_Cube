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


    private void Awake()
    {
        vp.Play();
        vp.loopPointReached += ActionAfterVideoPlayed;
        PlayerPrefs.SetInt("Level", 4);
        ACHIEVEMENT_01?.Invoke("ACHIEVEMENT_01");
    }
    

    

    private void ActionAfterVideoPlayed(VideoPlayer vp)
    {
        SceneManager.LoadScene("StartGame");
        ACHIEVEMENT_06?.Invoke("ACHIEVEMENT_06");
    }


}
