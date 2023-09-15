using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightController : MonoBehaviour
{
    [System.Serializable]
    public enum LogicGate
    {
        AND,
        OR
    }

    [SerializeField] LogicGate lightLogic;


    GameObject go;
    Material[] materials;

    [SerializeField] Color Color_On;
    [SerializeField] Color Color_Off;

    [SerializeField] private SwitchController sw_left;
    [SerializeField] private SwitchController sw_right;
    [SerializeField] private bool isLightOn;


    private void Start()
    {
        go = this.gameObject;
        materials = go.GetComponent<MeshRenderer>().materials;
        ResetMaterial();
        updateLight();
    }

    public void ResetMaterial()
    {
        foreach (Material mat in materials)
        {
            if (isLightOn)
            {
                mat.SetColor("_Color", Color_On);
            }
            else
            {
                mat.SetColor("_Color", Color_Off);
            }

        }

    }

    public bool IsLightOn()
    {
        return isLightOn;
    }

    private void OnEnable()
    {
        SwitchController.onSwitchChanged += updateLight;
    }

    private void OnDisable()
    {
        SwitchController.onSwitchChanged -= updateLight;

    }

    public void updateLight()
    {

        if (lightLogic == LogicGate.AND)
        {
            isLightOn = sw_left.getIsSwitchOn() && sw_right.getIsSwitchOn();
        }
        else if (lightLogic == LogicGate.OR)
        {
            isLightOn = sw_left.getIsSwitchOn() || sw_right.getIsSwitchOn();
        }

        foreach (Material mat in materials)
        {
            if (isLightOn)
            {
                mat.SetColor("_Color", Color_On);
            }
            else
            {
                mat.SetColor("_Color", Color_Off);
            }

        }

    }
}
