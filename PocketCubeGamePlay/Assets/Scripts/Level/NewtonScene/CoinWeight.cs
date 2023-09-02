using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinWeight : MonoBehaviour
{
    [SerializeField]
    private int _weight;

    public int getWeight()
    {
        return _weight;
    }

    public void setWeight(int weight)
    {
        _weight = weight; 
    }
    
}
