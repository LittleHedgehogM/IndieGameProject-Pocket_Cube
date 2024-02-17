using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using static System.TimeZoneInfo;

public class SceneTutorialController : MonoBehaviour
{
    [SerializeField] Image TutorialImage;
    [SerializeField] [Range (0, 10)] int seconds;
    [SerializeField] Image AimImage;
    [SerializeField] bool disableTutorial;
    [SerializeField] bool mouseClickExitTutorial;

    public static Action TutorialEnds;

    float aimAlphaVal = 1.0f;
    Vector3 tutorialScale = Vector3.one;

    float tutorialAlphaVal = 1.0f;
    Vector3 aimScale = Vector3.one;

    float tutorialShowTime = 0;

    private void Start()
    {
        tutorialAlphaVal = TutorialImage.color.a;
        tutorialScale = TutorialImage.transform.localScale;
        TutorialImage.color = new Color(TutorialImage.color.r, TutorialImage.color.g, TutorialImage.color.b, 0);
        TutorialImage.transform.localScale = Vector3.zero;

        aimScale = AimImage.transform.localScale;
        AimImage.transform.localScale = Vector3.zero;

        if (disableTutorial)
        {
            TutorialEnds?.Invoke();
        }

        tutorialShowTime = 0;
    }

    private void OnEnable()
    {
        if (!disableTutorial)
        {
            SceneOpeningCameraAnimationControl.PerformCameraFinished += TutorialStarts;
        }
        
    }

    private void OnDisable() 
    {
        if (!disableTutorial)
        {
            SceneOpeningCameraAnimationControl.PerformCameraFinished -= TutorialStarts;
        }
    }

    public void TutorialStarts()
    {
        if (seconds >0) 
        {
            StartCoroutine(showTutorial());
        }
        else 
        {
            TutorialEnds?.Invoke();
            StartCoroutine(showAimImage());

        }
    }

    private void Update()
    {
        if (tutorialShowTime < seconds)
        {
            tutorialShowTime += Time.deltaTime;
        }
        
        if (tutorialShowTime > 2.0f && mouseClickExitTutorial 
            && Input.GetMouseButtonUp(0) && TutorialImage.color.a == tutorialAlphaVal) 
        {
            StartCoroutine(hideTutorial());
        }
    }

    private IEnumerator showTutorial()
    {

        // show tutorial
        TutorialImage.transform.localScale = tutorialScale;

        float currentTime = 0;
        float translationTime = 0.5f;
        float t = currentTime / translationTime;
        float targetAlpha = tutorialAlphaVal;

        while (t < 1)
        {
            currentTime += Time.deltaTime;
            t = currentTime / translationTime;
            float currentAlpha = Mathf.Lerp(0, targetAlpha, t);
            TutorialImage.color = new Color(TutorialImage.color.r, TutorialImage.color.g, TutorialImage.color.b, currentAlpha);
            yield return null;

        }

        yield return new WaitForSeconds(seconds);
        if (!mouseClickExitTutorial) 
        {           
            StartCoroutine(hideTutorial());
        }

        //currentTime = 0;
        //translationTime = 1.0f;
        //t = currentTime / translationTime;
        //targetAlpha = 0f;

        //while (t < 1)
        //{
        //    currentTime += Time.deltaTime;
        //    t = currentTime / translationTime;
        //    float currentAlpha = Mathf.Lerp(tutorialAlphaVal, targetAlpha, t);
        //    TutorialImage.color = new Color(TutorialImage.color.r, TutorialImage.color.g, TutorialImage.color.b, currentAlpha);
        //    yield return null;

        //}
        //TutorialImage.transform.localScale = Vector3.zero;
        //yield return null;
        //TutorialEnds?.Invoke();
        //StartCoroutine(showAimImage());
        
    }


    private IEnumerator hideTutorial()
    {
        
        float currentTime = 0;
        float translationTime = 1.0f;
        float t = currentTime / translationTime;
        float targetAlpha = 0f;

        while (t < 1)
        {
            currentTime += Time.deltaTime;
            t = currentTime / translationTime;
            float currentAlpha = Mathf.Lerp(tutorialAlphaVal, targetAlpha, t);
            TutorialImage.color = new Color(TutorialImage.color.r, TutorialImage.color.g, TutorialImage.color.b, currentAlpha);
            yield return null;

        }
        TutorialImage.transform.localScale = Vector3.zero;
        yield return null;
        TutorialEnds?.Invoke();
        StartCoroutine(showAimImage());
    }

    private IEnumerator showAimImage()
    {
        AimImage.transform.localScale = aimScale;
        AimImage.color = new Color(AimImage.color.r, AimImage.color.g, AimImage.color.b, 0);
        float currentTime = 0;
        float translationTime = 0.5f;
        float t = currentTime / translationTime;
        float targetAlpha = aimAlphaVal;
        while (t < 1)
        {
            currentTime += Time.deltaTime;
            t = currentTime / translationTime;
            float currentAlpha = Mathf.Lerp(0, targetAlpha, t);
            AimImage.color = new Color(AimImage.color.r, AimImage.color.g, AimImage.color.b, currentAlpha);
            yield return null;

        }
    }

}
