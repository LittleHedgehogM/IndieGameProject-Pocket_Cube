using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateWholeCube : MonoBehaviour
{
    Vector2 initalPressPos;
    Vector2 endPressPos;
    Vector2 currentSwipe;
    //int swipeCount;
    public GameObject SwipeTarget;
    public float SwipeSpeed;
    Vector3 previousMousePosition;
    Vector2 mouseDelta;

    // Start is called before the first frame update
    void Start()
    {
        //swipeCount = 0;
    }

    // Update is called once per frame
    void Update()
    {
        SwipeWholeCube();
        DragWholeCube();
    }
    
    void DragWholeCube()
    {
        if (Input.GetMouseButton(1))
        {
            mouseDelta = Input.mousePosition - previousMousePosition;
            mouseDelta *= 0.5f;
            transform.rotation = Quaternion.Euler(mouseDelta.y, -mouseDelta.x, 0)*transform.rotation;
        }
        //else 
        //{
        //    if (transform.rotation != SwipeTarget.transform.rotation)
        //    {
        //        var step = SwipeSpeed * Time.deltaTime;
        //        transform.rotation = Quaternion.RotateTowards(transform.rotation, SwipeTarget.transform.rotation, step);
        //    }
        //}
        previousMousePosition = Input.mousePosition;

    }

    void SwipeWholeCube()
    {
        currentSwipe = Vector2.zero;

        if (Input.GetMouseButtonDown(1))
        {
            //get the 2D position of the first mouse position
            initalPressPos = Input.mousePosition; //new Vector2(Input.mousePosition.x, Input.mousePosition.y);
            //print( "initial position : "+ initalPressPos);
        }

        if (Input.GetMouseButtonUp(1))
        {
            // get the 2D position of the second mouse position
            endPressPos = Input.mousePosition; //new Vector2(Input.mousePosition.x, Input.mousePosition.y);
            currentSwipe = (endPressPos - initalPressPos);
            currentSwipe.Normalize();

        }

        if (currentSwipe != Vector2.zero)
        {
            //swipeCount++;
            if (IsLeftSwipe(currentSwipe))
            {                
                SwipeTarget.transform.Rotate(0, 90, 0,  Space.World);                
                //print(swipeCount + "Is left swipe");
            }
            else if (IsRightSwipe(currentSwipe))
            {
                SwipeTarget.transform.Rotate(0, -90, 0, Space.World);
                //print(swipeCount + "Is right swipe");
            }
            else if (IsUpLeftSwipe(currentSwipe))
            {
                SwipeTarget.transform.Rotate(0, 0, 90, Space.World);
                //print(swipeCount + "Is left up swipe");
            }
            else if (IsUpRightSwipe(currentSwipe))
            {
                SwipeTarget.transform.Rotate(90, 0, 0, Space.World);
                //print(swipeCount + "Is left down swipe");
            }
            else if (IsDownLeftSwipe(currentSwipe))
            {
                SwipeTarget.transform.Rotate(-90, 0, 0, Space.World);
                //print(swipeCount + "Is right up swipe");
            }
            else if (IsDownRightSwipe(currentSwipe))
            {
                SwipeTarget.transform.Rotate(0, 0, -90, Space.World);
                //print(swipeCount + "Is right down swipe");
            }
        }

    }

    bool IsLeftSwipe(Vector2 swipe)
    {
        return swipe.x < 0 && swipe.y > -0.5f && swipe.y < 0.5f;
    }
    bool IsRightSwipe(Vector2 swipe)
    {
        return swipe.x > 0 && swipe.y > -0.5f && swipe.y < 0.5f;
    }
    bool IsUpLeftSwipe(Vector2 swipe)
    {
        return swipe.x < 0 && swipe.y > 0;
    }

    bool IsUpRightSwipe(Vector2 swipe)
    {
        return swipe.x > 0 && swipe.y > 0;
    }
    bool IsDownLeftSwipe(Vector2 swipe)
    {
        return swipe.x < 0 && swipe.y < 0;
    }
    bool IsDownRightSwipe(Vector2 swipe)
    {
        return swipe.x > 0 && swipe.y < 0;
    }
}