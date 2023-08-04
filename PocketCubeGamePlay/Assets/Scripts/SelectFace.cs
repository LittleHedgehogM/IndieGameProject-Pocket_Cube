using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class SelectFace : MonoBehaviour
{
    
    CubeState cubeState;
    ReadCube readCube;
    SwipeFace swipeFace;
    //int layerMask = 1 << 8;


    //public Transform UpPivot;
    //public Transform DownPivot;
    //public Transform LeftPivot;
    //public Transform RightPivot;
    //public Transform FrontPivot;
    //public Transform BackPivot;
    //Transform closetPivot;

    void Start()
    {
       cubeState =  FindObjectOfType<CubeState>();
       readCube  =  FindObjectOfType<ReadCube>();
       swipeFace = FindObjectOfType<SwipeFace>();
    }

    public void GetMouseRayHit(out Vector3 HitPosition, out GameObject FaceHit, out GameObject CubeHit)
    {
        //GameObject CubeHit = null;
        RaycastHit hit;
        FaceHit = null;
        CubeHit = null;
        HitPosition = Vector3.zero;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit))
        {
            FaceHit     = hit.collider.gameObject;
            CubeHit     = hit.collider.gameObject.transform.parent.gameObject.transform.parent.gameObject;
            HitPosition = hit.point;
        }        

    }

    public GameObject GetMouseRayHitFace()
    {
        GameObject FaceHit = null;
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        
        if (Physics.Raycast(ray, out hit))
        {         
            FaceHit = hit.collider.gameObject;
        }
        return FaceHit;

    }

    public GameObject GetMouseRayHitFace(Vector3 mousePosition)
    {
        GameObject FaceHit = null;
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(mousePosition);

        if (Physics.Raycast(ray, out hit))
        {
            FaceHit = hit.collider.gameObject;
        }
        return FaceHit;

    }

    //float GetDotProduct(Vector3 CubeWorldPosition,Vector3 Axis)
    //{

    //    Vector3 mousePos = Input.mousePosition;
    //    //Vector3 AxisScreenPosition      = Camera.main.WorldToScreenPoint(CubeWorldPosition + Axis);
    //    Vector3 CubeScreenPosition      = Camera.main.WorldToScreenPoint(CubeWorldPosition);
    //    Vector3 v1 = Axis.normalized;
    //    Vector3 v2 = (mousePos - CubeScreenPosition).normalized; 
    //    return Vector3.Dot(v1, v2);

    //}


    // Update is called once per frame
    void Update()
    {                  

    }
}
