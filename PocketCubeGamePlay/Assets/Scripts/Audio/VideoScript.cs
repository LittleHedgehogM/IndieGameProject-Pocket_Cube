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
    private CanvasGroup settingBtn;

    private void Awake()
    {
        settingBtn = GameObject.FindWithTag("SettingBtn").GetComponent<CanvasGroup>();
    }
    void Start()
    {
        vid = GetComponent<VideoPlayer>();
        settingBtn.alpha = 0;
        settingBtn.interactable = false;
        settingBtn.blocksRaycasts = false;
        vid.Play();
        sound.Post(gameObject);
        vid.loopPointReached += DestroyAfterVideoPlayed;
    }

    /*private void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftAlt) && Input.GetKeyDown(KeyCode.Tab))
        {
            print("true");
            if (vid.isPlaying)
            {
                vid.Stop();
            }
            else
            {
                vid.Play();
            }
        }
    }*/
    void DestroyAfterVideoPlayed(VideoPlayer vp)
    {
        if (SceneManager.GetActiveScene().name == "First GPP")
        {
            settingBtn.alpha = 1;
            settingBtn.interactable = true;
            settingBtn.blocksRaycasts = true;
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
        settingBtn.alpha = 1;
        settingBtn.interactable = true;
        settingBtn.blocksRaycasts = true;
        gameObject.SetActive(false);
        VideoFinished?.Invoke();
        yield return null;
    }

    void OnApplicationFocus(bool isFocus)
    {
        if (isFocus)
        {
            if (!vid.isPlaying)
            {
                vid.Play();
                AkSoundEngine.PostEvent("Resume", gameObject);
            }
        }
        else
        {
            if (vid.isPlaying)
            {
                vid.Pause();
                AkSoundEngine.PostEvent("Pause", gameObject);
            }
            //Debug.Log("离开游戏 激活推送");  //  返回游戏的时候触发     执行顺序 1
        }
    }


    /*void OnApplicationPause(bool isPause)
    {
        if (isPause)
        {
            Debug.Log("游戏暂停 一切停止");  // 缩到桌面的时候触发
        }
        else
        {
            Debug.Log("游戏开始  万物生机");  //回到游戏的时候触发 最晚
        }
    }*/
}
