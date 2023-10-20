using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using TMPro.Examples;

public class MainMenuPanel : BasePanel
{
    public TextMeshProUGUI Tittle;
    public Button StartBtn;
    public Button SettingBtn;
    //public Button CollectionBtn;
    public Button CreditBtn;
    public Button QuitBtn;
    //private StartSceneCameraController StartSceneCameraController;
    //private GameObject CameraController;

    // Start is called before the first frame update
    void Awake()
    {
        StartBtn.onClick.AddListener(OnStartBtn);
        SettingBtn.onClick.AddListener(OnSettingBtn);
        //CollectionBtn.onClick.AddListener(OnCollectionBtn);
        CreditBtn.onClick.AddListener(OnCreditBtn);
        QuitBtn.onClick.AddListener(OnQuitBtn);
        
    }
    private void awake()
    {
        if(PlayerPrefs.GetInt("Level") == 0)
        {
            StartBtn.GetComponent<TextMeshProUGUI>().text = "Start Game";
        }
        else if(PlayerPrefs.GetInt("Level") > 0)
        {
            StartBtn.GetComponent<TextMeshProUGUI>().text = "Continue";
        }
    }

    private void Start()
    {
        string panelTittle = gameObject.name;
        Tittle.text = panelTittle;
        //CameraController = GameObject.Find("CameraController");
        
    }

    // Update is called once per frame

    public void OnStartBtn()
    {
        

        //print("OnStartBtn");
        switch (PlayerPrefs.GetInt("Level"))
        {
            /*case 0:
                LevelManager.Instance.LoadScene();
                break;*/
            case 1:
                LevelManager.Instance.LoadScene("NewtonLevel_GPP_Test");
                break;
            case 2:
                break;
            case 3:
                LevelManager.Instance.LoadScene("Level_Fourier");
                break;
        }

        ClosePanel();
    }

    public void OnSettingBtn()
    {
        UIManager.Instance.OpenPanel(UIConst.SettingPanel);
    }

    /*public void OnCollectionBtn()
    {
        UIManager.Instance.OpenPanel(UIConst.CollectionPanel);
    }*/

    public void OnCreditBtn()
    {
        UIManager.Instance.OpenPanel(UIConst.CreditPanel);
    }

    public void OnQuitBtn()
    {
        UIManager.Instance.OpenPanel(UIConst.ExitPanel);
    }



    public override void ClosePanel()
    {
        base.ClosePanel();

    }
}
