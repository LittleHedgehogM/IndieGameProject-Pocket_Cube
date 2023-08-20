using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeConfigure : MonoBehaviour
{
    // test string 
    //  UFUFRRRRFDFDDBDBLLLLUBUB

    //[SerializeField] bool useSteps;

    //public enum Action // your custom enumeration
    //{
    //    Swipe,
    //    Diagonal,
    //    Commutation
    //};

    //[System.Serializable]
    //public struct CubeConfigureSteps
    //{
    //    public Action anAction;
    //    public List<GameObject> cubeList;
    //    public Vector3 Axis;
    //    public bool isClockwise;
    //}

    //public List<CubeConfigureSteps> steps;

    [SerializeField]
    public string cubeStateString;


    [System.Serializable]
    public struct PieceTransform
    {
        public GameObject cubePiece;
        public Vector3 position;
        public Quaternion rotation;
    }

    public PieceTransform FrontLeftUp;
    public PieceTransform FrontLeftDown;
    public PieceTransform FrontRightUp;
    public PieceTransform FrontRightDown;
    public PieceTransform BackLeftUp;
    public PieceTransform BackLeftDown;
    public PieceTransform BackRightUp;
    public PieceTransform BackRightDown;



}
