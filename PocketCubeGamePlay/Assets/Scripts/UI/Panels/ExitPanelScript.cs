using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ExitPanel : BasePanel
{
    public TextMeshProUGUI Tittle;
    public Button QuitYesBtn;
    public Button QuitNoBtn;
    

    // Start is called before the first frame update
    void Start()
    {
        string panelTittle = gameObject.name;
        Tittle.text = panelTittle;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
