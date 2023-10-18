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
        material.SetColor("_diffusegradient01", isSwitchOn?colorOn:colorOff);
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
        material.SetColor("_diffusegradient01", isSwitchOn ? colorOn : colorOff);
        SwitchColorTranslationFinished?.Invoke();
    }

}
