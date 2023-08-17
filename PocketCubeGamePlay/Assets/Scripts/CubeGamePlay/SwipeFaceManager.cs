using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class SwipeFaceManager : MonoBehaviour
{
    public GameObject FrontLeftUp;
    public GameObject FrontRightUp;
    public GameObject FrontLeftDown;
    public GameObject FrontRightDown;
    public GameObject BackLeftUp;
    public GameObject BackRightUp;
    public GameObject BackLeftDown;
    public GameObject BackRightDown;

    [SerializeField] AnimationCurve swipeAnimationCurve;
    [SerializeField][Range(0, 1)] float totalTime;
    public GameObject emptyGO;
    float currentUsedTime;

    Vector3 initalMousePressPos;
    Vector3 endMousePressPos;
    Vector3 currentMouseSwipe;

    List<GameObject>    CurrentSwipeFace;
    Vector3             CurrentSwipeAxis;
    float               CurrentSwipeDegree = 0;
    bool                isCurrentSwipeClockWise = true;
    CubeState cubeState;
    enum SwipeState 
    { 
        WaitForSwipe,
        Swiping,
        FinishSwipe
    };
    SwipeState currentSwipeState;

    List<Vector3> cubeStartPosition;
    List<Quaternion> cubeStartRotation;
    List<GameObject> cubeTargetGameObjects;

    public static event Action onSwipeFinished;

    // Start is called before the first frame update
    void Start()
    {
        cubeState       = FindObjectOfType<CubeState>();
    }

    public List<GameObject> SelectFaceToSwipe(List<GameObject> cubeSides)
    {

        List<GameObject> FaceToSwipe = new List<GameObject>();
        foreach (GameObject face in cubeSides)
        {
            FaceToSwipe.Add(face.transform.parent.gameObject.transform.parent.gameObject);
            
        }
        return FaceToSwipe;
    }

    public List<GameObject> SelectFaceToSwipe(GameObject Cube, Vector3 Axis)
    {
        List<GameObject> CubeList = new List<GameObject>
        {
            FrontLeftUp,
            FrontRightUp,
            FrontLeftDown,
            FrontRightDown,
            BackLeftUp,
            BackLeftDown,
            BackRightUp,
            BackRightDown,
        };

        CurrentSwipeAxis = Axis ;
        List<GameObject>  FaceToSwipe = new List<GameObject>();

        float eplison = 0.01f;
        foreach (GameObject aCube in CubeList)
        {
            if (Axis.x!=0)
            {
                if (Math.Abs(aCube.transform.position.x - Cube.transform.position.x)<eplison) 
                {
                    FaceToSwipe.Add(aCube);
                }
            }
            else if (Axis.y!=0)
            {
                if (Math.Abs(aCube.transform.position.y - Cube.transform.position.y) < eplison)
                {
                    FaceToSwipe.Add(aCube);
                }
            }
            else if (Axis.z != 0)
            {
                if (Math.Abs(aCube.transform.position.z - Cube.transform.position.z) < eplison)
                {
                    FaceToSwipe.Add(aCube);
                }
            }
                       
        }

        return FaceToSwipe;
    }

    List<GameObject> SelectFaceToSwipe(List<List<GameObject>> possibleFaces, GameObject CubeHit)
    {
        List<GameObject> CubesToSwipe = new List<GameObject>();
        bool findFace = false;

        foreach ( List<GameObject> face in possibleFaces)
        {
            foreach ( GameObject aFace in face )
            {
                GameObject CubeRelatedToFace = aFace.transform.parent.gameObject.transform.parent.gameObject;
                if (CubeRelatedToFace == CubeHit)
                {
                    findFace = true;
                    break;
                }
            }

            if (findFace)
            {
                foreach (GameObject aFace in face)
                {
                    GameObject CubeRelatedToFace = aFace.transform.parent.gameObject.transform.parent.gameObject;
                    CubesToSwipe.Add(CubeRelatedToFace);
                }
                break;
            }
        }

        return CubesToSwipe;
    }


    void ResetValues()
    {
        currentUsedTime = 0;
        CurrentSwipeDegree = 0;
        CurrentSwipeAxis = Vector3.zero;
        isCurrentSwipeClockWise = false;
        CurrentSwipeFace.Clear();
        currentSwipeState = SwipeState.WaitForSwipe;
        //cubeTargetGameObjects.Clear();
        //cubeStartPosition.Clear();
        //cubeStartRotation.Clear();
    }


    public void InitSwipe()
    {
        currentUsedTime = 0;
        //cubeStartPosition = new List<Vector3>();
        //cubeStartRotation = new List<Quaternion>();
        //cubeTargetGameObjects = new List<GameObject>();

        //for (int i = 0; i < CurrentSwipeFace.Count; i++)
        //{
        //    GameObject cube = CurrentSwipeFace[i];
        //    //Transform transform = cube.transform;           
        //    GameObject newObj = Instantiate(emptyGO, cube.transform.position, cube.transform.rotation);
        //    newObj.transform.RotateAround(Vector3.zero, CurrentSwipeAxis, isCurrentSwipeClockWise ? 90 : -90);

        //    cubeTargetGameObjects.Add(newObj);
        //    cubeStartPosition.Add(cube.transform.position);
        //    cubeStartRotation.Add(cube.transform.rotation);
        //}
        CurrentSwipeDegree = 0;

    }
    public void UpdateSwipe()
    {

        if  (currentSwipeState == SwipeState.WaitForSwipe)
        {
            InitSwipe();
            currentSwipeState = SwipeState.Swiping;

        }
        else if (currentSwipeState == SwipeState.Swiping)
        {
            currentUsedTime += Time.deltaTime;
            float t = currentUsedTime / totalTime;

            //for (int i = 0; i < CurrentSwipeFace.Count; i++)
            //{
            //    GameObject cube = CurrentSwipeFace[i];
            //    Transform cubeTarget = cubeTargetGameObjects[i].transform;
            //    cube.transform.position = Vector3.Slerp(cubeStartPosition[i], cubeTarget.position, t);
            //    should move along the curve
            //    cube.transform.rotation = Quaternion.Lerp(cubeStartRotation[i], cubeTarget.rotation, t);

            //}
            float degree = Mathf.Lerp(0, 90, swipeAnimationCurve.Evaluate(t));
            float deltaDegree = degree - CurrentSwipeDegree;
            CurrentSwipeDegree = degree;

            //float deltaDegree = 3;
            foreach (GameObject cube in CurrentSwipeFace)
            {
                cube.transform.RotateAround(Vector3.zero, CurrentSwipeAxis, isCurrentSwipeClockWise ? deltaDegree : -deltaDegree);
            }
            // CurrentSwipeDegree += deltaDegree;
            if (t >=1 /*CurrentSwipeDegree >= 90*/ )
            {
                currentSwipeState = SwipeState.FinishSwipe;
            }
        }
        else if (currentSwipeState == SwipeState.FinishSwipe)
        {
            onSwipeFinished?.Invoke();
            ResetValues();
        }

    }


    public bool InitSwipeMouseDrag(Vector3 initialMousePos, Vector3 endMousePos)
    {
        initalMousePressPos = initialMousePos;
        endMousePressPos = endMousePos;
        currentMouseSwipe = (endMousePressPos - initalMousePressPos);
        GameObject FaceHit = SelectFace.GetMouseRayHitFace(initalMousePressPos);

        if (!isValidSwipe(currentMouseSwipe) || FaceHit == null)
        {
            return false;
        }
      
        currentMouseSwipe.Normalize();
        GameObject CubeHit = FaceHit.transform.parent.gameObject.transform.parent.gameObject;
        List<List<GameObject>> cubeSides = cubeState.GetAllCubeSidesFaces();
        Vector3 faceNormalAxis = Vector3.zero;
        List<Vector3> directionWorldAxes = new List<Vector3>();
        foreach (List<GameObject> cubeSide in cubeSides)
        {
            if (cubeSide.Contains(FaceHit))
            {
                if (cubeSide == cubeState.up || cubeSide == cubeState.down)
                {
                    directionWorldAxes.Add(transform.right); directionWorldAxes.Add(-transform.right);
                    directionWorldAxes.Add(transform.forward); directionWorldAxes.Add(-transform.forward);
                    faceNormalAxis = (cubeState.up == cubeSide) ? transform.up : -transform.up;

                }
                else if (cubeSide == cubeState.left || cubeSide == cubeState.right)
                {
                    directionWorldAxes.Add(transform.up); directionWorldAxes.Add(-transform.up);
                    directionWorldAxes.Add(transform.forward); directionWorldAxes.Add(-transform.forward);
                    faceNormalAxis = (cubeState.right == cubeSide) ? transform.right : -transform.right;
                }
                else if (cubeSide == cubeState.front || cubeSide == cubeState.back)
                {
                    directionWorldAxes.Add(transform.right); directionWorldAxes.Add(-transform.right);
                    directionWorldAxes.Add(transform.up); directionWorldAxes.Add(-transform.up);
                    faceNormalAxis = (cubeState.front == cubeSide) ? -transform.forward : transform.forward;
                }
            }
        }

        Vector3 SwipeAlongAxis = getClosestSwipeAxis(currentMouseSwipe, directionWorldAxes);
        CurrentSwipeAxis = Vector3.Cross(faceNormalAxis, SwipeAlongAxis).normalized;

        if (CurrentSwipeAxis == transform.right || CurrentSwipeAxis == -transform.right)
        {
            CurrentSwipeFace = SelectFaceToSwipe(new List<List<GameObject>> { cubeState.right, cubeState.left }, CubeHit);
        }
        else if (CurrentSwipeAxis == transform.up || CurrentSwipeAxis == -transform.up)
        {
            CurrentSwipeFace = SelectFaceToSwipe(new List<List<GameObject>> { cubeState.up, cubeState.down }, CubeHit);
        }
        else if (CurrentSwipeAxis == transform.forward || CurrentSwipeAxis == -transform.forward)
        {
            CurrentSwipeFace = SelectFaceToSwipe(new List<List<GameObject>> { cubeState.front, cubeState.back }, CubeHit);
        }

        isCurrentSwipeClockWise = true;
        
        return true;
    }


    public bool InitSwipeMouseScroll(bool isMouseScrollWheelForward, Vector3 mousePos)
    {
        
        GameObject FaceHit = SelectFace.GetMouseRayHitFace(mousePos);        
        if (FaceHit != null)
        {
            List<List<GameObject>> cubeSides = cubeState.GetAllCubeSidesFaces();
            foreach (List<GameObject> cubeSide in cubeSides)
            {
                if (cubeSide.Contains(FaceHit))
                {
                    if (cubeSide == cubeState.up || cubeSide == cubeState.down)
                    {
                        CurrentSwipeAxis = transform.up;
                    }
                    if (cubeSide == cubeState.left || cubeSide == cubeState.right)
                    {
                        CurrentSwipeAxis = transform.right;
                    }
                    else if (cubeSide == cubeState.front || cubeSide == cubeState.back)
                    {
                        CurrentSwipeAxis = transform.forward;
                    }
                    CurrentSwipeFace = SelectFaceToSwipe(cubeSide);
                }
            }
            isCurrentSwipeClockWise = isMouseScrollWheelForward;
           
            return true;
        }
        return false;

    }

    public bool isValidSwipe(Vector3 currentMouseSwipe)
    {
        return currentMouseSwipe.sqrMagnitude > 1;
    }


    Vector3 getClosestSwipeAxis(Vector3 mouseSwipe, List<Vector3> directionWorldAxes)
    {
        float maxVal = -Mathf.Infinity;
        Vector3 closestSwipeAxis = Vector3.zero;
        print(mouseSwipe);
        foreach (Vector3 direction in directionWorldAxes)
        {
            Vector3 screenOrigin = Camera.main.WorldToScreenPoint(Vector3.zero);
            Vector3 screenEnd = Camera.main.WorldToScreenPoint(direction);
            Vector3 screenDirectionVector = (screenEnd-screenOrigin).normalized;
            float dotProduct = Vector3.Dot(screenDirectionVector, mouseSwipe);
            if (dotProduct > maxVal)
            {
                maxVal = dotProduct;
                closestSwipeAxis = direction;
            }
        }

        return closestSwipeAxis;
    }
}
