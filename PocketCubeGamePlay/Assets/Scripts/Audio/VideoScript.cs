using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class VideoScript: MonoBehaviour
{
    private VideoPlayer vid;
    [SerializeField] AK.Wwise.Event sound;
    //private bool isOpeningPlayed = false;
    public static Action VideoFinished;
    [SerializeField] GameObject performCamera;
    [SerializeField] float translationTime = 1f;
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
        if (SceneManager.GetActiveScene().name == "First GPP")
        {
            gameObject.SetActive(false);
            performCamera.SetActive(true);
            VideoFinished?.Invoke();
        }
        else if (SceneManager.GetActiveScene().name == "NewtonLevel_GPP_Test")
        {
            StartCoroutine(VideoFadeout());
        }
        
    }

    private IEnumerator VideoFadeout()
    {
        float startAlphaVal = 1;
        float targetAlphaVal = 0;
        float currentTime = 0;
        
        performCamera.SetActive(true);
        vid.targetCamera = performCamera.GetComponent<Camera>();
        while (vid.targetCameraAlpha > 0)
        {
            currentTime += Time.deltaTime;
            
            vid.targetCameraAlpha = Mathf.Lerp(startAlphaVal, targetAlphaVal, translationTime * currentTime);
            yield return null;
        }
        //finishImage.gameObject.SetActive(false);
        gameObject.SetActive(false);
        VideoFinished?.Invoke();
        yield return null;
    }
}
