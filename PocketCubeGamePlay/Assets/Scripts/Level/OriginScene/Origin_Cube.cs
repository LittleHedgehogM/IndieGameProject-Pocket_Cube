using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Origin_Cube : MonoBehaviour
{
    bool isEnabled = false;
    [SerializeField] Animator cubeAnimator;
    [SerializeField] GameObject cubeVFX;
    public void PlayAnim(){
        cubeAnimator.Play("Show");
    }

    public void enableCube(){
        isEnabled = true;

        cubeVFX.SetActive(true);
        cubeVFX.transform.parent = this.transform;
        cubeVFX.transform.localPosition = Vector3.zero;
        cubeVFX.GetComponent<ParticleSystem>().Play();
    }

    private void OnMouseUp()
    {
        if (isEnabled)
        {
            FindObjectOfType<LevelLoaderScript>().LoadNextLevel();
        }
    }
}
