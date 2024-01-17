using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EyeScoreCtl_1 : MonoBehaviour
{
    private List<GameObject> Eyes = new List<GameObject>();

    [SerializeField]private CanvasGroup beatTutorial;
    private void OnEnable()
    {
        PlayPointBehaviour.ScoreChanged += OnScoreChanged;
        PlayPointBehaviour.StopShoot += TutorialEndTrigger;
    }
    private void OnDisable()
    {
        PlayPointBehaviour.ScoreChanged -= OnScoreChanged;
        PlayPointBehaviour.StopShoot -= TutorialEndTrigger;
    }
    private void Awake()
    {
        foreach (Transform child in transform)
        {
            Eyes.Add(child.gameObject);
            child.gameObject.SetActive(false);
        }
    }
    private void OnScoreChanged(int score)
    {
        if (score == 0)
        {
            foreach (var eye in Eyes)
            {
                eye.SetActive(false);
            }
        }
        else if (score > 0)
        {
            print(score);
            Eyes[score-1].SetActive(true);
        }
    }
    private void TutorialEndTrigger(string state)
    {

        if (state == "Half")
        {
            StartCoroutine(TutorialEnd());   
        }
    }

    private IEnumerator TutorialEnd()
    {
        float t = 0;
        float current = 0;
        float transition = 0.5f;
        while (beatTutorial.alpha > 0)
        {
            current += Time.deltaTime;
            t = current / transition;
            beatTutorial.alpha -= t;
        }
        
            foreach (var eye in Eyes)
            {
                eye.SetActive(false);
            }

        yield return null;
    }
}
