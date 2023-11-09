using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetGoalColor : MonoBehaviour
{
    [SerializeField]
    private Color _color;



    private void Awake()
    {
        GetComponent<Renderer>().material.color = _color;
    }
}
