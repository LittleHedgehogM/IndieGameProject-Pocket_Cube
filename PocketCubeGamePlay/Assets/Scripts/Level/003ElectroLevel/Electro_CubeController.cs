using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Electro_CubeController : MonoBehaviour
{
    public static Action onCubeEnabled;
    [SerializeField] private GameObject cubeVFX;
    [SerializeField] private Transform CubeTransform;


    public void enableCube()
    {
        StartCoroutine(cubeEnable());
    }

    private IEnumerator cubeEnable()
    {
        yield return new WaitForSeconds(1.5f);
        onCubeEnabled?.Invoke();
        //show VFX
        cubeVFX.SetActive(true);
        cubeVFX.transform.position = CubeTransform.position;
        cubeVFX.GetComponent<ParticleSystem>().Play();
    }


}
