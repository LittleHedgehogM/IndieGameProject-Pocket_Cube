using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackGroundMusic : MonoBehaviour
{
    static BackGroundMusic S;

    void Awake()
    {
        if(S == null)
            S = this;
        else if(S != this)
            Destroy(gameObject);

    DontDestroyOnLoad(gameObject);
}
    
    
    
    

    
    
    
    
    
}
// 








