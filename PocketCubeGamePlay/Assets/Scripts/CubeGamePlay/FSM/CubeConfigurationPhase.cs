using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CubeConfigurationPhase : GameplayPhase
{

    CubeConfigure myCubeConfigure;
    ReadCube readCube;
    CubeState myCubeState;
    //CubeVFXManager myCubeVFXManager;
    CubePlayUIController myUIController;
    CubeCursorController myCursorController;

    [Header("Animation")]
    [SerializeField] [Range(0, 1)] 
    float startAnimationTime;
    [SerializeField] private AnimationCurve translationCurve;
    float t;
    bool animationFinished = true;

    [Header("Tutorial Panel Settings")]
    [SerializeField] [Range(0, 10)] private int minTutorialDisplayTime;

    enum ConfigurationState
    {
        Animation,
        Check, 
        Tutorial,
        TutorialFinish
    }

    ConfigurationState currentState;

    bool canClickTutorial;
    float tutorialDisplayTime = 0;

    private void OnEnable()
    {
        CubePlayUIController.TutorialPanelHide += FinishConfigurationPhase;
    }

    private void OnDisable()
    {
        CubePlayUIController.TutorialPanelHide -= FinishConfigurationPhase;

    }

    private void FinishConfigurationPhase()
    {
        if (currentState == ConfigurationState.Tutorial)
        {
            currentState = ConfigurationState.TutorialFinish;
        }
    }

    public override void onStart()
    {
        myCubeConfigure = FindObjectOfType<CubeConfigure>();
        myCubeState     = FindObjectOfType<CubeState>();
        readCube        = FindObjectOfType<ReadCube>();
        myUIController  = FindObjectOfType<CubePlayUIController>();
        myCursorController = FindObjectOfType<CubeCursorController>();
        //myCubeVFXManager = FindObjectOfType<CubeVFXManager>();
        ConfuigurePocketCube();
        currentState = ConfigurationState.Check;
        animationFinished = true;
        myUIController.InitCubePlayUIElements();
        canClickTutorial = false;

    }

    private void ConfigureOneCubePiece(CubeConfigure.PieceTransform pieceTransform)
    {
        GameObject cubePiece = pieceTransform.cubePiece;
        Vector3 position     = pieceTransform.position;
        Quaternion rotation  = pieceTransform.rotation;

        cubePiece.transform.position = position;
        cubePiece.transform.rotation = rotation;
    }


    private void ConfuigurePocketCube()
    {
        ConfigureOneCubePiece(myCubeConfigure.FrontLeftUp);
        ConfigureOneCubePiece(myCubeConfigure.FrontLeftDown);
        ConfigureOneCubePiece(myCubeConfigure.FrontRightUp);
        ConfigureOneCubePiece(myCubeConfigure.FrontRightDown);
        ConfigureOneCubePiece(myCubeConfigure.BackLeftUp);
        ConfigureOneCubePiece(myCubeConfigure.BackLeftDown);
        ConfigureOneCubePiece(myCubeConfigure.BackRightUp);
        ConfigureOneCubePiece(myCubeConfigure.BackRightDown);
    }


    public IEnumerator playStartAnimation()
    {
        GameObject pocketCube = CubePlayManager.instance.pocketCube;

        float currentUsedTime = 0;
        float currentRotationDegree = 0;
        t = 0;

        Vector3 startScale = Vector3.zero;
        Vector3 endScale = pocketCube.transform.localScale;
        while (t < 1)
        {
            currentUsedTime += Time.deltaTime;
            t = currentUsedTime / startAnimationTime;

            float angle = Mathf.Lerp(0, 360, translationCurve.Evaluate(t));
            float deltaAngle = angle - currentRotationDegree;
            currentRotationDegree = angle;
            pocketCube.transform.RotateAround(pocketCube.transform.position, Vector3.up, deltaAngle);
            pocketCube.transform.localScale = Vector3.Lerp(startScale, endScale, translationCurve.Evaluate(t));

            yield return null;

        }
        animationFinished = true;
    }


    // return true if is the cube is loaded
    public override bool onUpdate()
    {

        if (currentState == ConfigurationState.Animation)
        {
            StartCoroutine(playStartAnimation());
            //Audio
            AkSoundEngine.PostEvent("Play_cube_ani", gameObject);
            //
            currentState = ConfigurationState.Check;
            myCursorController.setNormalCursor();

        }
        else if (currentState == ConfigurationState.Check)
        {
            if (animationFinished)
            {
                readCube.ReadState();
                if (myCubeState.GetStateString() == myCubeConfigure.cubeStateString)
                {
                    //return true;
                    if (myUIController.getShowTutorialPanel())
                    {
                        currentState = ConfigurationState.Tutorial;
                        myUIController.TutorialStarts();

                    }
                    else 
                    {
                        return true;
                    }
                }
            }
        }
        else if (currentState == ConfigurationState.Tutorial)
        {
            if (Input.GetMouseButton(0) && !Utils.isMouseOverUI())
            {
                myCursorController.setSwipeCursor();                
            }

        }
        else if (currentState == ConfigurationState.TutorialFinish) 
        {
            return true;
        }
        
        return false;
    }

    

    public override void onEnd()
    {
        print("Configuration End, Start to Play Game");
    }

    public override void onRestart()
    {
        base.onRestart();
        ConfuigurePocketCube();
        readCube.ReadState();
        t = 0;
        currentState = ConfigurationState.Animation;
        animationFinished = false;
        myUIController.InitCubePlayUIElements();

    }


}