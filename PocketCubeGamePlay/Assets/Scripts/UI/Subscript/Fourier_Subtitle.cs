using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using TMPro;
using System.Threading.Tasks;


public class Fourier_Subtitle : MonoBehaviour
{
    public TMP_Text testText;
    public string subtitle_file_name;
    List<String> subtitle_sequence;
    private int line_number;

    private void OnEnable()
    {
        Origin_CameraController.PerformCameraStarts += LoadPrologue;
        //SceneTutorialController.TutorialEnds += LoadTutorialGuide;
        PlayPointBehaviour.StopShoot += LoadTutorialGuide;
        Origin_Controller.CubeShow += LoadCubeShow;
        NewtonScenePlayController.ScaleDraw += LoadCubeShow;
        Electro_GamePlay.CubeShow += LoadCubeShow;
        FourierKeyCube.CubeShow += LoadCubeShow;
        //CubeSolvedEvent: += LoadCubeSolved
    }

    private void OnDisable()
    {
        Origin_CameraController.PerformCameraStarts -= LoadPrologue;
       // SceneTutorialController.TutorialEnds -= LoadTutorialGuide;
        PlayPointBehaviour.StopShoot -= LoadTutorialGuide;
        Origin_Controller.CubeShow -= LoadCubeShow;
        NewtonScenePlayController.ScaleDraw -= LoadCubeShow;
        Electro_GamePlay.CubeShow -= LoadCubeShow;
        FourierKeyCube.CubeShow -= LoadCubeShow;
    }
    // Start is called before the first frame update
    void Start()
    {
        TextAsset mytxtData = Resources.Load("Textimg/" + subtitle_file_name) as TextAsset;
        testText.text = " ";
        subtitle_sequence = new List<string>(mytxtData.text.Split('&'));
        line_number = 1;
    }


    void LoadPrologue()
    {
        StartCoroutine(ShowSutitleLine(0));
    }

    void LoadTutorialGuide(string levelState)
    {
        StartCoroutine(ShowSutitleLine(line_number));
        line_number++;
    }

    void LoadCubeShow()
    {
        StartCoroutine(ShowSutitleLine(2));
    }
    private IEnumerator ShowSutitleLine(int line)
    {
        EnableSubtitle();
        AkSoundEngine.PostEvent("Play_subtitle", gameObject);
        testText.text = subtitle_sequence[line];
        yield return new WaitForSeconds(10f);

        /*AkSoundEngine.PostEvent("Play_subtitle", gameObject);
        testText.text = subtitle_sequence[line+1];
        yield return new WaitForSeconds(6f);*/
        DisableSubtitle();
        yield return null;
    }

    void EnableSubtitle()
    {
        StartCoroutine(FadeIn());
    }

    void DisableSubtitle()
    {
        StartCoroutine(FadeOut());
    }

    private IEnumerator FadeOut()
    {
        float timer = 0;
        float currentUsedTime = 0;

        float alpha = 1;

        var textLayer = this.GetComponentInChildren<TextMeshProUGUI>();
        var imgLayer = this.GetComponentInChildren<Image>();
        while (timer < 1)
        {
            currentUsedTime += Time.deltaTime;
            timer = currentUsedTime / 2;
            //Debug.Log(textLayer.color);
            alpha = (1 - timer) / timer;
            textLayer.color = new Color(textLayer.color.r, textLayer.color.g, textLayer.color.b, alpha);
            imgLayer.color = new Color(imgLayer.color.r, imgLayer.color.g, imgLayer.color.b, alpha);

            yield return null;

        }
        imgLayer.enabled = false;
        textLayer.enabled = false;

        yield return null;
    }

    private IEnumerator FadeIn()
    {
        float timer = 0;
        float currentUsedTime = 0;

        float alpha = 0;

        var textLayer = this.GetComponentInChildren<TextMeshProUGUI>();
        var imgLayer = this.GetComponentInChildren<Image>();
        imgLayer.enabled = true;
        textLayer.enabled = true;

        while (timer < 1)
        {
            currentUsedTime += Time.deltaTime;
            timer = currentUsedTime / 0.8f;
            //Debug.Log(textLayer.color);
            alpha = timer;
            textLayer.color = new Color(textLayer.color.r, textLayer.color.g, textLayer.color.b, alpha);
            imgLayer.color = new Color(imgLayer.color.r, imgLayer.color.g, imgLayer.color.b, alpha);

            yield return null;
        }

        yield return null;
    }
}
