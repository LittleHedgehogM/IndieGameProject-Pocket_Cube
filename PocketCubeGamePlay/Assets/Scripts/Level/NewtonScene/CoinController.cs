//using System.Collections;
//using System.Collections.Generic;
//using UnityEditor;
//using UnityEngine;

//public class CoinController : MonoBehaviour
//{
//    enum CoinStatus
//    {
//        OnScale,
//        DropToScale,
//        Selected,
//        WithPlayer,
//        Other,
//    }

//    CoinStatus currentStatus;
//    private bool enableMouseDrag;

//    GameObject currentScale;
//    Camera currentCamera;

//    // Start is called before the first frame update
//    void Start()
//    {
        
//    }

    
//    // Update is called once per frame
//    void Update()
//    {
//        if (currentStatus == CoinStatus.OnScale)
//        {
//            // enable mouse drag 
//            // can be selected 
//        }
//        else if (currentStatus == CoinStatus.DropToScale)
//        {
//            // drop on scale 
//            // in a very short time
//            // when it stablizes, set it to OnScale
//        }
//        else if (currentStatus == CoinStatus.Selected)
//        {
//            // set is kinematic to true
//            // move coin out of the scale and show confirm button

//        }
//        else if (currentStatus == CoinStatus.WithPlayer)
//        {
//            // follow player's movement
//        }
       
//    }

//    public void SetCamera(Camera camera)
//    {
//        currentCamera = camera;
//    }

//    public void SetScale(GameObject scale)
//    {
//        currentScale = scale;
//    }
//}
