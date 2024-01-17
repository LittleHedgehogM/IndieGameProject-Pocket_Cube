using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EyeScoreCtl : MonoBehaviour
{
    List<GameObject> Eyes = new List<GameObject>();
    private void OnEnable()
    {
        PlayPointBehaviour.ScoreChanged += OnScoreChanged;
    }
    private void OnDisable()
    {
        PlayPointBehaviour.ScoreChanged -= OnScoreChanged;
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
            Eyes[score-1].SetActive(true);
        }
    }
}
