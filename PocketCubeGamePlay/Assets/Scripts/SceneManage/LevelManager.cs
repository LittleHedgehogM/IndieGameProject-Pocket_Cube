using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance;

    //notice
    public static Action SceneChangeInprogress;

    //[SerializeField] private GameObject _loaderCanvas;
    //[SerializeField] private Image _progressBar;
    [SerializeField] private int _loadingTime;

    //[SerializeField] private Image transitionImg;
    //[SerializeField] private float transitionSpeed;
    //private float alpha;
    //[SerializeField] 
    [SerializeField]private Animator _animatorTransition;
    

    [SerializeField] private Image titleIMG;
    [SerializeField] private Button titleBtn;
    private bool titleBtnClicked = false;
    public float titleBtnCDtime = 2f;

    private PlayerData playerData;
    [SerializeField] Button settingBtn;

    private string   latestVersion = "1.13.20240227";
    //private string previousVersion;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        if (SceneManager.GetActiveScene().name == "StartGame")
        {
            print("disable btn");
            settingBtn.gameObject.SetActive(false);
        }

        //_animatorTransition = GameObject.Find("CrossFade").GetComponent<Animator>();
        settingBtn.onClick.AddListener(OnClickSetting);
        titleBtn.onClick.AddListener(OnClickTitleBtn);
        titleBtn.gameObject.SetActive(false);


        //local save data scan
        if (PlayerPrefs.HasKey("Level") && !PlayerPrefs.HasKey("Version")) // 有存档 有版本号前的存档  =》赋予最新版本号
        {
            PlayerPrefs.SetString("PreVersion", "ancient"); //save pre-version
            PlayerPrefs.SetString("Version", latestVersion);
            Debug.Log("Create Version data for ancient players,and load data");
            LoadData();
            return;
        }
        else if (PlayerPrefs.HasKey("Version") && !(PlayerPrefs.GetString("Version") == latestVersion)) // 有存档 上个版本存档 =》 更新版本号
        {
            Debug.Log("Precious version savedata exist. save pre-version, update current-version and load data.");
            PlayerPrefs.SetString("PreVersion", PlayerPrefs.GetString("Version")); //save pre-version
            PlayerPrefs.SetString("Version", latestVersion); //update version
            LoadData();
            return;
        }
        else if (PlayerPrefs.GetString("Version") == latestVersion) //有存档 当前版本存档 =》 读取存档
        {         
                Debug.Log("Latest version savedata exist. Load data.");
                LoadData();
                return;          
        }
        else if (!PlayerPrefs.HasKey("Level") && !PlayerPrefs.HasKey("Version"))//新用户 创建数据
        {
            CreatePlayerData(); 
            //Debug.Log("Create Save data for new players");
        }
    }

    void Start()
    {
        
        
        //print(_animatorTransition);
    }

    private void OnEnable()
    {
        EndingVideo.EndingVideoStart += EndingSettignBtnCtl;
        VideoScript.VideoStart += VideoStart;
        VideoScript.VideoFinished += VideoEnded;
    }

    private void OnDisable()
    {
        EndingVideo.EndingVideoStart -= EndingSettignBtnCtl;
        VideoScript.VideoFinished -= VideoEnded;
        VideoScript.VideoStart -= VideoStart;
    }

    public async void LoadScene(string sceneName)
    {
        UIManager.Instance.ClosePanel(UIConst.SettingPanel);

        settingBtn.gameObject.SetActive(false);
        SceneChangeInprogress?.Invoke();
        titleIMG.sprite = Resources.Load<Sprite>(sceneName);
        _animatorTransition.Play("Crossfade_Start");
        await Task.Delay(_loadingTime);
        titleBtn.gameObject.SetActive(true);
        Debug.Log("_loadingTime finished");
        var scene = SceneManager.LoadSceneAsync(sceneName);
        scene.allowSceneActivation = false;
       
        //_loaderCanvas.SetActive(true);
        
        do
        {
            await Task.Delay(1000);
            //Debug.Log("wait for click");
            //_progressBar.fillAmount = scene.progress;

        }while (!titleBtnClicked);

        titleBtn.gameObject.SetActive(false);
        scene.allowSceneActivation = true;
        _animatorTransition.SetTrigger("End");

 


        //print("active btn");
        
        //print(SceneManager.GetActiveScene().name);
        //print(sceneName);
        if (sceneName == "StartGame")
        {
            print("disable btn");
            settingBtn.gameObject.SetActive(false);
        }

        Debug.Log("reset titleBtn state");
        settingBtn.gameObject.SetActive(true);
    }


    //PlayerPrefs Load and Save
    public void CreatePlayerData()
    {
        playerData = new PlayerData(0,0, latestVersion, "None"); //Creata data with Latest version 
        Debug.Log("New Player. Creating data.");
        SaveData();      
    }
    public void SaveData()
    {
        PlayerPrefs.SetInt("Level", playerData.level);
        PlayerPrefs.SetInt("Tutorials", playerData.tutorials);
        PlayerPrefs.SetString("Version", playerData.version);
        PlayerPrefs.SetString("PreVersion", playerData.preVersion);
        Debug.Log($"Saving Data\r\nLevel: {playerData.level}\r\nTutorials: {playerData.tutorials}\r\nVersion: {playerData.version}\r\nPreVersion: {playerData.preVersion}");
    }

    public void LoadData()
    {
        playerData = new PlayerData(PlayerPrefs.GetInt("Level"), PlayerPrefs.GetInt("Tutorials"), PlayerPrefs.GetString("Version"), PlayerPrefs.GetString("PreVersion"));
        Debug.Log($"Loading Data\r\nLevel: {playerData.level}\r\nTutorials: {playerData.tutorials}\r\nVersion: {playerData.version}\r\nPreVersion: {playerData.preVersion}");

    }


    //协程
    private void OnClickSetting()
    {
        UIManager.Instance.OpenPanel(UIConst.SettingPanel);
    }

    private void OnClickTitleBtn()
    {
        if (titleBtnClicked)
        {
            Debug.Log("clicked return");
            return;
        }
        else
        {
            Debug.Log("First clicked");
            StartCoroutine(ButtonCooldown());
            
        }
        

    }
    //Button cool down

    private IEnumerator ButtonCooldown()
    {
        titleBtnClicked = true;
        titleBtn.interactable = false;
        //titleBtn.gameObject.SetActive(false);
        // 执行按钮点击后的逻辑
        // ...

        yield return new WaitForSeconds(titleBtnCDtime);

        titleBtnClicked = false;
        titleBtn.interactable = true;
        //titleBtnisCooldown = false;
    }

    private void EndingSettignBtnCtl()
    {
        settingBtn.gameObject.SetActive(false);
        StartCoroutine(EndingVideoCountDown());
    }

    private IEnumerator EndingVideoCountDown()
    {
        yield return new WaitForSeconds(40);
        settingBtn.gameObject.SetActive(true);
        yield return null;
    }
    private void VideoStart()
    {
        settingBtn.gameObject.SetActive(false);
    }
    private void VideoEnded()
    {
        settingBtn.gameObject.SetActive(true);
    }
}
