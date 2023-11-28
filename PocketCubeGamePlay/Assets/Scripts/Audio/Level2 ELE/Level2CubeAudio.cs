using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Level2CubeAudio : MonoBehaviour
{
    private GameObject audioPlayer;
    [SerializeField]private AK.Wwise.Event outro;
    [SerializeField] private Button finishBtn;
    //[SerializeField] private AK.Wwise.Switch ELE_Outro;

    private void Start()
    {
        audioPlayer = GameObject.Find("WwiseGlobal");
        finishBtn.onClick.AddListener(OnFinishBtn);
    }

    private void OnFinishBtn()
    {
        outro.Post(audioPlayer);
    }
}
