using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Follow : MonoBehaviour
{
    [SerializeField]
    private Transform[] routes;

    [SerializeField]
    private GameObject FirstCube;

    [SerializeField]
    [Range(0, 2)]
    private float totalTime;

    private int routeToGo;

    private float tParam;

    private Vector3 objectPosition;

    //private float speedModifier;

    private bool coroutineAllowed;

    // Start is called before the first frame update
    void Start()
    {
        routeToGo = 0;
        tParam = 0f;
        //speedModifier = 0.5f;
        coroutineAllowed = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (coroutineAllowed)
        {
            StartCoroutine(GoByTheRoute(routeToGo));
        }
    }

    

    private IEnumerator GoByTheRoute(int routeNum)
    {

        // translate route to the position of the cube
        // rotate the route around the first pivot point to match the start and end pos

        coroutineAllowed = false;

        Vector3 p0 = routes[routeNum].GetChild(0).position;
        Vector3 p1 = routes[routeNum].GetChild(1).position;
        Vector3 p2 = routes[routeNum].GetChild(2).position;
        Vector3 p3 = routes[routeNum].GetChild(3).position;



        //float totalTime = 2.0f;
        float currentUsedTime = 0;
        float currentRotationDegree = 0;
        
        while (tParam < 1)
        {

            currentUsedTime += Time.deltaTime;
            float t = currentUsedTime / totalTime;

            float angle = Mathf.Lerp(0, 90, t);
            float deltaAngle = angle - currentRotationDegree;
            currentRotationDegree = angle;

            tParam = t; // += Time.deltaTime;

            objectPosition = Mathf.Pow(1 - tParam, 3) * p0 + 
                             3 * Mathf.Pow(1 - tParam, 2) * tParam * p1 + 
                             3 * (1 - tParam) * Mathf.Pow(tParam, 2) * p2 + 
                             Mathf.Pow(tParam, 3) * p3;

            FirstCube.transform.position = objectPosition;

            FirstCube.transform.RotateAround(FirstCube.transform.position, FirstCube.transform.forward, deltaAngle);



            yield return null; // new WaitForEndOfFrame();
        }

        tParam = 0;
        routeToGo += 1;

        if (routeToGo > routes.Length - 1)
        {
            routeToGo = 0;
            coroutineAllowed = false;
            // finish route
        }
        else
        {
            coroutineAllowed = true;
        }

        
        

    }
}
