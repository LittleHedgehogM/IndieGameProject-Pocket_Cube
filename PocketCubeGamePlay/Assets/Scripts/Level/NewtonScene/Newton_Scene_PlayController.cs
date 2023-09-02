using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class NewtonScenePlayController : MonoBehaviour
{
    DetectDistance myDetectDistance;
    Scene_Newton_Camera_Controller myCameraController;
    Newton_Scene_PlayerMovement myPlayerMovement;
    [SerializeField] [Range(0, 3)] private float dist_threshold;
    [SerializeField] [Range(0, 3)] private float  leave_dist_threshold;


    enum PlayStatus
    {
        Configuration,
        PlayerMovement,
        InDisplayEyeAndScale,
        InPutCoin,
        InTakeCoin,
        InScaleDraw,
        InScalePositionAdjustment,
        InCameraTranslation,
    }
    PlayStatus myPlayStatus;

    [SerializeField] 
    GameObject Scale_Left;

    [SerializeField] 
    GameObject Scale_Right;

    [SerializeField]
    GameObject coin1; //A

    [SerializeField]
    GameObject coin2; //A

    [SerializeField]
    GameObject coin3; //B

    [SerializeField]
    GameObject coin4; //B

    [SerializeField]
    GameObject coin5; //C

    [SerializeField]
    GameObject player;
    
    [SerializeField]
    GameObject Cube;

    void Start()
    {
        myDetectDistance = FindObjectOfType<DetectDistance>();
        myPlayerMovement = FindObjectOfType<Newton_Scene_PlayerMovement>();
        myCameraController = FindAnyObjectByType<Scene_Newton_Camera_Controller>();


        Scale_Left.GetComponent<Scale>().insertCoin(coin1);
        Scale_Left.GetComponent<Scale>().insertCoin(coin2);
        Scale_Left.GetComponent<Scale>().insertCoin(coin3);

        Scale_Right.GetComponent<Scale>().insertCoin(coin4);
        Scale_Right.GetComponent<Scale>().insertCoin(coin5);

        Scale_Left.GetComponent<Scale>().InitScalePosition();
        Scale_Right.GetComponent<Scale>().InitScalePosition();

        myPlayStatus = PlayStatus.Configuration;


    }


    private void OnEnable()
    {
        Scene_Newton_Camera_Controller.zoomInFinish += onCameraTranslationFinish;
        Scene_Newton_Camera_Controller.ResetFinish  += onCameraResetFinish;
    }

    private void OnDisable()
    {
        Scene_Newton_Camera_Controller.zoomInFinish -= onCameraTranslationFinish;
        Scene_Newton_Camera_Controller.ResetFinish  -= onCameraResetFinish;

    }

    public void onCameraTranslationFinish()
    {
        if (myPlayStatus == PlayStatus.InCameraTranslation)
        {
            myPlayStatus = PlayStatus.InDisplayEyeAndScale;
            myPlayerMovement.setEnableMovement(true);

        }
        else if (myPlayStatus == PlayStatus.InScalePositionAdjustment)
        {
            myPlayStatus = PlayStatus.InDisplayEyeAndScale;
        }
    }

    public void onCameraResetFinish()
    {

    }


    GameObject currentScale;

    // Update is called once per frame
    void Update()
    {
        if (myPlayStatus == PlayStatus.InScaleDraw)
        {
            //.....
        }
        else if (myPlayStatus == PlayStatus.Configuration)
        {
            myPlayStatus = PlayStatus.PlayerMovement;
        }

        else if (myPlayStatus == PlayStatus.PlayerMovement)
        {
            // check if draw
            int leftWeight  = Scale_Left.GetComponent<Scale>().getTotalWeight();
            int rightWeight = Scale_Right.GetComponent<Scale>().getTotalWeight();

            if (leftWeight == 5 && rightWeight == 5)
            {
                Cube.GetComponent<Rigidbody>().isKinematic = false;
                myPlayStatus = PlayStatus.InScaleDraw;
                return;
            }

            // player Movement and camera movement Update
            myPlayerMovement.OnUpdate();
            myCameraController.onUpdateCameraWithPlayerMovement(myPlayerMovement.getMovementDirection());

            float Dist_L = myDetectDistance.getDistanceBetweenPlayerAndLeftEye();
            float Dist_R = myDetectDistance.getDistanceBetweenPlayerAndRightEye();

            if (Dist_L < dist_threshold)
            {
                //if (Input.GetKeyDown(KeyCode.K))
                //{
                    myCameraController.showLeftEye();
                    myPlayerMovement.setEnableMovement(false);
                    myPlayerMovement.TranslateToLeftEye();
                    myPlayStatus = PlayStatus.InCameraTranslation;
                    currentScale = Scale_Left;
                //}

            }
            else if (Dist_R < dist_threshold)
            {
                //if (Input.GetKeyDown(KeyCode.K))
                //{
                    myCameraController.showRightEye();
                    myPlayerMovement.setEnableMovement(false);
                    myPlayerMovement.TranslateToRightEye();
                    myPlayStatus = PlayStatus.InCameraTranslation;
                    currentScale = Scale_Right;
                //}

            }
            
        }
        //else if (myPlayStatus == PlayStatus.InCameraTranslation)
        //{
        //    myPlayerMovement.setEnableMovement(false);

        //}
        else if (myPlayStatus == PlayStatus.InDisplayEyeAndScale)
        {
            myPlayerMovement.OnUpdate();
            float Dist_L = myDetectDistance.getDistanceBetweenPlayerAndLeftEye();
            float Dist_R = myDetectDistance.getDistanceBetweenPlayerAndRightEye();
            if (Dist_L >= leave_dist_threshold && Dist_R >= leave_dist_threshold)
            {
                myCameraController.resetCam();
                myPlayerMovement.setEnableMovement(true);
                myPlayStatus = PlayStatus.PlayerMovement;
            }

            //    if (Input.GetKeyDown(KeyCode.K))
            //{
            //    myCameraController.resetCam();
            //    myPlayerMovement.setEnableMovement(true);
            //    myPlayStatus = PlayStatus.PlayerMovement;
            //}
            //else

            if (Input.GetKeyDown(KeyCode.P))
            {
                EquipCoin playerEquipCoin = player.GetComponent<EquipCoin>();
                if (playerEquipCoin.getEquipped() != null) // if player has a coin
                {
                    myPlayStatus = PlayStatus.InPutCoin;
                    // play Animation to drop coin                    

                }
                else // if player doesn't have a coin
                {
                    myPlayStatus = PlayStatus.InTakeCoin;
                    if (currentScale == Scale_Left)
                    {
                        myCameraController.showLeftcale();

                    }
                    else if (currentScale == Scale_Right)
                    {
                        myCameraController.showRightScale();
                    }

                }
            }

        }
        else if (myPlayStatus == PlayStatus.InPutCoin)
        {
            // if can put coin
            EquipCoin playerEquipCoin = player.GetComponent<EquipCoin>();
            GameObject coin = playerEquipCoin.getEquipped();
            if (coin != null)
            {
                playerEquipCoin.Drop();
                currentScale.GetComponent<AddNewCoin>().AddCoin(coin);
                myPlayStatus = PlayStatus.InDisplayEyeAndScale;
                StartCoroutine(currentScale.GetComponent<Scale>().UpdatePosition());
            }

        }
        else if (myPlayStatus == PlayStatus.InTakeCoin)
        {
            myPlayerMovement.setEnableMovement(false);
            EquipCoin playerEquipCoin = player.GetComponent<EquipCoin>();
            if (Input.GetMouseButtonUp(0))
            {
                //GameObject coinHit = null;
                RaycastHit hit;
                Ray ray;
                ray = myCameraController.getCurrentCamera().ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray, out hit))
                {
                    GameObject coinHit = hit.collider.gameObject;
                    if (coinHit == coin1 || coinHit == coin2 || coinHit == coin3
                        || coinHit == coin4 || coinHit == coin5)
                    {

                        //coinHit.GetComponent<Rigidbody>().isKinematic = true;
                        
                        currentScale.GetComponent<Scale>().popCoin(coinHit);
                        playerEquipCoin.Equip(coinHit);

                        if (currentScale == Scale_Left)
                        {
                            myCameraController.showLeftEye();
                        }
                        else if (currentScale == Scale_Right)
                        {
                            myCameraController.showRightEye();
                        }
                        //myCameraController.TranslateBackToInitPosition();
                        //myPlayStatus = PlayStatus.InDisplayEyeAndScale;
                        StartCoroutine(currentScale.GetComponent<Scale>().UpdatePosition());
                        myPlayStatus = PlayStatus.InCameraTranslation;
                    }
                }
            }
        }
        else if (myPlayStatus == PlayStatus.InScalePositionAdjustment)
        {
            

        }
    }

}
