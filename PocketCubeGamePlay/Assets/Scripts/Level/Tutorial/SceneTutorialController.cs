using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class SceneTutorialController : MonoBehaviour
{
    [SerializeField] Image TutorialImage;
    [SerializeField] [Range (0, 10)] int seconds;

    public static Action TutorialEnds;

    float AlphaVal = 1.0f;
    Vector3 scale = Vector3.one;

    private void Start()
    {
        AlphaVal = TutorialImage.color.a;
        scale = TutorialImage.transform.localScale;
        TutorialImage.color = new Color(TutorialImage.color.r, TutorialImage.color.g, TutorialImage.color.b, 0);
        TutorialImage.transform.localScale = Vector3.zero;
    }

    private void OnEnable()
    {
        SceneOpeningCameraAnimationControl.PerformCameraFinished += TutorialStarts;
    }

    private void OnDisable() 
    {
        SceneOpeningCameraAnimationControl.PerformCameraFinished -= TutorialStarts;

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
        }        
    }

    private IEnumerator showTutorial()
    {

        // show tutorial
        TutorialImage.transform.localScale = scale;

        float currentTime = 0;
        float translationTime = 0.5f;
        float t = currentTime / translationTime;
        float targetAlpha = AlphaVal;

        while (t < 1)
        {
            currentTime += Time.deltaTime;
            t = currentTime / translationTime;
            float currentAlpha = Mathf.Lerp(0, targetAlpha, t);
            TutorialImage.color = new Color(TutorialImage.color.r, TutorialImage.color.g, TutorialImage.color.b, currentAlpha);
            yield return null;

        }

        yield return new WaitForSeconds(seconds);

        currentTime = 0;
        translationTime = 1.0f;
        t = currentTime / translationTime;
        targetAlpha = 0f;

        while (t < 1)
        {
            currentTime += Time.deltaTime;
            t = currentTime / translationTime;
            float currentAlpha = Mathf.Lerp(AlphaVal, targetAlpha, t);
            TutorialImage.color = new Color(TutorialImage.color.r, TutorialImage.color.g, TutorialImage.color.b, currentAlpha);
            yield return null;

        }
        TutorialImage.transform.localScale = Vector3.zero;
        yield return null;
        TutorialEnds?.Invoke();

    }

}
