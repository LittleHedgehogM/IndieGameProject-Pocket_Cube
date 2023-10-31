using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    // Start is called before the first frame update
     void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    

    // Update is called once per frame
    void Update()
    {
        if (!UIManager.Instance.panelDict.ContainsKey(UIConst.SettingPanel) && !(SceneManager.GetActiveScene().name == "StartGame") && Input.GetKeyDown(KeyCode.Escape) )
        {
            print("ESC");
            UIManager.Instance.OpenPanel(UIConst.SettingPanel);
        }
    }

    
}
