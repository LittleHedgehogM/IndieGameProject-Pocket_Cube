using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FourierRoundTransition : MonoBehaviour
{
    private Material material;
    [SerializeField] private Color[] diffuseGradient01;
    [SerializeField] private Color[] diffuseGradient02;
    private int currentColorIndex = 0;
    private int targetColorIndex = 0;
    [SerializeField] private float transitionTimeColor;
    [SerializeField] private float transitionTimeWhite;

    int _beat;


    private void Awake()
    {
        material = GetComponent<Renderer>().material;

    }

    private void OnEnable()
    {
        RhythmCallBack.Rhythm_Bar += StartColorTransition;
       
    }

    private void StartColorTransition(int beat)
    {
        _beat = beat;
        StartCoroutine(ColorTransition());
    }

    private IEnumerator ColorTransition()
    {

        //_current 赋值
        Color currentColor01 = material.GetColor("_diffusegradient01");
        Color currentColor02 = material.GetColor("_diffusegradient02");
        //target
        Color targetColor01 = diffuseGradient01[targetColorIndex];
        Color targetColor02 = diffuseGradient02[targetColorIndex];

        float currentTime = 0;
        //加过渡色
        /*float targetPointWhite = 0;

        while (targetPointWhite < 1)
        {

            currentTime += Time.deltaTime;
            targetPointWhite = currentTime / transitionTimeWhite;

            material.SetColor("_diffusegradient01", Color.Lerp(currentColor01, Color.white, targetPointWhite));
            material.SetColor("_diffusegradient02", Color.Lerp(currentColor02, Color.white, targetPointWhite));

            yield return null;
        }*/

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
        
                if (targetColorIndex == 3)
                {
                    targetColorIndex = 0;
                }
                


        yield return null;
    }
}
