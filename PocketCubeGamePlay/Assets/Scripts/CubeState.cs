using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CubeState : MonoBehaviour
{

    public List<GameObject> front   = new List<GameObject>();
    public List<GameObject> back    = new List<GameObject>();
    public List<GameObject> up      = new List<GameObject>();
    public List<GameObject> down    = new List<GameObject>();
    public List<GameObject> left    = new List<GameObject>();
    public List<GameObject> right   = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public List<List<GameObject>>  GetAllCubeSides()
    {
        List<List<GameObject>> cubeSides = new List<List<GameObject>>()
        {up,down,
        left,right,
        front,back};

        return cubeSides;
    }
}
