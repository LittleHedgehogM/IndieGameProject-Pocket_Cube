using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class SettingsPanel : BasePanel
{
    //public TextMeshProUGUI Tittle;
    //Disk Btn
    [Header("Main Tab")]
    Vector3 targetAngle;
    [SerializeField] private GameObject disk;
    [SerializeField] private Button OptionBtn;
    [SerializeField] private Button ExitBtn;
    [SerializeField] private Button TutorialBtn;
    private bool diskSwitch = true;
    

    [Header("Option Tab")]
    [SerializeField] private GameObject optionTab;
    [SerializeField] private Button ResetDataBtn;
    [SerializeField] private Button ReturnBtn;

    [Header("Exit Tab")]
    [SerializeField] private GameObject exitTab;
    [SerializeField] private Button ExitYes;
    [SerializeField] private Button ExitNo;

    [Header("Tutorial Tab")]
    [SerializeField] private GameObject tutorialTab;
    [SerializeField] private Button MoveLeft;
    [SerializeField] private Button MoveRight;
    [SerializeField] private RectTransform tutorialIMG;
    private bool tutorialSwitch = true;
    private float tutorialWidth = 1160f;
    //[SerializeField] private float speed;
    //

    // Start is called before the first frame update
    private void Awake()
    {
        optionTab.SetActive(true);
        tutorialTab.SetActive(false);
        exitTab.SetActive(false);
    }
    void Start()
    {
        //Main
        OptionBtn.onClick.AddListener(OnClickOption);
        ExitBtn.onClick.AddListener(OnClickExit);
        TutorialBtn.onClick.AddListener(OnClickTutorial);

        //Tutorial
        MoveLeft.onClick.AddListener(OnClickMoveLeft);
        MoveRight.onClick.AddListener(OnClickMoveRight);

        //Settings
        ReturnBtn.onClick.AddListener(OnClickReturn); 
        ResetDataBtn.onClick.AddListener(OnClickResetData);

        //Exit
        ExitYes.onClick.AddListener(OnExitYes);
        ExitNo.onClick.AddListener(OnExitNo);

        if(SceneManager.GetActiveScene().name == "StartGame")
        {
            ReturnBtn.gameObject.SetActive(false);
        }

        if (tutorialIMG.anchoredPosition.x > 1180)
        {
            MoveLeft.gameObject.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ClosePanel();
        }
    }

#region DiskTabs
    //option:0 ;Exit:120 ;tutorial:240;
    void OnClickOption()
    {
        if (diskSwitch)
        {
            targetAngle = new Vector3(0, 0, 0);
            StartCoroutine(DiskSwitch(targetAngle, 0));
        }
        //print("Option");

    }

    void OnClickTutorial()
    {
        if (diskSwitch)
        {
            targetAngle = new Vector3(0, 0, 240);
            StartCoroutine(DiskSwitch(targetAngle, 1));
        }
        //print("Tutorial");
        
    }

    
    void OnClickExit()
    {
        if (diskSwitch) 
        {
            targetAngle = new Vector3(0, 0, 120);
            StartCoroutine(DiskSwitch(targetAngle, 2));
        }
        //print("Exit");
        

        //Application.Quit();
    }

    //Tutorial
    void OnClickMoveLeft()
    {
        if (tutorialSwitch)
        {
            tutorialSwitch = false;
            StartCoroutine(TutorialMove(new Vector2(tutorialWidth, 0)));
        }
        
    }

    void OnClickMoveRight()
    {
        if (tutorialSwitch)
        {
            tutorialSwitch = false;
            StartCoroutine(TutorialMove(new Vector2(-tutorialWidth, 0)));
        }
        
    }

    IEnumerator DiskSwitch(Vector3 targetAngle, int tab)
    {
        optionTab.SetActive(false);
        tutorialTab.SetActive(false);
        exitTab.SetActive(false);
        diskSwitch = false;
        var target = Quaternion.Euler(targetAngle);
        float t = 0f;


        while (t < 1f)
        {
            t += Time.deltaTime;
            disk.transform.rotation = Quaternion.Slerp(disk.transform.rotation, target, t);
            switch (tab)
            {
                case 0:
                    optionTab.SetActive(true);
                    break;

                case 1:
                    tutorialTab.SetActive(true);
                    break;

                case 2:
                    exitTab.SetActive(true);
                    break;
            }
            yield return null;
        }
        diskSwitch = true;
        yield break;
    }
    #endregion


    //Settings
    void OnClickReturn()
    {
        ClosePanel();
        LevelManager.Instance.LoadScene("StartGame");
        
    }

    void OnClickResetData()
    {
        PlayerPrefs.SetInt("Level", 0);
        print(PlayerPrefs.GetInt("Level"));
        ClosePanel();
        if (SceneManager.GetActiveScene().name == "StartGame")
        {
            UIManager.Instance.ClosePanel(UIConst.MainMenuPanel);
        }
        SceneManager.LoadScene("StartGame");
    }

    
    //Tutorials
    IEnumerator TutorialMove(Vector2 moveLength)
    {
        print(tutorialIMG.anchoredPosition);
        Vector2 target = tutorialIMG.anchoredPosition + moveLength;  
        print(target);
        float t = 0f;
        print(PlayerPrefs.GetInt("Level"));

        while ( t < 1f )
        {
            t += Time.deltaTime;
            //float t = 100f;
            //float speed = 1000f;
            tutorialIMG.anchoredPosition = Vector2.Lerp(tutorialIMG.anchoredPosition, target, t);
            //Max dis                    
            //left
            if (tutorialIMG.anchoredPosition.x <= 1248)
            {
                MoveLeft.gameObject.SetActive(true);
            }
            else if (tutorialIMG.anchoredPosition.x > 1248)
            {
                MoveLeft.gameObject.SetActive(false);
            }
            //right
            if (PlayerPrefs.GetInt("Level") <= 1)
            {
                if (tutorialIMG.anchoredPosition.x < 2000)
                {
                    MoveRight.gameObject.SetActive(false);
                }
                else if (tutorialIMG.anchoredPosition.x >= 2000)
                {
                    MoveRight.gameObject.SetActive(true);
                }
            }
            else
            {
                if (tutorialIMG.anchoredPosition.x < -1248)
                {
                    MoveRight.gameObject.SetActive(false);
                }
                else if (tutorialIMG.anchoredPosition.x >= -1248)
                {
                    MoveRight.gameObject.SetActive(true);
                }
            }
            
     
            yield return null;          
        }

        tutorialSwitch = true;
        //print("moveable = true");
        yield break;

        

    }

    void OnExitYes()
    {
        Application.Quit();
    }

    void OnExitNo()
    {
        ClosePanel();
    }
}

