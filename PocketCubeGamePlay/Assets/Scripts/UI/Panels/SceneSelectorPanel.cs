using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SceneSelectorPanel : MonoBehaviour
{
    [SerializeField] private Button nextScene_Btn;
    [SerializeField] private GameObject panel;
    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(gameObject);
        nextScene_Btn.onClick.AddListener(OnNextSceneBtn);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            if (panel.activeSelf)
            {
                panel.SetActive(false);
            }
            else if (!panel.activeSelf)
            {
                panel.SetActive(true);
            }
        }
    }

    void OnNextSceneBtn()
    {
        switch (PlayerPrefs.GetInt("Level"))
        {
            case 0:
                LevelManager.Instance.LoadScene("NewtonLevel_GPP_Test");
                break;
            case 1:
                LevelManager.Instance.LoadScene("ELE_GPP Temp");
                break;
            case 2:
                LevelManager.Instance.LoadScene("Level_Fourier");
                break;
        }
    }
}
