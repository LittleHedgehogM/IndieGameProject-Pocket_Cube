using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Origin_CameraController : CameraZoomInHelper
{
    [SerializeField][Range(0, 1.0f)] float translationTime;
    [SerializeField] Vector3 lookAtTarget;
    [SerializeField] Camera mainCam;
    [SerializeField][Range(0, 0.05f)] float sensitivity;
    [SerializeField] Transform CubeTransform;

    public static Action PerformCameraStarts;

    private void Start()
    {
        mainCam.transform.LookAt(lookAtTarget);
    }

    private void OnEnable()
    {
        Origin_Axis.LeftAxisClicked   += RotateWhenLeftAxisClicked;
        Origin_Axis.RightAxisClicked  += RotateWhenRightAxisClicked;
        Origin_Axis.UpAxisClicked     += RotateWhenUpAxisClicked;
        Origin_Axis.DownAxisClicked   += RotateWhenDownAxisClicked;
        Origin_Cube.CubeClicked       += zoomInCube;
    }

    private void OnDisable()
    {
        Origin_Axis.LeftAxisClicked     -= RotateWhenLeftAxisClicked;
        Origin_Axis.RightAxisClicked    -= RotateWhenRightAxisClicked;
        Origin_Axis.UpAxisClicked       -= RotateWhenUpAxisClicked;
        Origin_Axis.DownAxisClicked     -= RotateWhenDownAxisClicked;
        Origin_Cube.CubeClicked         -= zoomInCube;
    }

    public void startPerformCam()
    {
        PerformCameraStarts?.Invoke();
    }

    private void RotateWhenLeftAxisClicked()
    {
        StartCoroutine(RotateSelf(Vector3.right, true));
    }

    private void RotateWhenRightAxisClicked()
    {
        StartCoroutine(RotateSelf(Vector3.right, false));
    }

    private void RotateWhenUpAxisClicked()
    {
        StartCoroutine(RotateSelf(Vector3.up, true));
    }

    private void RotateWhenDownAxisClicked()
    {
        StartCoroutine(RotateSelf(Vector3.up, false));
    }


    private IEnumerator RotateSelf(Vector3 axis, bool isClockwise)
    {
        float t = 0;
        float currentUsedTime = 0;
        float deltaPos = isClockwise ? sensitivity : -sensitivity;
        while (t<1)
        {
            currentUsedTime += Time.deltaTime;
            t = currentUsedTime / translationTime;
            mainCam.transform.position += axis * deltaPos;

            mainCam.transform.LookAt(lookAtTarget);

            yield return null;
        }

        t = 0;
        currentUsedTime = 0;
        while (t < 1)
        {
            currentUsedTime += Time.deltaTime;
            t = currentUsedTime / translationTime;
            mainCam.transform.position -= axis * deltaPos;
            mainCam.transform.LookAt(lookAtTarget);

            yield return null;
        }

        yield return null;

   }

   private void zoomInCube()
   {
        zoomInCube(mainCam, CubeTransform,  lookAtTarget);
   }


}
