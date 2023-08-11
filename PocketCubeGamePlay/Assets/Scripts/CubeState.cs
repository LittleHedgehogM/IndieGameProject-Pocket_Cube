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


    string GetSideString(List<GameObject> side)
    {
        string sideString = "";
        foreach (GameObject cubeSide in side)
        {
            sideString += cubeSide.name[0];
        }
        return sideString;

    }
        
    public string GetStateString()
    {
        string stateString = "";
        stateString += GetSideString(up);
        stateString += GetSideString(right);
        stateString += GetSideString(front);
        stateString += GetSideString(down);
        stateString += GetSideString(left);
        stateString += GetSideString(back);
        return stateString;
    }

    bool isSideRestored(List<GameObject> side)
    {
        char firstChar = side[0].name[0];
        foreach (GameObject cubeSide in side)
        {
            if (cubeSide.name[0] != firstChar)
            {
                return false;
            }
        }
        return true;
    }


    public bool isCubeSolved()
    {
        // check 6 sides and see if their color are the same
        //GetSideString(up);
        //stateString == "UUUURRRRFFFFDDDDLLLLBBBB"
        return    isSideRestored(up) && isSideRestored(down) 
               && isSideRestored(left) && isSideRestored(right) 
               && isSideRestored(front) && isSideRestored(back);
    }

}
