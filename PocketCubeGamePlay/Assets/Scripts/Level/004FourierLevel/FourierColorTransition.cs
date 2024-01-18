using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FourierColorTransition : MonoBehaviour
{
    private Material material;
    [SerializeField] private Color[] diffuseGradient01;
    [SerializeField] private Color[] diffuseGradient02;
    private int currentColorIndex = 0;
    private int targetColorIndex = 0;
    [SerializeField]private float transitionTimeColor;
    [SerializeField] private float transitionTimeWhite;
    [SerializeField] Material unlockMat;

    int _beat;
    int _currentLevel;
    string thisLevel;
    public bool levelUnlock = false;
    private void Awake()
    {
        //material = GetComponent<Renderer>().material;
        thisLevel = gameObject.name;
    }

    private void OnEnable()
    {
        RhythmCallBack.Rhythm_Bar += StartColorTransition;
        PlayPointBehaviour.LevelPass += LevelUnlock;
    }

    private void OnDisable()
    {
        RhythmCallBack.Rhythm_Bar -= StartColorTransition;
        PlayPointBehaviour.LevelPass -= LevelUnlock;
    }

    private void StartColorTransition()
    {
        //_beat = beat;
        StartCoroutine(ColorTransition());
    }

    private void LevelUnlock(int level)
    {
        _currentLevel = level;
        switch (level)
        {
            case 1:
                if(thisLevel.Contains("polt01"))
                {
                    levelUnlock = true;
                    gameObject.GetComponent<Renderer>().material = unlockMat;
                    material = GetComponent<Renderer>().material;
                }
                break;
            case 2:
                if (thisLevel.Contains("polt02"))
                {
                    levelUnlock = true;
                    gameObject.GetComponent<Renderer>().material = unlockMat;
                    material = GetComponent<Renderer>().material;
                }
                break; 
            case 3:
                if (thisLevel.Contains("polt03"))
                {
                    levelUnlock = true;
                    gameObject.GetComponent<Renderer>().material = unlockMat;
                    material = GetComponent<Renderer>().material;
                }
                break;
        }
        
    }

    private IEnumerator ColorTransition()
    {
        if (!levelUnlock)
        {
            yield break;
        }
      
        //_current 赋值
        Color currentColor01 = material.GetColor("_diffusegradient01");
        Color currentColor02 = material.GetColor("_diffusegradient02");
        //target
        Color targetColor01 = diffuseGradient01[targetColorIndex];   
        Color targetColor02 = diffuseGradient02[targetColorIndex];

        float currentTime = 0;    
        float targetPoint = 0;
        while (targetPoint < 1)
        {

            currentTime += Time.deltaTime;
            targetPoint = currentTime / transitionTimeColor;

            material.SetColor("_diffusegradient01", Color.Lerp(currentColor01, targetColor01, targetPoint));
            material.SetColor("_diffusegradient02", Color.Lerp(currentColor02, targetColor02, targetPoint));

            yield return null;
        }

        //print("Transition Over ------" + currentColorIndex);

        currentColorIndex = targetColorIndex;
        //print(currentColorIndex);

        targetColorIndex++;
        //(targetColorIndex);
        switch (_currentLevel)
        {
            case 1:
                if(targetColorIndex == 2)
                {
                    targetColorIndex = 0;
                }
                break;
            case 2:
                if (targetColorIndex == 3)
                {
                    targetColorIndex = 0;
                }
                break;
            case 3:
                if (targetColorIndex == 4)
                {
                    targetColorIndex = 0;
                }
                break;
        }

        yield return null;
    }
}
