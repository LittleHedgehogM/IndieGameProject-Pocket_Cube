//using System;
//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public class Electro_Circuit : MonoBehaviour
//{
//    GameObject go;
//    Material[] materials;
//    [SerializeField] private bool isCircuitValid = false;

//    [SerializeField] private Color colorOn = new Color(120, 53, 0);
//    [SerializeField] private Color colorOff = new Color(219, 219, 219);
//    [SerializeField] private float colorTranslationTime;
//    public static event Action CircuitChange;

//    void Start()
//    {
//        go = this.gameObject;
//        materials = go.GetComponent<MeshRenderer>().materials;
//        foreach (Material mat in materials)
//        {
//            mat.SetColor("_diffusegradient01", isCircuitValid ? colorOn : colorOff);
//        }        
//    }

//    public bool getIsCircuitValid()
//    {
//        return isCircuitValid;
//    }
//    public void changeCircuitColor(bool isCircuitValid)
//    {
//        this.isCircuitValid = isCircuitValid;

//        if (this.isCircuitValid)
//        {
//            StartCoroutine(TranslateToColor(colorOn));
//        }
//        else
//        {
//            StartCoroutine(TranslateToColor(colorOff));
//        }
//    }

//    private IEnumerator TranslateToColor(Color targetColor)
//    {
//        yield return new WaitForSeconds(0.5f);
//        foreach (Material mat in materials)
//        {
//            mat.SetColor("_diffusegradient01", targetColor);
//        }
//        CircuitChange?.Invoke();
//    }


//}
