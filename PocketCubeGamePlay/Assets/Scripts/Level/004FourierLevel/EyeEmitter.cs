using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EyeEmitter : MonoBehaviour
{
    public static Action<string> Eye_Activated;

    private void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.CompareTag("Player"))
        {
            string eyeName = gameObject.name;
            Eye_Activated?.Invoke(eyeName);
        }
    }

    public static Action<string> Eye_InPosition;
    private void EyeAnimationOver()
    {
        string eyeName = gameObject.name;
        Eye_InPosition?.Invoke(eyeName);
    }
    [SerializeField] private GameObject beatTutorial;
    private IEnumerator BeatTutorialShow()
    {
        beatTutorial.SetActive(true);
        CanvasGroup canvasGroup = beatTutorial.GetComponent<CanvasGroup>();
        
        float startAlphaVal = 0;
        float targetAlphaVal = 1;
        float currentTime = 0;
        float translationTime = 0.5f;
        float t = 0;
        /*show image*/
        while (t < 1)
        {
            currentTime += Time.deltaTime;
            t = currentTime / translationTime;
            canvasGroup.alpha = Mathf.Lerp(startAlphaVal, targetAlphaVal, t);
            yield return null;

        }
        yield return null;
    }
}
