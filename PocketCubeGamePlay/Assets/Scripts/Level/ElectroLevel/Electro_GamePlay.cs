using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Electro_GamePlay : MonoBehaviour
{
    Electro_Camera_Controller myCameraController;
    Electro_PlayerMovement myPlayerMovement;

    [SerializeField] private Electro_Puzzle starPuzzle;
    [SerializeField] private Electro_SunPuzzle sunPuzzle;
    [SerializeField] private Electro_MoonPuzzle moonPuzzle;

    void Start()
    {
        myCameraController  = FindObjectOfType<Electro_Camera_Controller>();
        myPlayerMovement    = FindObjectOfType<Electro_PlayerMovement>();
        starPuzzle.Init();
        sunPuzzle.Init();
        moonPuzzle.Init();
    } 

    void Update()
    {
        myPlayerMovement.OnUpdate();
        starPuzzle.UpdatePuzzle();
        sunPuzzle.UpdatePuzzle();
        moonPuzzle.UpdatePuzzle();
        
    }


}
