using System.Collections;
using System.Collections.Generic;
using TMPro.Examples;
using UnityEngine;

public class Newton_CubeController : CubeClickEvent
{
    
    [SerializeField] private Animator cubeAnimator;
    [SerializeField] private GameObject cube;
    Scene_Newton_Camera_Controller myCameraController;
    Newton_CursorController cursorController;

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
        cursorController = FindObjectOfType<Newton_CursorController>();

    }



    private void Update()
    {
        if (canInteract) 
        {
            cursorController.setDefaultCursor();
            RaycastHit hit;
            Ray ray;
            ray = myCameraController.getCurrentCamera().ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit))
            {              
                GameObject hitObject = hit.collider.gameObject;
                if (hitObject == cube && !Utils.isMouseOverUI())
                {
                    cursorController.setSelectCursor();

                    /*                    if (Input.GetMouseButton(0) && !Utils.isMouseOverUI()) 
                                        {
                                            cursorController.setClickDownCursor();

                                        }
                                        else if (Input.GetMouseButtonUp(0) && !Utils.isMouseOverUI()) 
                                        {
                                            CubeClick?.Invoke();
                                            canInteract = false;
                                        }*/
                    if (Input.GetMouseButtonDown(0) && !Utils.isMouseOverUI())
                    {
                        Debug.Log("GetMouseButtonDown");
                        StartCoroutine(ClickCube());
                    }       
                }
            }
        }
    }
    private IEnumerator ClickCube()
    {
        while (Input.GetMouseButton(0))
        {
            cursorController.setClickDownCursor();
            yield return null;
        }
        RaycastHit hit;
        Ray ray;
        ray = myCameraController.getCurrentCamera().ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit))
        {
            GameObject hitObject = hit.collider.gameObject;
            if (hitObject == cube)
            {
                Debug.Log("EnterCube");
                CubeClick?.Invoke();
                canInteract = false;
            }
        }
        yield break;
    }
}
