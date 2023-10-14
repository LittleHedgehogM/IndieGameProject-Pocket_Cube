using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Electro_Switch : MonoBehaviour
{
    GameObject go;
    Material material;
    [SerializeField] bool isSwitchOn;
    [SerializeField] private Color colorOn;// = new Color(253, 178, 64);
    [SerializeField] private Color colorOff;// = new Color(219, 219, 219);
    public static event Action SwitchColorTranslationFinished;

    bool isInteractionEnabled = false;

    void Start()
    {
        go = this.gameObject;
        material = go.GetComponent<MeshRenderer>().material;
        InitSwitchColor();
    }


    public bool isElectroSwitchOn()
    {
        return isSwitchOn;
    }

    public void InitSwitchColor()
    {
        //foreach (Material mat in material)
        //{
            material.SetColor("_diffusegradient01", isSwitchOn?colorOn:colorOff);
            //Debug.Log(this.gameObject.name + ", " + material.name);
        //}

    }

    public void setInteractionEnabled(bool isEnabled)
    {
        isInteractionEnabled = isEnabled;
    }

    private void OnMouseUp()
    {
        if (isInteractionEnabled)
        {
            switchColor();
        }
        
    }

    public void switchColor()
    {
        isSwitchOn = !isSwitchOn;
        //foreach (Material mat in material)
        //{
        //    mat.SetColor("_diffusegradient01", isSwitchOn ? colorOn : colorOff);
        //}
        material.SetColor("_diffusegradient01", isSwitchOn ? colorOn : colorOff);
        SwitchColorTranslationFinished?.Invoke();
    }

    //private IEnumerator TranslateToColor(Color startColor, Color targetColor)
    //{

    //    float currentUsedTime = 0;
    //    float t = 0;


    //    while (t < 1)
    //    {
    //        Color aColor = Color.Lerp(startColor, targetColor, currentUsedTime);
    //        currentUsedTime += Time.deltaTime;
    //        t = currentUsedTime / colorTranslationTime;
    //        foreach (Material mat in materials)
    //        {
    //            mat.SetColor("_diffusegradient01", aColor);
    //        }
    //        yield return null;
    //    }
    //    //SwitchColorTranslationFinished?.Invoke();
    //}
}
