using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReadCube : MonoBehaviour
{

    public Transform tUp;
    public Transform tDown;
    public Transform tLeft;
    public Transform tRight;
    public Transform tFront;
    public Transform tBack;
    public GameObject emptyGO;

    private List<GameObject> frontRays  = new List<GameObject>();
    private List<GameObject> backRays   = new List<GameObject>();
    private List<GameObject> leftRays   = new List<GameObject>();
    private List<GameObject> rightRays  = new List<GameObject>();
    private List<GameObject> upRays     = new List<GameObject>();
    private List<GameObject> downRays   = new List<GameObject>();


    private int layerMask = 1 << 8;
    CubeState cubeState;
    CubeMap cubeMap;


    // Start is called before the first frame update
    void Start()
    {
        cubeState = FindObjectOfType<CubeState>();
        cubeMap   = FindObjectOfType<CubeMap>();
        SetRayTransform();
        ReadState();
    }

    public List<Transform> getAllTransformAxis()
    {
        return new List<Transform> { tUp, tDown, tLeft, tRight, tFront, tBack };
    }

    public void ReadState()
    {
        cubeState = FindObjectOfType<CubeState>();
        cubeMap   = FindObjectOfType<CubeMap>();

        cubeState.front = ReadFace(frontRays, tFront);
        cubeState.right = ReadFace(rightRays, tRight);
        cubeState.left  = ReadFace(leftRays, tLeft);
        cubeState.up    = ReadFace(upRays, tUp);
        cubeState.down  = ReadFace(downRays, tDown);
        cubeState.back  = ReadFace(backRays, tBack);
        cubeMap.Set();
        
    }

    void SetRayTransform()
    {
        //populate the ray lists with raycasts eminating from the transform, angled towards the cube.
        upRays    = BuildRays(tUp,      new Vector3(90, 0, 0));
        downRays  = BuildRays(tDown,    new Vector3(270, 0, 0));
        leftRays  = BuildRays(tLeft,    new Vector3(0, 90, 0));
        rightRays = BuildRays(tRight,   new Vector3(0, 270, 0));        
        frontRays = BuildRays(tFront,   new Vector3(0, 0, 0));
        backRays  = BuildRays(tBack,    new Vector3(0, 180, 0));              

    }

    // Build 4 Rays
    List<GameObject> BuildRays(Transform rayTransform, Vector3 direction) 
    {
        int rayCount = 0;

        List<GameObject> rays = new List<GameObject>();
        
        // ray 0 at the top left and ray 4 at the bottom right;

        // 0 | 1
        // 2 | 3

        for (float y = 0.5f; y >= -0.5f; y--)
        {
            for (float x = -0.5f; x <= 0.5f; x++)
            {
                Vector3 startPos = new Vector3 (rayTransform.localPosition.x + x,
                                                rayTransform.localPosition.y + y,
                                                rayTransform.localPosition.z);

                GameObject rayStart = Instantiate(emptyGO, startPos, Quaternion.identity, rayTransform);
                rayStart.name = rayCount.ToString();
                rays.Add(rayStart);
                rayCount++;
                    
            }
        }

        rayTransform.localRotation = Quaternion.Euler(direction);
        return rays;
    }


    // Update is called once per frame
    void Update()
    {

    }

    public List<GameObject> ReadFace(List<GameObject> rayStarts, Transform rayTransform)
    {
        List<GameObject> facesHit = new List<GameObject>();

        foreach (GameObject rayStart in rayStarts)
        {
            Vector3 ray = rayStart.transform.position;
            RaycastHit hit;
            if (Physics.Raycast(ray, rayTransform.forward, out hit, Mathf.Infinity, layerMask))
            {
                Debug.DrawRay(ray, rayTransform.forward * hit.distance, Color.yellow);
                facesHit.Add(hit.collider.gameObject);
                //print(hit.collider.gameObject.name);
            }
            else
            {
                Debug.DrawRay(ray, rayTransform.forward * 1000, Color.green);
            }
        }       

        return facesHit;

    }

}
