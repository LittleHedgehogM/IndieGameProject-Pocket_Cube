using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonFuncs : MonoBehaviour
{
    public void ExitGame()
    {
        Application.Quit();
    }

    public void ResetSavedataAndReboot()
    {
        PlayerPrefs.SetInt("Level", 0);
        print(PlayerPrefs.GetInt("Level"));
        
        SceneManager.LoadScene("StartGame");
    }
}
