using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubePieceOutlineController : MonoBehaviour
{

    public static void enableOutline(GameObject cubePiece)
    {
        CubeOutline cubeOutLine = cubePiece.GetComponentInChildren<CubeOutline>();
        if (cubeOutLine != null)
        {
            cubeOutLine.DrawOutline();  
        }
    }

    public static void disableOutline(GameObject cubePiece)
    {
        CubeOutline cubeOutLine = cubePiece.GetComponentInChildren<CubeOutline>();
        if (cubeOutLine != null)
        {
           cubeOutLine.EraseOutline();
        }
    }

}
