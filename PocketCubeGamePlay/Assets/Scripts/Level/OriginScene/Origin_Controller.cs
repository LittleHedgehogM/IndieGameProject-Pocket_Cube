using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static System.TimeZoneInfo;

public class Origin_Controller : MonoBehaviour
{
    public GameObject Sphere;

    [SerializeField] private Camera mainCam;
    [SerializeField] private GameObject leftAxis;
    [SerializeField] private GameObject rightAxis;  

    [SerializeField] private GameObject upAxis;
    [SerializeField] private GameObject downAxis;

    [SerializeField] [Range (30, 90)] float angleGapX;
    [SerializeField][Range(30, 90)] float angleGapY;

    [SerializeField][Range(0.5f, 3f)]  private float translationTime;


    bool enableInteraction = true;
    private IEnumerator Rotate(Vector3 Axis, bool isClockwise, float angleGap)
    {
        float currentUsedTime = 0;
        float t = 0;
        float currentAngle = 0f;
        while (t < 1)
        {
            currentUsedTime += Time.deltaTime;
            t = currentUsedTime / translationTime;

            float targetAngle = Mathf.Lerp(0, angleGap, t);
            float deltaAngle = targetAngle - currentAngle ;
            currentAngle = targetAngle ;
            Sphere.transform.RotateAround(Sphere.transform.position, Axis, isClockwise ? deltaAngle : -deltaAngle) ;

            yield return null;
        }
        enableInteraction = true;

    }

    float currentUpGap = 108.0f;
    float currentDownGap = 72.0f;
    private float getNextAngleGap(float currentGap)
    {
        if (currentGap == 72.0f)
        {
            currentGap = 108.0f;
        }
        else 
        {
            currentGap = 72.0f;
        }

        return currentGap;
    }

    // Update is called once per frame
    void Update()
    {
        GameObject hitObject = null;
        RaycastHit hit;
        Ray ray;
        ray = mainCam.ScreenPointToRay(Input.mousePosition);
        if (enableInteraction) 
        {
            if (Input.GetMouseButtonUp(0) && Physics.Raycast(ray, out hit))
            {
                hitObject = hit.collider.gameObject;
                if (hitObject != null && hitObject.gameObject == leftAxis)
                {
                    Debug.Log("Left Axis Hit");
                    StartCoroutine(Rotate(Vector3.up, true, angleGapX));
                    enableInteraction = false;
                }
                else if (hitObject != null && hitObject.gameObject == rightAxis)
                {
                    Debug.Log("Right Axis Hit"); 
                    //Sphere.transform.Rotate(new Vector3(0, -angleGap, 0));
                    StartCoroutine(Rotate(Vector3.up, false, angleGapX));
                    enableInteraction = false;
                }
                else if (hitObject != null && hitObject.gameObject == upAxis)
                {
                    Debug.Log("Up Axis Hit" + currentUpGap);
                    StartCoroutine(Rotate(Vector3.right, true, currentUpGap));
                    currentUpGap = getNextAngleGap(currentUpGap);
                    enableInteraction = false;
                }
                else if (hitObject != null && hitObject.gameObject == downAxis)
                {
                    Debug.Log("Down Axis Hit" + currentDownGap);
                    StartCoroutine(Rotate(Vector3.right, false, currentDownGap));
                    currentDownGap = getNextAngleGap(currentDownGap);
                    enableInteraction = false;
                }
            }
        }
       
    }
}
