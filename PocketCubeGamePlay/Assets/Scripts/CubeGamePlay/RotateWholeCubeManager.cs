using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateWholeCubeManager : MonoBehaviour
{

    //[SerializeField] Camera cam;
    [SerializeField] [Range(0,1)] private float mouseSenstivity;
    private Vector3 previousMousePosition;
    //Vector2 mouseDelta;
    CubePlayCameraController cameraColler;


    //public GameObject SwipeTarget;
    //public float SwipeSpeed;

    public static event Action onRotateWholeCubeFinished;

    // Start is called before the first frame update
    void Start()
    {
        cameraColler = FindObjectOfType<CubePlayCameraController>();
    }

    // Update is called once per frame
    void Update()
    {
    }
    

    public bool InitPosition()
    {
        previousMousePosition = cameraColler.ScreenToViewportPoint(Input.mousePosition);
        return SelectFace.GetMouseRayHitFace(Input.mousePosition) !=null;
    }

    public void UpdateRotateWholeCube()
    {
        if (Input.GetMouseButtonUp(1))
        {
            onRotateWholeCubeFinished?.Invoke();
           
        }
        else if (Input.GetMouseButton(1))
        {
            Vector3 direction = previousMousePosition - cameraColler.ScreenToViewportPoint(Input.mousePosition);
            direction *= mouseSenstivity;
            cameraColler.RotateMainCamera(direction);

            //cam.transform.position = Vector3.zero;
            //cam.transform.Rotate(new Vector3(1, 0, 0), direction.y * 180);
            //cam.transform.Rotate(new Vector3(0, 1, 0), -direction.x * 180, Space.World);
            //cam.transform.Translate(new Vector3(0, 0, -10));
            
            //mouseDelta = Input.mousePosition - previousMousePosition;
            //mouseDelta *= mouseSenstivity;
            //transform.rotation = Quaternion.Euler(mouseDelta.y, -mouseDelta.x, 0) * transform.rotation;
        }

        //previousMousePosition = Input.mousePosition;
        previousMousePosition = cameraColler.ScreenToViewportPoint(Input.mousePosition); ;
    }

    //void SwipeWholeCube()
    //{
    //    currentSwipe = Vector2.zero;

    //    if (Input.GetMouseButtonDown(1))
    //    {
    //        //get the 2D position of the first mouse position
    //        initalPressPos = Input.mousePosition; //new Vector2(Input.mousePosition.x, Input.mousePosition.y);
    //        //print( "initial position : "+ initalPressPos);
    //    }

    //    if (Input.GetMouseButtonUp(1))
    //    {
    //        // get the 2D position of the second mouse position
    //        endPressPos = Input.mousePosition; //new Vector2(Input.mousePosition.x, Input.mousePosition.y);
    //        currentSwipe = (endPressPos - initalPressPos);
    //        currentSwipe.Normalize();

    //    }

    //    if (currentSwipe != Vector2.zero)
    //    {
    //        //swipeCount++;
    //        if (IsLeftSwipe(currentSwipe))
    //        {                
    //            SwipeTarget.transform.Rotate(0, 90, 0,  Space.World);                
    //            //print(swipeCount + "Is left swipe");
    //        }
    //        else if (IsRightSwipe(currentSwipe))
    //        {
    //            SwipeTarget.transform.Rotate(0, -90, 0, Space.World);
    //            //print(swipeCount + "Is right swipe");
    //        }
    //        else if (IsUpLeftSwipe(currentSwipe))
    //        {
    //            SwipeTarget.transform.Rotate(0, 0, 90, Space.World);
    //            //print(swipeCount + "Is left up swipe");
    //        }
    //        else if (IsUpRightSwipe(currentSwipe))
    //        {
    //            SwipeTarget.transform.Rotate(90, 0, 0, Space.World);
    //            //print(swipeCount + "Is left down swipe");
    //        }
    //        else if (IsDownLeftSwipe(currentSwipe))
    //        {
    //            SwipeTarget.transform.Rotate(-90, 0, 0, Space.World);
    //            //print(swipeCount + "Is right up swipe");
    //        }
    //        else if (IsDownRightSwipe(currentSwipe))
    //        {
    //            SwipeTarget.transform.Rotate(0, 0, -90, Space.World);
    //            //print(swipeCount + "Is right down swipe");
    //        }
    //    }

    //}

    //bool IsLeftSwipe(Vector2 swipe)
    //{
    //    return swipe.x < 0 && swipe.y > -0.5f && swipe.y < 0.5f;
    //}
    //bool IsRightSwipe(Vector2 swipe)
    //{
    //    return swipe.x > 0 && swipe.y > -0.5f && swipe.y < 0.5f;
    //}
    //bool IsUpLeftSwipe(Vector2 swipe)
    //{
    //    return swipe.x < 0 && swipe.y > 0;
    //}

    //bool IsUpRightSwipe(Vector2 swipe)
    //{
    //    return swipe.x > 0 && swipe.y > 0;
    //}
    //bool IsDownLeftSwipe(Vector2 swipe)
    //{
    //    return swipe.x < 0 && swipe.y < 0;
    //}
    //bool IsDownRightSwipe(Vector2 swipe)
    //{
    //    return swipe.x > 0 && swipe.y < 0;
    //}
}
