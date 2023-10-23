using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{


    // Start is called before the first frame update
    private void Awake()
    {
        //UIManager.Instance.OpenPanel(UIConst.MainMenuPanel);
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!UIManager.Instance.panelDict.ContainsKey(UIConst.SettingPanel) && !UIManager.Instance.panelDict.ContainsKey(UIConst.MainMenuPanel) && Input.GetKeyDown(KeyCode.Escape) )
        {
            //print("ESC");
            UIManager.Instance.OpenPanel(UIConst.SettingPanel);
        }

        
    }

    
}
