using UnityEngine;

public class SelectFace : MonoBehaviour
{
    
    //CubeState cubeState;
    //ReadCube  readCube;
    //SwipeFaceManager swipeFace;

    void Start()
    {
       //cubeState =  FindObjectOfType<CubeState>();
       //readCube  =  FindObjectOfType<ReadCube>();
       //swipeFace =  FindObjectOfType<SwipeFaceManager>();
    }

    //public void GetMouseRayHit(out Vector3 HitPosition, out GameObject FaceHit, out GameObject CubeHit)
    //{
    //    //GameObject CubeHit = null;
    //    RaycastHit hit;
    //    FaceHit = null;
    //    CubeHit = null;
    //    HitPosition = Vector3.zero;
    //    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

    //    if (Physics.Raycast(ray, out hit))
    //    {
    //        FaceHit     = hit.collider.gameObject;
    //        CubeHit     = hit.collider.gameObject.transform.parent.gameObject.transform.parent.gameObject;
    //        HitPosition = hit.point;
    //    }        

    //}

    public static GameObject GetMouseRayHitFace()
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

    public static GameObject GetMouseRayHitFace(Vector3 mousePosition)
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

    public static GameObject getFaceRelatedCube(GameObject aFace)
    {
        return aFace.transform.parent.gameObject.transform.parent.gameObject;

    }

}
