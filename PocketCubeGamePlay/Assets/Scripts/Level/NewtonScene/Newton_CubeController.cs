using System.Collections;
using System.Collections.Generic;
using TMPro.Examples;
using UnityEngine;

public class Newton_CubeController : MonoBehaviour
{
    
    [SerializeField] private Animator cubeAnimator;
    [SerializeField] private GameObject cube;
    Scene_Newton_Camera_Controller myCameraController;


    bool canInteract = false;

    public GameObject getCube()
    {
        return this.gameObject;
    }

    public void startCubeAnimation()
    {
        cubeAnimator.SetTrigger("IsNewtonPuzzleSolved");

    }

    public void finishCubeAnimation()
    {
        canInteract = true;
    }


    private void Start()
    {
        myCameraController = FindObjectOfType<Scene_Newton_Camera_Controller>();

    }
    private void Update()
    {
        if (canInteract) 
        {
            RaycastHit hit;
            Ray ray;
            ray = myCameraController.getCurrentCamera().ScreenPointToRay(Input.mousePosition);
            if (Input.GetMouseButtonUp(0) && Physics.Raycast(ray, out hit))
            {
                GameObject hitObject = hit.collider.gameObject;
                if (hitObject == cube)
                {
                    FindObjectOfType<LevelLoaderScript>().LoadNextLevel();
                }

            }
        }
    }

}
