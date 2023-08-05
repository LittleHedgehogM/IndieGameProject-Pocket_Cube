using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestSoundPlay : MonoBehaviour
{
    public AK.Wwise.Event PlayEvent;
    public AK.Wwise.Event StopEvent;
    bool musicOn = false;

    void Start()
    {
        Debug.Log("Press Q to play some");
    }
    void Update()
    {
        
        if (Input.GetKeyDown(KeyCode.Q))
        {
            if (!musicOn)
            {
                PlayEvent.Post(gameObject);
                musicOn = true;
                Debug.Log("Press Q again to stop playing");
            }
            else if (musicOn)
            {
                StopEvent.Post(gameObject);
                musicOn = false;
                Debug.Log("Music stops");
            }
            
        }

    }
}
