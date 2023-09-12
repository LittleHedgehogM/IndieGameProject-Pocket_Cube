using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "FourierScriptableObject", menuName = "ScriptableObjects/FourierLevel")]
public class FourierScriptableObject : ScriptableObject
{
    [SerializeField]
    Color goalColor;
    public Color Icon { get => goalColor; set => goalColor = value; }
}
