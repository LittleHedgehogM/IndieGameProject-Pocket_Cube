using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircuitLevel_PlayController : MonoBehaviour
{

    CircuitLevel_PlayerMovement myPlayerMovement;

    // Start is called before the first frame update

    [SerializeField] private GameObject Light;

    void Start()
    {
        myPlayerMovement = FindObjectOfType<CircuitLevel_PlayerMovement>();
    }



    //// Update is called once per frame
    //void Update()
    //{


    //}
}
