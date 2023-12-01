using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level1AniAudio : MonoBehaviour
{
    void Level1_Cube_drop()
    {
        AkSoundEngine.PostEvent("Play_Cube_drop", gameObject);
    }
}
