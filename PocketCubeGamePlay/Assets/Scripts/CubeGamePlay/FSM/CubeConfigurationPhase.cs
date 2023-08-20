using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CubeConfigurationPhase : GameplayPhase
{

    CubeConfigure myCubeConfigure;
    ReadCube readCube;
    CubeState myCubeState;

    public override void onStart()
    {
        myCubeConfigure = FindObjectOfType<CubeConfigure>();
        myCubeState     = FindObjectOfType<CubeState>();
        readCube        = FindObjectOfType<ReadCube>();

        ConfuigurePocketCube();
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

    // return true if is the cube is loaded
    public override bool onUpdate()
    {

        //ConfuigurePocketCube();

        readCube.ReadState();

        if (myCubeState.GetStateString() == myCubeConfigure.cubeStateString)
        {
            return true;

        }
        else
        {
            // play as default settings
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

    }

}