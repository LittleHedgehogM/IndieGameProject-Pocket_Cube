using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialWindowPanel : BasePanel
{
    [SerializeField] private Button closeBtn;
    //[SerializeField] private Image tutorialIMG;
    [SerializeField] private RectTransform ImgRect;
    /*[SerializeField] private Button MoveLeft;
    [SerializeField] private Button MoveRight;
*/
    private void Awake()
    {
        closeBtn.onClick.AddListener(CloseBtn);
    }

    private void Start()
    {
        print("Init Tutorial Data:" + PlayerPrefs.GetInt("Tutorials"));
        switch (PlayerPrefs.GetInt("Tutorials"))
        {
            case 0:
                ImgRect.anchoredPosition = new Vector2(0,0);
                PlayerPrefs.SetInt("Tutorials", 1);
                break;

            case 1:
                ImgRect.anchoredPosition = new Vector2(-935, 0);
                PlayerPrefs.SetInt("Tutorials", 2);
                break;

            case 2:
                ImgRect.anchoredPosition = new Vector2(-1870, 0);
                PlayerPrefs.SetInt("Tutorials", 3);
                break;
                
            case 3:
                ImgRect.anchoredPosition = new Vector2(-2805, 0);
                PlayerPrefs.SetInt("Tutorials", 4);
                break;

            case 4:
                ImgRect.anchoredPosition = new Vector2(-3740, 0);
                PlayerPrefs.SetInt("Tutorials", 5);
                break;

        }

        print("Set Tutorial Data:" + PlayerPrefs.GetInt("Tutorials"));
    }

    private void CloseBtn()
    {
        ClosePanel();
    }
}
