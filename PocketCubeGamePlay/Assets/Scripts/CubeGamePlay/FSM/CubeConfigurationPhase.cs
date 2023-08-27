using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CubeConfigurationPhase : GameplayPhase
{

    CubeConfigure myCubeConfigure;
    ReadCube readCube;
    CubeState myCubeState;

    [SerializeField] [Range(0, 1)] 
    float startAnimationTime;
    [SerializeField] private AnimationCurve translationCurve;
    float t;


    enum ConfigurationState
    {
        Animation,
        Check
    }

    ConfigurationState currentState;


    public override void onStart()
    {
        myCubeConfigure = FindObjectOfType<CubeConfigure>();
        myCubeState     = FindObjectOfType<CubeState>();
        readCube        = FindObjectOfType<ReadCube>();

        ConfuigurePocketCube();
        currentState = ConfigurationState.Animation;
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

    }


    // return true if is the cube is loaded
    public override bool onUpdate()
    {

        if (currentState == ConfigurationState.Animation)
        {
            StartCoroutine(playStartAnimation());
            currentState = ConfigurationState.Check;
        }
        else if (currentState == ConfigurationState.Check)
        {
            if (  t >= 1)
            {
                readCube.ReadState();
                if (myCubeState.GetStateString() == myCubeConfigure.cubeStateString)
                {
                    return true;
                }
            }
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
    }


}