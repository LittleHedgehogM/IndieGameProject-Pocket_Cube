using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetPlayerData : MonoBehaviour
{
    public void ResetSavedata(int setLevel)
    {
        PlayerPrefs.SetInt("Level", setLevel);
        print(PlayerPrefs.GetInt("Level"));
    }
}
