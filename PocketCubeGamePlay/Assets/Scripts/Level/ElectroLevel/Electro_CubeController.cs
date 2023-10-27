using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Electro_CubeController : MonoBehaviour
{
    public static Action onCubeEnabled;
    public void enableCube()
    {
        onCubeEnabled?.Invoke();
    }

}
