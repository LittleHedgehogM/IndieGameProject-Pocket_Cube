using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using TMPro;
using System.Threading.Tasks;

public class SubtitleController : MonoBehaviour
{
    public TMP_Text testText;
    public string subtitle_file_name;
    List<String> subtitle_sequence;

    private void OnEnable()
    {
        Origin_CameraController.PerformCameraStarts += LoadPrologue;
        SceneTutorialController.TutorialEnds        += LoadTutorialGuide;
        Origin_Controller.CubeShow                  += LoadCubeShow;
        NewtonScenePlayController.ScaleDraw         += LoadCubeShow;
        Electro_GamePlay.CubeShow                   += LoadCubeShow;
        FourierKeyCube.CubeShow                     += LoadCubeShow;
        //CubeSolvedEvent: += LoadCubeSolved
    }

    private void OnDisable()
    {
        Origin_CameraController.PerformCameraStarts -= LoadPrologue;
        SceneTutorialController.TutorialEnds        -= LoadTutorialGuide;
        Origin_Controller.CubeShow                  -= LoadCubeShow;
        NewtonScenePlayController.ScaleDraw         -= LoadCubeShow;
        Electro_GamePlay.CubeShow                   -= LoadCubeShow;
        FourierKeyCube.CubeShow                     -= LoadCubeShow;
    }

    void Start()
    {
        TextAsset mytxtData = Resources.Load("Textimg/" + subtitle_file_name) as TextAsset;
        testText.text = " ";
        subtitle_sequence = new List<string>(mytxtData.text.Split('&'));
    }

    void LoadPrologue()
    {
        StartCoroutine(ShowSutitleLine(0));
    }

    void LoadTutorialGuide()
    {
        StartCoroutine(ShowSutitleLine(2));
    }

    void LoadCubeShow()
    {
        StartCoroutine(ShowSutitleLine(4));
    }

    private IEnumerator ShowSutitleLine(int line)
    {
        EnableSubtitle();
        AkSoundEngine.PostEvent("Play_subtitle", gameObject);
        testText.text = subtitle_sequence[line];
        yield return new WaitForSeconds(6f);

        AkSoundEngine.PostEvent("Play_subtitle", gameObject);
        testText.text = subtitle_sequence[line+1];
        yield return new WaitForSeconds(6f);
        DisableSubtitle();
        yield return null;
    }

    void EnableSubtitle()
    {
        try
        {
            var textLayer = this.GetComponentInChildren<TextMeshProUGUI>();
            var imgLayer = this.GetComponentInChildren<Image>();

            imgLayer.enabled = true;
            textLayer.enabled = true;
        }
        catch (Exception e)
        {
            Debug.LogException(e);
        }
    }

    void DisableSubtitle()
    {
        //StartCoroutine(FadeOut());

        try
        {
            //_animatorTransition.Play("Crossfade_Start");

            var textLayer = this.GetComponentInChildren<TextMeshProUGUI>();
            var imgLayer = this.GetComponentInChildren<Image>();

            imgLayer.enabled = false;
            textLayer.enabled = false;
        }
        catch(Exception e)
        {
            Debug.LogException(e);
        }
    }
    /*
    private IEnumerator FadeOut()
    {
        float timer = 0;
        float currentUsedTime = 0;

        float alpha = 1;
        
        var textLayer = this.GetComponentInChildren<TextMeshProUGUI>();
        var imgLayer = this.GetComponentInChildren<Image>();
        while (timer < 5)
        {
            currentUsedTime += Time.deltaTime;
            timer = currentUsedTime / 5;
            Debug.Log(textLayer.color);
            alpha = 1 - (1/timer);
            textLayer.color = new Color(imgLayer.color.r, imgLayer.color.g, imgLayer.color.b, alpha);
        }
        yield return null;

    }*/
    /*void EndofSubtitle()
    {
        var textLayer = this.GetComponentInChildren<TextMeshProUGUI>();
        var imgLayer = this.GetComponentInChildren<Image>();

        Destroy(imgLayer);
        Destroy(textLayer);
        Destroy(this);
    }*/
}
