using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CubeCollider : MonoBehaviour
{

    bool isEnabled = false;
    private void OnEnable()
    {
        Electro_CubeController.onCubeEnabled += enableCubeCollide;
    }

    private void OnDisable()
    {
        Electro_CubeController.onCubeEnabled -= enableCubeCollide;

    }

    public void enableCubeCollide(){
       isEnabled = true;
    }

    // Start is called before the first frame update
    private void OnMouseUp()
    {
        if (isEnabled)
        {
            FindObjectOfType<LevelLoaderScript>().LoadNextLevel();
        }
    }
}
