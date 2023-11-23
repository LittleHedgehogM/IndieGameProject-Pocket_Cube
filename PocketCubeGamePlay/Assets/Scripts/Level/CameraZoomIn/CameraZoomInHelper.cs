using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraZoomInHelper : MonoBehaviour
{

    [SerializeField][Range(0.5f, 2f)] private float zoomInTime;
    [SerializeField] private AnimationCurve zoomInAnimationCurve;

    //public static Action ZoomInCubeFinished;

    //private void OnEnable()
    //{
    //    CubeClickEvent.CubeClick += zoomInCube;
    //}

    //private void OnDisable()
    //{
    //    CubeClickEvent.CubeClick -= zoomInCube;
    //}

    public virtual void zoomInCube()
    {
        Debug.Log("CameraZoomInHelper");
    }

    public void zoomInCube(Camera mainCam, Transform CubeTransform, Vector3 lookAtTarget)
    {
        StartCoroutine(zoomInCubeTranslation(mainCam, CubeTransform, lookAtTarget));
    }

    private IEnumerator zoomInCubeTranslation(Camera mainCam, Transform CubeTransform, Vector3 lookAtTarget)
    {
        float t = 0;
        float currentUsedTime = 0;
        float camSize = mainCam.orthographicSize;
        float targetCamSize = 2;
        while (t < 1)
        {
            currentUsedTime += Time.deltaTime;
            t = currentUsedTime / zoomInTime;
            Vector3 currentLookAtTarget = Vector3.Slerp(lookAtTarget, CubeTransform.position, zoomInAnimationCurve.Evaluate(t));
            mainCam.transform.LookAt(currentLookAtTarget);
            mainCam.orthographicSize = Mathf.Lerp(camSize, targetCamSize, zoomInAnimationCurve.Evaluate(t));
            
            yield return null;
        }

        yield return null;
        //ZoomInCubeFinished?.Invoke();
        FindObjectOfType<LevelLoaderScript>().LoadNextLevel();
    }

}
