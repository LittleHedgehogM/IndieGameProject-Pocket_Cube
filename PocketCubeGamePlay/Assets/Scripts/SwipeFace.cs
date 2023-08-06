using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class SwipeFace : MonoBehaviour
{
    public GameObject FrontLeftUp;
    public GameObject FrontRightUp;
    public GameObject FrontLeftDown;
    public GameObject FrontRightDown;
    public GameObject BackLeftUp;
    public GameObject BackRightUp;
    public GameObject BackLeftDown;
    public GameObject BackRightDown;

    //List<GameObject> CubeList;

    //SwipeDirection currentSwipeDirection;
    //public enum SwipeDirection
    //{
    //    NoSwipe,
    //    LeftSwipe, 
    //    RightSwipe,
    //    UpLeftSwipe,
    //    DownLeftSwipe,
    //    UpRightSwipe,
    //    DownRightSwipe
    //};

    Vector3 initalMousePressPos;
    Vector3 endMousePressPos;
    Vector3 currentMouseSwipe;

    List<GameObject> CurrentSwipeFace;
    Vector3 CurrentSwipeAxis;
    int CurrentSwipeDegree = 0;
    bool isCurrentSwipeClockWise = true;
    SelectFace selectFace;
    CubeState cubeState;
    ReadCube readCube;
    enum SwipeState 
    { 
        WaitForSwipe,
        Swiping,
        FinishSwipe
    };

    SwipeState currentSwipeState;

    class SwipeCommand
    {
        public List<GameObject> CubesToSwipe;
        public Vector3 Axis;
        public bool isClockWise;
        float currentDegree;

        public SwipeCommand (List<GameObject> CubesToSwipe, Vector3 Axis, bool isClockWise)
        {
            this.CubesToSwipe = CubesToSwipe;
            this.Axis = Axis;
            this.isClockWise = isClockWise;
            currentDegree = 0;
        }


        public void incrementDegree(float degreeIncrement)
        {
            currentDegree += degreeIncrement;
        }

        public bool hasFinishSwipe()
        {
            return currentDegree == 90.0f;
        }
        

    }

    // Start is called before the first frame update
    void Start()
    {
        selectFace      = FindObjectOfType<SelectFace>();
        cubeState       = FindObjectOfType<CubeState>();
        readCube        = FindObjectOfType<ReadCube>();
        currentSwipeState = SwipeState.WaitForSwipe;
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
        //List<GameObject> FaceToSwipe;
        List<GameObject> faceToSelect = new List<GameObject>();
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

    // Update is called once per frame
    void Update()
    {
        if (currentSwipeState == SwipeState.Swiping)
        {
            if (CurrentSwipeDegree >= 90)
            {
                currentSwipeState = SwipeState.FinishSwipe;
            }
            else
            {
                const int deltaDegree = 5;
                foreach (GameObject cube in CurrentSwipeFace)
                {
                   cube.transform.RotateAround(Vector3.zero, CurrentSwipeAxis, isCurrentSwipeClockWise? deltaDegree  : -deltaDegree);
                }
                CurrentSwipeDegree += deltaDegree;
            }                        
        }
        else if (currentSwipeState == SwipeState.FinishSwipe)
        {
            currentSwipeState = SwipeState.WaitForSwipe;
            CurrentSwipeDegree = 0;
            CurrentSwipeAxis = Vector3.zero;
            isCurrentSwipeClockWise = false;
            
            CurrentSwipeFace.Clear();
        }
        else if (currentSwipeState == SwipeState.WaitForSwipe)
        {
            bool isMouseScrollWheelForward  = Input.GetAxis("Mouse ScrollWheel") > 0f;
            bool isMouseScrollWheelBackward = Input.GetAxis("Mouse ScrollWheel") < 0f;
            bool isRotatingWholeCube = Input.GetMouseButton(1);
            if (isRotatingWholeCube)
            {
                return;
            }

            readCube.ReadState();

            if (Input.GetMouseButtonDown(0))
            {
                initalMousePressPos = Input.mousePosition; 
            }
            else if (Input.GetMouseButtonUp(0))
            {
                endMousePressPos = Input.mousePosition; 
                currentMouseSwipe = (endMousePressPos - initalMousePressPos);
                currentMouseSwipe.Normalize();
                GameObject FaceHit = selectFace.GetMouseRayHitFace(initalMousePressPos);
                List<List<GameObject>> cubeSides = cubeState.GetAllCubeSides();
                GameObject CubeHit = FaceHit.transform.parent.gameObject.transform.parent.gameObject;

                Vector3 faceNormalAxis = Vector3.zero ;

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
                //print("face normal Axis :" + faceNormalAxis + ", SwipeAlongAxis : " + SwipeAlongAxis);

                CurrentSwipeAxis = Vector3.Cross(faceNormalAxis, SwipeAlongAxis).normalized;

                if (CurrentSwipeAxis == transform.right || CurrentSwipeAxis == -transform.right)
                {
                    CurrentSwipeFace = SelectFaceToSwipe(new List<List<GameObject>> { cubeState.right, cubeState.left},CubeHit);
                    //print("Swipe Axis = right");
                }
                else if (CurrentSwipeAxis == transform.up || CurrentSwipeAxis == -transform.up)
                {
                    CurrentSwipeFace = SelectFaceToSwipe(new List<List<GameObject>> { cubeState.up, cubeState.down }, CubeHit);
                    //print("Swipe Axis = up");
                }               
                else if (CurrentSwipeAxis == transform.forward || CurrentSwipeAxis == -transform.forward)
                {
                    CurrentSwipeFace = SelectFaceToSwipe(new List<List<GameObject>> { cubeState.front, cubeState.back }, CubeHit);
                    //print("Swipe Axis = forward");
                }
                
                isCurrentSwipeClockWise = true;
                //// get cubes to swipe
                currentSwipeState = SwipeState.Swiping;


            }
            else if ( isMouseScrollWheelForward || isMouseScrollWheelBackward )
            {
                // get current mouse hit face, swipe around it;
                GameObject FaceHit = selectFace.GetMouseRayHitFace();              
                if ( FaceHit!=null )
                {
                    List<List<GameObject>> cubeSides = cubeState.GetAllCubeSides();
                  
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
                    currentSwipeState = SwipeState.Swiping;
                }            
            
            }
        }
 
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
            //print("direction : " + direction  + ", screen direction =  " + screenDirectionVector + ", dot product = " + dotProduct);
        }

        //print("closet direction : " + closestSwipeAxis);
        return closestSwipeAxis;
    }
}
