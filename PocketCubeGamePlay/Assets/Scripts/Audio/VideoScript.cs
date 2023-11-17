using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class VideoScript: MonoBehaviour
{
    private VideoPlayer vid;
    [SerializeField] AK.Wwise.Event sound;
    //private bool isOpeningPlayed = false;
    public static Action VideoFinished;

    void Start()
    {
        vid = GetComponent<VideoPlayer>();
        vid.Play();
        sound.Post(gameObject);
        vid.loopPointReached += DestroyAfterVideoPlayed;
    }

    // Update is called once per frame
    void DestroyAfterVideoPlayed(VideoPlayer vp)
    {
        gameObject.SetActive(false);
        VideoFinished?.Invoke();
        // play camera 
    }
}
