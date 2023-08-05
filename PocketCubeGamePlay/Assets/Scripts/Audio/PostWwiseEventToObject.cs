using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PostWwiseEventToObject : MonoBehaviour
{
    public AK.Wwise.Event MyEvent;
    public GameObject PostTo;
    
    public void PlayFootstepSound()
    {

        MyEvent.Post(PostTo);
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
