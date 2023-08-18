using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MainMenuPanel : BasePanel
{
    public TextMeshProUGUI Tittle;
    public Button StartBtn;
    public Button SettingBtn;
    public Button CollectionBtn;
    public Button CreditBtn;
    public Button QuitBtn;

    // Start is called before the first frame update
    void Awake()
    {
        StartBtn.onClick.AddListener(OnStartBtn);
        SettingBtn.onClick.AddListener(OnSettingBtn);
        CollectionBtn.onClick.AddListener(OnCollectionBtn);
        CreditBtn.onClick.AddListener(OnCreditBtn);
        QuitBtn.onClick.AddListener(OnQuitBtn);
    }

    private void Start()
    {
        string panelTittle = gameObject.name;
        Tittle.text = panelTittle;


        
    }

    // Update is called once per frame

    public void OnStartBtn()
    {
        
        print("OnStartBtn");
        UIManager.Instance.OpenPanel(UIConst.LevelMapPanel);
        ClosePanel();
    }

    public void OnSettingBtn()
    {

    }

    public void OnCollectionBtn()
    {

    }

    public void OnCreditBtn()
    {

    }

    public void OnQuitBtn()
    {

    }



    /*public override void ClosePanel()
    {
        base.ClosePanel();
        
    }*/
}
