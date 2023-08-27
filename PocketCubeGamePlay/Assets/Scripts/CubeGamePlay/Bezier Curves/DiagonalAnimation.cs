using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class DiagonalAnimation : MonoBehaviour
{
    [SerializeField]
    private Transform diagonalRoutePoints;

    [SerializeField]
    private GameObject FirstCube;

    [SerializeField]
    private GameObject SecondCube;

    [SerializeField]
    [Range(0, 5)]
    private float totalTime;

    [SerializeField]
    [Range(0, 180)]
    private float firstPhaseDegree;

    [SerializeField] private AnimationCurve animationCurve;


    //private int routeToGo;

    private float tParam;

    private Vector3 objectPosition;
    private Vector3 sec_objectPosition;

    //private bool coroutineAllowed;

 
    private List<Vector3> bCurve_one;
    private List<Vector3> bCurve_two;

    private List<Vector3> sec_bCurve_one;
    private List<Vector3> sec_bCurve_two;

    private List<Transform> path;


    //float currentUsedTime = 0;
    //float currentRotationDegree = 0;
    //float t = 0;

    // Start is called before the first frame update
    void Start()
    {
        //routeToGo = 0;
        //tParam = 0f;
        //coroutineAllowed = true;
        Application.targetFrameRate = 60;
    }

    private void Awake()
    {
        Transform p0 = diagonalRoutePoints.GetChild(0);
        Transform p1 = diagonalRoutePoints.GetChild(1);
        Transform p2 = diagonalRoutePoints.GetChild(2);
        Transform p3 = diagonalRoutePoints.GetChild(3);
        Transform p4 = diagonalRoutePoints.GetChild(4);
        Transform p5 = diagonalRoutePoints.GetChild(5);
        Transform p6 = diagonalRoutePoints.GetChild(6);

        path = new List<Transform>()
        {
            p0,
            p1,
            p2,
            p3,
            p4,
            p5,
            p6
        };

    }


    public void StartDiagonalPath(GameObject CubeStart, GameObject CubeFinish)
    {
        List<Vector3> curvePoints = InitbCurvePositionTo(path, CubeStart, CubeFinish);
        //curvePoints[6] = CubeFinish.transform.position;
        bCurve_one = new List<Vector3>
        {
            curvePoints[0],
            curvePoints[1],
            curvePoints[2],
            curvePoints[3],
        };

        bCurve_two = new List<Vector3>
        {
            curvePoints[3],
            curvePoints[4],
            curvePoints[5],
            curvePoints[6],
        };

        

        List<Vector3> second_curvePoints = InitbCurvePositionTo(path, CubeFinish, CubeStart);
        //second_curvePoints[6] = CubeStart.transform.position;
        sec_bCurve_one = new List<Vector3>
        {
            second_curvePoints[0],
            second_curvePoints[1],
            second_curvePoints[2],
            second_curvePoints[3],
        };

        sec_bCurve_two = new List<Vector3>
        {
            second_curvePoints[3],
            second_curvePoints[4],
            second_curvePoints[5],
            second_curvePoints[6],
        };


        StartCoroutine(GoByTheRoute(CubeStart, CubeFinish, bCurve_one, bCurve_two, sec_bCurve_one, sec_bCurve_two));

        //coroutineAllowed = false;
        //yield return null;
    }


    private List<Vector3> InitbCurvePositionTo(List<Transform> path, GameObject CubeStart, GameObject CubeFinish)
    {
            
        Vector3 moveTo = CubeStart.transform.position - path[0].position;
        
        Vector3 curveDirection = path[6].position - path[0].position;

        Vector3 cubeTranslationDirection = CubeFinish.transform.position - CubeStart.transform.position;

        float angle = Vector3.Angle(curveDirection, cubeTranslationDirection);

        Vector3 axis = Vector3.forward;// should be face normal

        List<Vector3> curvePoints = new List<Vector3>();

        for (int i =0; i<path.Count; i++)
        {
            path[i].position += moveTo;
            path[i].transform.RotateAround(path[0].position, axis, angle);
            curvePoints.Add(path[i].position);
        }

        curvePoints[6] = CubeFinish.transform.position;
        return curvePoints;
    }


    private IEnumerator GoByTheRoute(GameObject CubeStart, GameObject CubeFinish, List<Vector3> bCurve_one, List<Vector3> bCurve_two, List<Vector3> sec_bCurve_one, List<Vector3> sec_bCurve_two)
    {
        // translate route to the position of the cube
        // rotate the route around the first pivot point to match the start and end pos

        float currentUsedTime = 0;
        float currentRotationDegree = 0;
        float t = 0;
        while (t < 1)
        {
            //tParam += Time.deltaTime;

            currentUsedTime += Time.deltaTime;
            t = currentUsedTime / totalTime;

            float angle = Mathf.Lerp(0, firstPhaseDegree, t);
            float deltaAngle = angle - currentRotationDegree;
            currentRotationDegree = angle;

            tParam = t;
            objectPosition = Mathf.Pow(1 - tParam, 3) * bCurve_one[0] +
                             3 * Mathf.Pow(1 - tParam, 2) * tParam * bCurve_one[1]+
                             3 * (1 - tParam) * Mathf.Pow(tParam, 2) * bCurve_one[2] +
                             Mathf.Pow(tParam, 3) * bCurve_one[3];

            CubeStart.transform.position = objectPosition;
            CubeStart.transform.RotateAround(CubeStart.transform.position, CubeStart.transform.forward, deltaAngle);

            sec_objectPosition = Mathf.Pow(1 - tParam, 3) * sec_bCurve_one[0] +
                             3 * Mathf.Pow(1 - tParam, 2) * tParam * sec_bCurve_one[1] +
                             3 * (1 - tParam) * Mathf.Pow(tParam, 2) * sec_bCurve_one[2] +
                             Mathf.Pow(tParam, 3) * sec_bCurve_one[3];

            CubeFinish.transform.position = sec_objectPosition;
            CubeFinish.transform.RotateAround(CubeFinish.transform.position, CubeFinish.transform.forward, deltaAngle);
            yield return null;
        }

        currentUsedTime = 0;
        currentRotationDegree = 0;
        t = 0;
        while (t < 1)
        {

            currentUsedTime += Time.deltaTime;
            t = currentUsedTime / totalTime;

            float angle = Mathf.Lerp(0, 180 - firstPhaseDegree, animationCurve.Evaluate(t));
            float deltaAngle = angle - currentRotationDegree;
            currentRotationDegree = angle;

            //tParam = t; // += Time.deltaTime;

            objectPosition = Mathf.Pow(1 - t, 3) * bCurve_two[0] +
                             3 * Mathf.Pow(1 - t, 2) * t * bCurve_two[1] +
                             3 * (1 - t) * Mathf.Pow(t, 2) * bCurve_two[2] +
                             Mathf.Pow(t, 3) * bCurve_two[3];

            CubeStart.transform.position = objectPosition;
            CubeStart.transform.RotateAround(CubeStart.transform.position, CubeStart.transform.forward, deltaAngle);

            sec_objectPosition = Mathf.Pow(1 - t, 3) * sec_bCurve_two[0] +
                 3 * Mathf.Pow(1 - t, 2) * t * sec_bCurve_two[1] +
                 3 * (1 - t) * Mathf.Pow(t, 2) * sec_bCurve_two[2] +
                 Mathf.Pow(t, 3) * sec_bCurve_two[3];

            CubeFinish.transform.position = sec_objectPosition;
            CubeFinish.transform.RotateAround(CubeFinish.transform.position, CubeFinish.transform.forward, deltaAngle);

            yield return new WaitForEndOfFrame();
        }



        yield return new WaitForEndOfFrame();

    }

}
