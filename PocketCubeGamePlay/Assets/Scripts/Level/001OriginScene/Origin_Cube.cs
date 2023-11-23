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
    Origin_CursorController cursorController;

    public static Action CubeClicked;

    public void PlayAnim(){
        cubeAnimator.Play("Show");
    }

    private void Start()
    {
        cursorController = FindObjectOfType<Origin_CursorController>();
        
    }
    public void enableCube(){
        isEnabled = true;

        cubeVFX.SetActive(true);
        cubeVFX.transform.parent = this.transform;
        cubeVFX.transform.localPosition = Vector3.zero;
        cubeVFX.GetComponent<ParticleSystem>().Play();
    }

    private void OnMouseExit()
    {
        cursorController.setNormalCursor();
    }
    private void OnMouseEnter()
    {
        if (isEnabled)
        {
            cursorController.setHoverCursor();
        }
    }

    private void OnMouseUp()
    {
        if (isEnabled)
        {
            //cursorController.setNormalCursor();
            isEnabled = false;
            CubeClicked?.Invoke();
            // click cube
            AkSoundEngine.PostEvent("Play_box_click", gameObject);
        }
    }
}
