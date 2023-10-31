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

    //[SerializeField] private GameObject _loaderCanvas;
    //[SerializeField] private Image _progressBar;
    [SerializeField] private int _loadingTime;

    //[SerializeField] private Image transitionImg;
    //[SerializeField] private float transitionSpeed;
    //private float alpha;
    //[SerializeField] 
    [SerializeField]private Animator _animatorTransition;
    

    [SerializeField] private Image titleIMG;

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

        //_animatorTransition = GameObject.Find("CrossFade").GetComponent<Animator>();
        
    }

    void Start()
    {
        CreatePlayerData();
        //print(_animatorTransition);
    }

    
    public async void LoadScene(string sceneName)
    {
        titleIMG.sprite = Resources.Load<Sprite>(sceneName);
        _animatorTransition.Play("Crossfade_Start");       
       
        var scene = SceneManager.LoadSceneAsync(sceneName);
        scene.allowSceneActivation = false;
       
        //_loaderCanvas.SetActive(true);
        
        do
        {
            await Task.Delay(_loadingTime);

            //_progressBar.fillAmount = scene.progress;

        }while (scene.progress < 0.9f);

       
        scene.allowSceneActivation = true;
        _animatorTransition.SetTrigger("End");
        //_animatorTransition.Play("Entry");
        //_animatorTransition = GameObject.Find("CrossFade").GetComponent<Animator>();

        //_animatorTransition.Play("Entry");

        //_loaderCanvas.SetActive(false);

        //print(_animatorTransition);

        //Sync Data
        if (playerData.level < 1 && sceneName == "NewtonLevel_GPP_Test")
        {
            playerData.level = 1;
            SaveData();
            print("Reached Level" + playerData.level);
        }
        else if(playerData.level < 2 && sceneName == "ELE_GPP Temp")
        {
            playerData.level = 2;
            SaveData();
            print("Reached Level" + playerData.level);
        }

        else if(playerData.level < 3 && sceneName == "Level_Fourier")
        {
            playerData.level = 3;
            SaveData();
            print("Reached Level" + playerData.level);
        }

        

        

      


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


    //协程
    
}
