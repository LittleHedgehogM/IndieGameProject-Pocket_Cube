using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LevelMapPanel : BasePanel
{
    public TextMeshProUGUI Tittle;
    public Button LastLevelBtn;
    public Button NextLevelBtn;
    public Button EnterLevelBtn;
    

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
