using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using TMPro.Examples;
using UnityEngine.Video;

public class MainMenuPanel : BasePanel
{
    
    [SerializeField]private Button StartBtn;
    [SerializeField]private Button ContinueBtn;
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
        ContinueBtn.onClick.AddListener(OnContinueBtn);
        SettingBtn.onClick.AddListener(OnSettingBtn);
        //CollectionBtn.onClick.AddListener(OnCollectionBtn);
        //CreditBtn.onClick.AddListener(OnCreditBtn);
        QuitBtn.onClick.AddListener(OnQuitBtn);



        if (PlayerPrefs.GetInt("Level") == 0)
        {
            print("start");
            StartBtn.gameObject.SetActive(true);
            ContinueBtn.gameObject.SetActive(false);
        }
        else if (PlayerPrefs.GetInt("Level") > 0)
        {
            print("Continue");
            StartBtn.gameObject.SetActive(false);
            ContinueBtn.gameObject.SetActive(true);
        }
    }
    

    private void Start()
    {
        string panelTittle = gameObject.name;
        
        //CameraController = GameObject.Find("CameraController");
        

    }

    // Update is called once per frame

    public void OnStartBtn()
    {
        

        //print("OnStartBtn");
        switch (PlayerPrefs.GetInt("Level"))
        {
            case 0:
                
                LevelManager.Instance.LoadScene("First GPP");
                break;
            case 1:
                LevelManager.Instance.LoadScene("NewtonLevel_GPP_Test");
                break;
            case 2:
                LevelManager.Instance.LoadScene("ELE_GPP Temp");
                break;
            case 3:
                LevelManager.Instance.LoadScene("Level_Fourier");
                break;
        }

        ClosePanel();
    }
    public void OnContinueBtn()
    {
        switch (PlayerPrefs.GetInt("Level"))
        {
            case 0:
                LevelManager.Instance.LoadScene("First GPP");
                break;
            case 1:
                LevelManager.Instance.LoadScene("NewtonLevel_GPP_Test");
                break;
            case 2:
                LevelManager.Instance.LoadScene("ELE_GPP Temp");
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
