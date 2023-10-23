using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SettingsPanel : BasePanel
{
    public TextMeshProUGUI Tittle;
    public Button ReturnBtn;

    // Start is called before the first frame update
    void Start()
    {
        string panelTittle = gameObject.name;
        Tittle.text = panelTittle;
        ReturnBtn.onClick.AddListener(OnClickReturn);
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
        LevelManager.Instance.LoadScene("StartGame");
        ClosePanel();
    }
}
