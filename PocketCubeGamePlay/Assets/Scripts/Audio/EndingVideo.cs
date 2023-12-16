using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class EndingVideo : MonoBehaviour
{
    [SerializeField] VideoPlayer vp;

    private void Awake()
    {
        vp.Play();
        vp.loopPointReached += ActionAfterVideoPlayed;
    }
    void Start()
    {
        
        
    }

    

    private void ActionAfterVideoPlayed(VideoPlayer vp)
    {
        SceneManager.LoadScene("StartGame");
    }
}
