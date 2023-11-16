using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutotialWindowPanel : BasePanel
{
    [SerializeField] private Button closeBtn;
    [SerializeField] private Image tutorialIMG;

    private void Start()
    {
        closeBtn.onClick.AddListener(CloseBtn);
        if (PlayerPrefs.GetInt("Tutorials") == 0)
        {
            tutorialIMG.sprite = Resources.Load<Sprite>("Tutorial_0");
            PlayerPrefs.SetInt("Tutorials", 0);
        }
        else if (PlayerPrefs.GetInt("Tutorials") == 1)
        {
            tutorialIMG.sprite = Resources.Load<Sprite>("Tutorial_1");
            PlayerPrefs.SetInt("Tutorials", 1);
        }
        else if (PlayerPrefs.GetInt("Tutorials") == 2)
        {
            tutorialIMG.sprite = Resources.Load<Sprite>("Tutorial_1");
            PlayerPrefs.SetInt("Tutorials", 2);
        }

    }

    private void CloseBtn()
    {
        ClosePanel();
    }
}
