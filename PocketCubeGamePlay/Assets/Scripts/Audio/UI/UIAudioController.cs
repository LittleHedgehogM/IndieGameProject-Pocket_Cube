using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIAudioController : MonoBehaviour
{
    public void Play_UI_click()
    {
        AkSoundEngine.PostEvent("Play_UI_click", gameObject);
    }

    

}
