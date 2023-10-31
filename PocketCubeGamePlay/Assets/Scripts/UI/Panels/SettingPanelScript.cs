using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class SettingsPanel : BasePanel
{
    public TextMeshProUGUI Tittle;
    [SerializeField]private Button ReturnBtn;
    [SerializeField] private Button ExitBtn;
    [SerializeField] private Button ResetDataBtn;

    // Start is called before the first frame update
    void Start()
    {
        string panelTittle = gameObject.name;
        Tittle.text = panelTittle;
        ReturnBtn.onClick.AddListener(OnClickReturn);
        ExitBtn.onClick.AddListener(OnClickExit);
        ResetDataBtn.onClick.AddListener(OnClickResetData);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ClosePanel();
        }
    }

    void OnClickReturn()
    {
        ClosePanel();
        LevelManager.Instance.LoadScene("StartGame");
        
    }

    void OnClickExit()
    {
        Application.Quit();
    }

    void OnClickResetData()
    {
        PlayerPrefs.SetInt("Level", 0);
        print(PlayerPrefs.GetInt("Level"));
        ClosePanel();
        SceneManager.LoadScene("StartGame");
    }
}
