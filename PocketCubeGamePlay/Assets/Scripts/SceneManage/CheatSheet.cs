using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheatSheet: MonoBehaviour
{
    private LevelManager levelManager;
    public void ChangeScene(string sceneName)
    {
        LevelManager.Instance.LoadScene(sceneName);
    }
    public void SetLevelPass()
    {
        PlayerPrefs.SetInt("Level", 4);
    }
    public void SetLevel(int level)
    {
        PlayerPrefs.SetInt("Level", level);
    }
    public void ResetAch()
    {
        List<bool> r = new List<bool>();
        for (int i = 1; i < 8; i++)
        {
            var cal = new Steamworks.Data.Achievement($"ACHIEVEMENT_0{i}");
            cal.Clear();
        }
    }
    public void CleanPrefData()
    {
        PlayerPrefs.DeleteAll();
        //levelManager.CreatePlayerData();
    }
}
