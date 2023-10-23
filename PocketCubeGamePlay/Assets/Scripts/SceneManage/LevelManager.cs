using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance;

    [SerializeField] private GameObject _loaderCanvas;
    [SerializeField] private Image _progressBar;
    [SerializeField] private int _loadingTime;

    private PlayerData playerData;

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
    }

    void Start()
    {
        CreatePlayerData();
    }

    
    public async void LoadScene(string sceneName)
    {
        
        var scene = SceneManager.LoadSceneAsync(sceneName);
        scene.allowSceneActivation = false;

        _loaderCanvas.SetActive(true);

        do{
            await Task.Delay(_loadingTime);

            _progressBar.fillAmount = scene.progress;
        }while (scene.progress < 0.9f);

        scene.allowSceneActivation = true;
        _loaderCanvas.SetActive(false);

        if (playerData.level < 1 && sceneName == "NewtonLevel_GPP_Test")
        {
            playerData.level = 1;
            SaveData();
            //print("Reached Level" + playerData.level);
        }
        else if(playerData.level < 2 && sceneName == "ELE_GPP Temp")
        {
            playerData.level = 2;
            SaveData();
        }

        else if(playerData.level < 3 && sceneName == "Level_Fourier")
        {
            playerData.level = 3;
            SaveData();
            //print("Reached Level" + playerData.level);
        }
        print("Current level status :" + playerData.level);
    }


    //PlayerPrefs Load and Save
    private void CreatePlayerData()
    {
        playerData = new PlayerData(0);
    }
    public void SaveData()
    {
        PlayerPrefs.SetInt("Level", playerData.level);

    }

    public void LoadData()
    {
        playerData = new PlayerData(PlayerPrefs.GetInt("Level"));

        Debug.Log(playerData.level);
    }
}
