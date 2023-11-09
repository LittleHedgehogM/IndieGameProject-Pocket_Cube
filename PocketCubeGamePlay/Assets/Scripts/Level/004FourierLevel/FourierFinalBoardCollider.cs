using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FourierFinalBoardCollider : MonoBehaviour
{

    [SerializeField] Camera mainCam;

    private void OnCollisionStay(Collision collision)
    {
        if (Input.GetKeyUp(KeyCode.Space))
        {
            FindObjectOfType<LevelLoaderScript>().LoadNextLevel();
        }
    }
}
