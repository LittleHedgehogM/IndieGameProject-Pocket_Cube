using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MouseDrag : MonoBehaviour
{
    [SerializeField] 
    private bool enableMouseDrag;

    //[SerializeField]
    //Camera cam;

    //[SerializeField]
    //GameObject Scale;

    [SerializeField]
    [Range(0, 5)] float distance_threshold;

    Rigidbody rb;

    Vector3 previousMousePosition;

    float mouseSensitivity = 1f;
    float moveBackSensitivity = 0.05f;
    GameObject currentScale;
    Camera currentCamera;

    public static event Action onMouseSelected;


    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        //myStatus = CoinStatus.NotOnScale;
    }

    public void SetCameraAndScale(Camera camera, GameObject scale)
    {
        currentCamera = camera;
        currentScale = scale;
    }

    //public void SetScale(GameObject scale)
    //{
    //    currentScale = scale; 
    //}

    public void setEnableMouseDrag(bool enable)
    {
        enableMouseDrag = enable;
    }

    private void OnMouseDown()
    {
        previousMousePosition = currentCamera.ScreenToViewportPoint(Input.mousePosition);
        // isPicked
        onMouseSelected?.Invoke();

    }

    private void Update()
    {
        if (enableMouseDrag)
        {
            Vector3 coinPosition = transform.position;
            Vector3 scalePosition = currentScale.transform.position;
            if (Vector3.Distance(coinPosition, scalePosition) >= distance_threshold)
            {
                transform.position += new Vector3(moveBackSensitivity * (scalePosition.x - coinPosition.x), 0,
                                                  moveBackSensitivity * (scalePosition.z - coinPosition.z));

            }
        }
        
    }

    private void OnMouseDrag()
    {
       if (enableMouseDrag)
        {
            Vector3 coinPosition = transform.position;
            Vector3 scalePosition = currentScale.transform.position;
            Vector3 mousePos = Input.mousePosition;
            Vector3 currentMouseScreenPos = currentCamera.ScreenToViewportPoint(mousePos);

            if (Vector3.Distance(coinPosition, scalePosition) < distance_threshold)
            {
                Vector3 direction =  currentMouseScreenPos - previousMousePosition;

                transform.position += new Vector3(mouseSensitivity*direction.x, 0, mouseSensitivity*direction.y);
                rb.isKinematic = true;
            }
            else
            {
                transform.position += new Vector3(moveBackSensitivity * (scalePosition.x - coinPosition.x), 0,
                                                  moveBackSensitivity * (scalePosition.z - coinPosition.z));

            }
            previousMousePosition = currentMouseScreenPos;
        }   
    }

    private void OnMouseUp()
    {
        rb.isKinematic = false;
    }
}
