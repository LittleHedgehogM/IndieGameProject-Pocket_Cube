using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level0AnimationAudio : MonoBehaviour
{
    void Level0CubeShow()
    {
        AkSoundEngine.PostEvent("Play_reduce", gameObject);
    }
}
