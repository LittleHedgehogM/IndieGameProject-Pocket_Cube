using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class NewtonScenePlayController : MonoBehaviour
{
    DetectDistance myDetectDistance;
    Scene_Newton_Camera_Controller myCameraController;
    Newton_Scene_PlayerMovement myPlayerMovement;
    Newton_Scene_VFX_Controller myVFXController;
    LevelLoaderScript myLevelLoader;


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

    [SerializeField]
    Animator cubeTransition;

    GameObject currentScale;

    [SerializeField]
    Animator REyeAnimator;
    [SerializeField]
    Animator LEyeAnimator;

    [SerializeField]
    Animator REyeBallAnimator;
    [SerializeField]
    Animator LEyeBallAnimator;

    //[SerializeField]
    //Animator RCoinAnimator;
    //[SerializeField]
    //Animator LCoinAnimator;

    bool isPlayingSwallowAnimation = false;
    bool isPlayingSpriteAnimation = false;
    GameObject activeCoin = null;

    void Start()
    {
        myDetectDistance    = FindObjectOfType<DetectDistance>();
        myPlayerMovement    = FindObjectOfType<Newton_Scene_PlayerMovement>();
        myCameraController  = FindObjectOfType<Scene_Newton_Camera_Controller>();
        myVFXController     = FindObjectOfType<Newton_Scene_VFX_Controller>();
        myLevelLoader       = FindObjectOfType<LevelLoaderScript>();

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
        Newton_Scene_PlayerMovement.PlayerCollideWithCube += LoadNextLevel;
        
    }

    private void OnDisable()
    {
        Scene_Newton_Camera_Controller.zoomInFinish -= onCameraTranslationFinish;
        Scene_Newton_Camera_Controller.ResetFinish  -= onCameraResetFinish;
        Newton_Scene_PlayerMovement.PlayerCollideWithCube += LoadNextLevel;

    }

    void LoadNextLevel()
    {
        if (myPlayStatus == PlayStatus.InScaleDraw)
        {
            myLevelLoader.LoadNextLevel();
        }
    }

    public void onCameraTranslationFinish()
    {
        if (myPlayStatus == PlayStatus.InCameraTranslation)
        {
            myPlayStatus = PlayStatus.InDisplayEyeAndScale;
            myPlayerMovement.setEnableMovement(true);

        }
    }

    public void onCameraResetFinish()
    {

    }


    private void PlaySpriteAnimation(bool isLeftEye, GameObject coin)
    {
        string animName = "Sprite";
        if (isLeftEye)
        {
            LEyeAnimator.SetTrigger(animName);
            LEyeBallAnimator.SetTrigger(animName);
        }
        else
        {
            REyeAnimator.SetTrigger(animName);
            REyeBallAnimator.SetTrigger(animName);

        }
        coin.GetComponent<CoinAnimationController>().PlaySpriteAnim(isLeftEye);
    }

    private void PlaySwallowAnimation(bool isLeftEye, GameObject coin)
    {
        string animName = "Swallow";
        if (isLeftEye)
        {
            LEyeAnimator.SetTrigger(animName);
            LEyeBallAnimator.SetTrigger(animName);
        }
        else
        {
            REyeAnimator.SetTrigger(animName);
            REyeBallAnimator.SetTrigger(animName);
        }
        coin.GetComponent<CoinAnimationController>().PlaySwallowAnim(isLeftEye);

    }


    IEnumerator DelayForSeconds(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        myPlayStatus = PlayStatus.PlayerMovement;

    }

    // Update is called once per frame
    void Update()
    {
        if (myPlayStatus == PlayStatus.InScaleDraw)
        {
            myVFXController.PlayCubeVFX();
            bool isPlayingAnimation = cubeTransition.GetCurrentAnimatorStateInfo(0).IsName("AM_Cube_Finsh")
                && cubeTransition.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f;

            if (!isPlayingAnimation)
            {
                myPlayerMovement.OnUpdate();
                // is press key Enter, transition to apple scene
            }

        }
        else if (myPlayStatus == PlayStatus.Configuration)
        {
            StartCoroutine(DelayForSeconds(1));
        }

        else if (myPlayStatus == PlayStatus.PlayerMovement)
        {
            // check if draw
            int leftWeight = Scale_Left.GetComponent<Scale>().getTotalWeight();
            int rightWeight = Scale_Right.GetComponent<Scale>().getTotalWeight();

            if (leftWeight == 5 && rightWeight == 5)
            {
                cubeTransition.SetTrigger("IsNewtonPuzzleSolved");
                myPlayStatus = PlayStatus.InScaleDraw;

                return;
            }

            myPlayerMovement.OnUpdate();
            myCameraController.onUpdateCameraWithPlayerMovement(myPlayerMovement.getMovementDirection());

            float Dist_L = myDetectDistance.getDistanceBetweenPlayerAndLeftEye();
            float Dist_R = myDetectDistance.getDistanceBetweenPlayerAndRightEye();

            if (Dist_L < dist_threshold)
            {
                myCameraController.showLeftEye();
                myPlayerMovement.setEnableMovement(false);
                myPlayerMovement.TranslateToLeftEye();
                myPlayStatus = PlayStatus.InCameraTranslation;
                currentScale = Scale_Left;
                Scale_Left.GetComponent<ScaleHighlighter>().HighlightScale();
                Scale_Right.GetComponent<ScaleHighlighter>().disableHighlight();
                myVFXController.PlayDisplayEyeAndScaleVFX(currentScale.transform);

            }
            else if (Dist_R < dist_threshold)
            {
                myCameraController.showRightEye();
                myPlayerMovement.setEnableMovement(false);
                myPlayerMovement.TranslateToRightEye();
                myPlayStatus = PlayStatus.InCameraTranslation;
                currentScale = Scale_Right;
                Scale_Right.GetComponent<ScaleHighlighter>().HighlightScale();
                Scale_Left.GetComponent<ScaleHighlighter>().disableHighlight();
                myVFXController.PlayDisplayEyeAndScaleVFX(currentScale.transform);

            }
            else
            {
                currentScale = null;
                Scale_Right.GetComponent<ScaleHighlighter>().disableHighlight();
                Scale_Left.GetComponent<ScaleHighlighter>().disableHighlight();
                myVFXController.StopDisplayEyeAndScaleVFX();
            }

        }
        else if (myPlayStatus == PlayStatus.InCameraTranslation)
        {
            if (currentScale != null)
            {
                currentScale.GetComponent<ScaleHighlighter>().HighlightScale();
            }

        }
        else if (myPlayStatus == PlayStatus.InDisplayEyeAndScale)
        {
            currentScale.GetComponent<ScaleHighlighter>().HighlightScale();
            myPlayerMovement.OnUpdate();
            float Dist_L = myDetectDistance.getDistanceBetweenPlayerAndLeftEye();
            float Dist_R = myDetectDistance.getDistanceBetweenPlayerAndRightEye();
            if (Dist_L >= leave_dist_threshold && Dist_R >= leave_dist_threshold)
            {
                myCameraController.resetCam();
                myPlayerMovement.setEnableMovement(true);
                myPlayStatus = PlayStatus.PlayerMovement;
            }

            RaycastHit hit;
            Ray ray;
            ray = myCameraController.getCurrentCamera().ScreenPointToRay(Input.mousePosition);

            if (Input.GetMouseButtonUp(0) && Physics.Raycast(ray, out hit))
            {
                GameObject hitObject = hit.collider.gameObject;
                if (hitObject == coin1 || hitObject == coin2 || hitObject == coin3
                    || hitObject == coin4 || hitObject == coin5 || hitObject == currentScale)
                {
                    EquipCoin playerEquipCoin = player.GetComponent<EquipCoin>();
                    if (playerEquipCoin.getEquipped() != null) // if player has a coin
                    {
                        myPlayStatus = PlayStatus.InPutCoin;
                    }
                    else if (!currentScale.GetComponent<Scale>().isEmpty())
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

        }
        else if (myPlayStatus == PlayStatus.InPutCoin)
        {
            if (isPlayingSwallowAnimation)
            {
                Animator coinAnimator = activeCoin.GetComponent<Animator>();
                isPlayingSwallowAnimation = coinAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f;
                if (!isPlayingSwallowAnimation)
                {
                    currentScale.GetComponent<AddNewCoin>().AddCoin(activeCoin);
                    myVFXController.PlayScalePutVFX(currentScale.transform);
                    StartCoroutine(currentScale.GetComponent<Scale>().UpdatePosition());
                    myPlayStatus = PlayStatus.InDisplayEyeAndScale;
                    print(coin4.transform.position);
                    print(coin5.transform.position);
                }

            }
            else
            {
                currentScale.GetComponent<ScaleHighlighter>().HighlightScale();
                // if can put coin
                EquipCoin playerEquipCoin = player.GetComponent<EquipCoin>();
                GameObject coin = playerEquipCoin.getEquipped();
                if (coin != null)
                {
                    activeCoin = coin;
                    playerEquipCoin.Drop();    
                    PlaySwallowAnimation(currentScale == Scale_Left, coin);
                    isPlayingSwallowAnimation = true;
                }
            }


        }
        else if (myPlayStatus == PlayStatus.InTakeCoin)
        {
            if (isPlayingSpriteAnimation)
            {
                
                if (activeCoin != null)
                {
                    Animator coinAnimator = activeCoin.GetComponent<Animator>();
                    isPlayingSpriteAnimation = coinAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f;

                    if (!isPlayingSpriteAnimation)
                    {
                        EquipCoin playerEquipCoin = player.GetComponent<EquipCoin>();
                        playerEquipCoin.Equip(activeCoin);
                        activeCoin.GetComponent<MaterialChangeOutline>().SetEnableMaterialChange(false);
                        myPlayStatus = PlayStatus.InDisplayEyeAndScale;
                        myPlayerMovement.setEnableMovement(true);

                    }
                }
                
            }
            else
            {
                currentScale.GetComponent<ScaleHighlighter>().HighlightScale();
                List<GameObject> coins = currentScale.GetComponent<Scale>().getCoinsOnScales();
                foreach (GameObject coin in coins)
                {
                    coin.GetComponent<MaterialChangeOutline>().SetEnableMaterialChange(true);
                }

                myPlayerMovement.setEnableMovement(false);
                //EquipCoin playerEquipCoin = player.GetComponent<EquipCoin>();
                if (Input.GetMouseButtonUp(0))
                {
                    RaycastHit hit;
                    Ray ray;
                    ray = myCameraController.getCurrentCamera().ScreenPointToRay(Input.mousePosition);
                    if (Physics.Raycast(ray, out hit))
                    {
                        GameObject coinHit = hit.collider.gameObject;
                        if (coinHit == coin1 || coinHit == coin2 || coinHit == coin3
                            || coinHit == coin4 || coinHit == coin5)
                        {
                            currentScale.GetComponent<Scale>().popCoin(coinHit);
                                              
                            if (currentScale == Scale_Left)
                            {
                                myCameraController.showLeftEye();
                            }
                            else if (currentScale == Scale_Right)
                            {
                                myCameraController.showRightEye();
                            }
                            StartCoroutine(currentScale.GetComponent<Scale>().UpdatePosition());                        
                            foreach (GameObject coin in coins)
                            {
                                coin.GetComponent<MaterialChangeOutline>().SetEnableMaterialChange(false);
                            }
                            activeCoin = coinHit;
                            PlaySpriteAnimation(currentScale == Scale_Left, coinHit);
                            isPlayingSpriteAnimation = true;
                        }
                    }
                }
            }

            
        }
    }


}
