using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EyeEmitter : MonoBehaviour
{
    public static Action<string> Eye_Activated;

    private void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.CompareTag("Player"))
        {
            string eyeName = gameObject.name;
            Eye_Activated?.Invoke(eyeName);
        }
    }

    public static Action<string> Eye_InPosition;
    private void EyeAnimationOver()
    {
        string eyeName = gameObject.name;
        Eye_InPosition?.Invoke(eyeName);
    }
}
