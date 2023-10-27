using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Origin_Cube : MonoBehaviour
{
    bool isEnabled = false;
    [SerializeField] Animator cubeAnimator;

    public void PlayAnim(){
        cubeAnimator.Play("Show");
    }

    public void enableCube(){
        isEnabled = true;
    }

    private void OnMouseUp()
    {
        if (isEnabled)
        {
            FindObjectOfType<LevelLoaderScript>().LoadNextLevel();
        }
    }
}
