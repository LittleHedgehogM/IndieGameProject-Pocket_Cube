using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SaveDataCorrector : MonoBehaviour
{
    private string sceneName;
    private void Awake()
    {

        sceneName = SceneManager.GetActiveScene().name;
        if (sceneName == "First GPP")
        {
            PlayerPrefs.SetInt("Level", 0);
        }
        else if (sceneName == "NewtonLevel_GPP_Test")
        {

            PlayerPrefs.SetInt("Level", 1);
        }
        else if (sceneName == "ELE_GPP Temp")
        {
            PlayerPrefs.SetInt("Level", 2);
        }

        else if (sceneName == "Level_Fourier")
        {
            PlayerPrefs.SetInt("Level", 3);
        }
    }
}
