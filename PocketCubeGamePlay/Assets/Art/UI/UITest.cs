using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UITest : MonoBehaviour
{
    public Button startBtn;

    void StartGame()
    {
        print("Start");
    }
    // Start is called before the first frame update
    void Start()
    {
        startBtn.onClick.AddListener(StartGame);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
