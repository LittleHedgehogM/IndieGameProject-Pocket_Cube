using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FourierCubeColor : MonoBehaviour
{
    private Material material;
    [SerializeField] private GameObject syncLevel;

    // Start is called before the first frame update
    void Start()
    {
        material = GetComponent<Renderer>().material;
    }

    // Update is called once per frame
    void Update()
    {
        if(!syncLevel.GetComponent<FourierColorChanger>().isLevelPass)
        {
            material.SetColor("_diffusegradient01", syncLevel.GetComponent<Renderer>().material.GetColor("_diffusegradient01"));
            material.SetColor("_diffusegradient02", syncLevel.GetComponent<Renderer>().material.GetColor("_diffusegradient02"));
        }
        
    }
}
