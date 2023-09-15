using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubePieceOutlineController : MonoBehaviour
{

    public static void enableOutline(GameObject cubePiece)
    {
        Outline cubeOutLine = cubePiece.GetComponentInChildren<Outline>();
        if (cubeOutLine != null)
        {
            cubeOutLine.enabled = true;
        }
    }

    public static void disableOutline(GameObject cubePiece)
    {
        Outline cubeOutLine = cubePiece.GetComponentInChildren<Outline>();
        if (cubeOutLine != null)
        {
            cubeOutLine.enabled = false;
        }
    }

}
