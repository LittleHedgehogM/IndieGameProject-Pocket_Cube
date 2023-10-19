using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Origin_SphereRotator : MonoBehaviour
{

    [SerializeField] Camera mainCam;

    private GameObject checkCollision()
    {
        GameObject hitObject = null;
        RaycastHit hit;
        Ray ray;
        ray = mainCam.ScreenPointToRay(Input.mousePosition);

        if (Input.GetMouseButtonDown(0) && Physics.Raycast(ray, out hit))
        {
            hitObject = hit.collider.gameObject;
            print("Hit Object" + hitObject.name);
        }
        return hitObject;
    }

    Vector2 mouseDownPos;
    Vector2 mouseUpPos;
    private bool isSphereHit  = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        GameObject hitObject = null;
        RaycastHit hit;
        Ray ray;
        ray = mainCam.ScreenPointToRay(Input.mousePosition);

        if (Input.GetMouseButtonDown(0) && Physics.Raycast(ray, out hit))
        {
            hitObject = hit.collider.gameObject;
            if (hitObject != null && hitObject.gameObject.name == "SphereCollider")
            {
                mouseDownPos = mainCam.ScreenToViewportPoint(Input.mousePosition); 
                isSphereHit = true;
            }
        }


        if (Input.GetMouseButton(0) && isSphereHit)
        {
            mouseUpPos = mainCam.ScreenToViewportPoint(Input.mousePosition);
            Vector3 direction =  mouseUpPos- mouseDownPos;  
            transform.rotation = Quaternion.Euler(direction.y*100, -direction.x * 100, 0) * transform.rotation;

            mouseDownPos = mainCam.ScreenToViewportPoint(Input.mousePosition);
        }

        if (Input.GetMouseButtonUp(0))
        {
            isSphereHit = false;
        }

    }
}
