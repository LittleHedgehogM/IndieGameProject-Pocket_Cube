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

    async void LoadPrologue()
    {
        EnableSubtitle();
        testText.text = subtitle_sequence[0];
        await Task.Delay(4000);
        testText.text = subtitle_sequence[1];
    }

    async void LoadTutorialGuide()
    {
        testText.text = subtitle_sequence[2];
        await Task.Delay(6000);
        testText.text = subtitle_sequence[3];
        await Task.Delay(8000);
        DisableSubtitle();
    }

    async void LoadCubeShow()
    {
        EnableSubtitle();
        testText.text = subtitle_sequence[4];
        await Task.Delay(6000);
        testText.text = subtitle_sequence[5];
        await Task.Delay(9000);
        DisableSubtitle();
        //EndofSubtitle();
    }

    async void LoadCubeSolved()
    {
        EnableSubtitle();
        testText.text = subtitle_sequence[6];
        await Task.Delay(10000);
        testText.text = subtitle_sequence[7];
        await Task.Delay(10000);
        DisableSubtitle();
    }

    void EnableSubtitle()
    {
        var textLayer = this.GetComponentInChildren<TextMeshProUGUI>();
        var imgLayer = this.GetComponentInChildren<Image>();

        imgLayer.enabled = true;
        textLayer.enabled = true;
    }

    void DisableSubtitle()
    {
        var textLayer = this.GetComponentInChildren<TextMeshProUGUI>();
        var imgLayer = this.GetComponentInChildren<Image>();

        imgLayer.enabled = false;
        textLayer.enabled = false;
    }

    void EndofSubtitle()
    {
        var textLayer = this.GetComponentInChildren<TextMeshProUGUI>();
        var imgLayer = this.GetComponentInChildren<Image>();

        Destroy(imgLayer);
        Destroy(textLayer);
        Destroy(this);
    }
}
