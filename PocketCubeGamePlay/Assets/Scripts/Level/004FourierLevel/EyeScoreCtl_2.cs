using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EyeScoreCtl_2 : MonoBehaviour
{
    private List<GameObject> Eyes = new List<GameObject>();


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
        print(Eyes.Count);
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
            int eyeIndex = score - 1;
            Eyes[eyeIndex].SetActive(true);
        }
    }

}
