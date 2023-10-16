using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FrontPage : MonoBehaviour
{
    public Button pressAnyKey;
    // Start is called before the first frame update

    void Awake()
    {
        pressAnyKey.onClick.AddListener(OnPressAnyKey);
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnPressAnyKey()
    {
        
        UIManager.Instance.OpenPanel(UIConst.MainMenuPanel);
    }

}
