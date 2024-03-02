using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEngine.Assertions;
using System.Linq;

[RequireComponent(typeof(Canvas))]
public class TutorialController_Refactor : MonoBehaviour
{
    [SerializeField] Image TutorialImage;
    [SerializeField] Image switchLoader;
    [SerializeField] Sprite[] tutorialSprites;
    [SerializeField] Image AimImage;
    [SerializeField] float translationTime;
    [SerializeField] Image GoPrevButton;
    [SerializeField] Image GoNextButton;
    [SerializeField] Image ConfirmButton;   

    public static Action TutorialEnds;

    Vector3 tutorialScale = Vector3.one;
    Color transparentWhite;
    
    private int index = 0;

    private void Start()
    {

        Assert.IsNotNull(tutorialSprites);
        transparentWhite = new Color(Color.white.r, Color.white.g, Color.white.b, 0);
        tutorialScale = TutorialImage.transform.localScale;
        index = 0;

        TutorialImage.color = transparentWhite;
        TutorialImage.transform.localScale = Vector3.zero;
        TutorialImage.sprite = tutorialSprites[index];

        switchLoader.color = new Color(switchLoader.color.r, switchLoader.color.g, switchLoader.color.b, 0);

        GoPrevButton.color = transparentWhite;
        GoPrevButton.transform.localScale = Vector3.zero;

        GoNextButton.color = transparentWhite;
        GoNextButton.transform.localScale = Vector3.zero;

        ConfirmButton.color = transparentWhite;
        ConfirmButton.transform.localScale = Vector3.zero;

        AimImage.color = transparentWhite;
        AimImage.transform.localScale = Vector3.zero;
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
        index = 0;

        TutorialImage.sprite = tutorialSprites[index];

        StartCoroutine(showImage(TutorialImage, translationTime));
        if (tutorialSprites.Length ==1) 
        {
            GoPrevButton.transform.localScale = Vector3.zero;
            GoNextButton.transform.localScale = Vector3.zero;
            showConfirmButton();        
        }
        else if (tutorialSprites.Length > 1) 
        {
            StartCoroutine(showImage(GoNextButton, 0.2f));
        }

    }

    public void goPreviousTutorial()
    {

        if (index >= 1 && index <= tutorialSprites.Length - 1)
        {
            
            index--;
            StartCoroutine(switchTutorialImage(tutorialSprites[index]));
            showGoNextButton();
            hideConfirmButton();
        }

        if (index == 0)
        {
            hideGoPrevButton();     
        }

    }

    public void goNextTutorial()
    {
        if (index >= 0 && index < tutorialSprites.Length - 1)
        {          
            index++;
            StartCoroutine(switchTutorialImage(tutorialSprites[index]));
            showGoPrevButton();
        }

        if (index == tutorialSprites.Length - 1)
        {
            hideGoNextButton();
            showConfirmButton();
        }
        
    }

    public void closeTutorial()
    {
        StartCoroutine(hideTutorial());
    }

    private void showGoPrevButton()
    {
        StartCoroutine(showImage(GoPrevButton, 0.2f));
    }

    private void showGoNextButton()
    {
        StartCoroutine(showImage(GoNextButton, 0.2f));
    }


    private void hideGoPrevButton()
    {
        StartCoroutine(hideImage(GoPrevButton, 0.2f));
    }

    private void hideGoNextButton() 
    {
        StartCoroutine(hideImage(GoNextButton, 0.2f));
    }

    private void showConfirmButton()
    {
        StartCoroutine(showImage(ConfirmButton, 0.2f));
    }

    private void hideConfirmButton()
    {
        StartCoroutine(hideImage(ConfirmButton, 0.2f));
    }

    private IEnumerator showImage(Image img, float duration)
    {
        if (img.color.a > 0)
        {
            yield return null;
        }
        else
        {
            img.transform.localScale = Vector3.one;

            float currentTime = 0;
            float t = currentTime / duration;
            float targetAlpha = 1;

            while (t < 1)
            {
                currentTime += Time.deltaTime;
                t = currentTime / duration;
                float currentAlpha = Mathf.Lerp(0, targetAlpha, t);
                img.color = new Color(img.color.r, img.color.g, img.color.b, currentAlpha);
                yield return null;

            }


        }
    }

    private IEnumerator hideImage(Image img, float duration) 
    {   

        if (img.color.a == 0 || img.transform.localScale == Vector3.zero)
        {
            yield return null;
        }
        else 
        {
            float currentTime = 0;
            float t = currentTime / duration;
            float targetAlpha = 0;

            while (t < 1)
            {
                currentTime += Time.deltaTime;
                t = currentTime / duration;
                float currentAlpha = Mathf.Lerp(1, targetAlpha, t);
                img.color = new Color(img.color.r, img.color.g, img.color.b, currentAlpha);
                yield return null;

            }
        
            img.transform.localScale = Vector3.zero;
            yield return null;       
        }
        
    }


    private IEnumerator switchTutorialImage(Sprite next)
    {

        float currentTime = 0;
        float t = currentTime / translationTime;
        while (t < 1)
        {
            currentTime += Time.deltaTime;
            t = currentTime / translationTime;
            float currentAlpha = Mathf.Lerp(0, 1, t);
            switchLoader.color = new Color(switchLoader.color.r, switchLoader.color.g, switchLoader.color.b, currentAlpha);

            yield return null;

        }

        TutorialImage.sprite = next;
        yield return null;

        currentTime = 0;
        t = currentTime / translationTime;
        while (t < 1)
        {
            currentTime += Time.deltaTime;
            t = currentTime / translationTime;
            float currentAlpha = Mathf.Lerp(1, 0, t);
            switchLoader.color = new Color(switchLoader.color.r, switchLoader.color.g, switchLoader.color.b, currentAlpha);

            yield return null;

        }

        yield return null;

        
    }


    private IEnumerator hideTutorial()
    {
        StartCoroutine(hideImage(ConfirmButton, 0.1f));
        hideGoPrevButton();
        hideGoNextButton();
        float currentTime = 0;
        float t = currentTime / 1.0f;

        while (t < 1)
        {
            currentTime += Time.deltaTime;
            t = currentTime / (translationTime*2);
            float currentAlpha = Mathf.Lerp(1, 0, t);
            TutorialImage.color = new Color(TutorialImage.color.r,
                                                                  TutorialImage.color.g,
                                                                  TutorialImage.color.b, currentAlpha);
            yield return null;

        }

        TutorialImage.transform.localScale = Vector3.zero;
        yield return null;
        TutorialEnds?.Invoke();
        
        StartCoroutine(showAimImage());
    }


    private IEnumerator showAimImage()
    {
        AimImage.transform.localScale = Vector3.one;
        AimImage.color = new Color(AimImage.color.r, AimImage.color.g, AimImage.color.b, 0);
        float currentTime = 0;
        float translationTime = 0.5f;
        float t = currentTime / translationTime;
        float targetAlpha = 1;
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
