using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIAnimationScript : MonoBehaviour
{
    // Start is called before the first frame update
    private void MainMenuClickSound()
    {
        AkSoundEngine.PostEvent("Play_Main_Click01", gameObject);
    }
    private void MainMenuHoverSound()
    {
        AkSoundEngine.PostEvent("Play_Main_Hover", gameObject);
    }
}
