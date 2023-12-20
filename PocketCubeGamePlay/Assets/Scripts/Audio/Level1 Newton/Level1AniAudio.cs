using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level1AniAudio : MonoBehaviour
{
    void Level1_Cube_drop()
    {
        AkSoundEngine.PostEvent("Play_Cube_drop", gameObject);
    }

    void Level1_Coin_Put_in()
    {
        AkSoundEngine.PostEvent("Play_Put_on", gameObject);
    }
}
