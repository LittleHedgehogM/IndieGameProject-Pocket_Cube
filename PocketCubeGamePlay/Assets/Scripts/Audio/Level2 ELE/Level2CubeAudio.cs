using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Level2CubeAudio : MonoBehaviour
{
    [SerializeField] private GameObject audioPlayer;
    [SerializeField] private Button finishBtn;
    [SerializeField] private AK.Wwise.Switch ELE_Outro;

    private void Start()
    {
        finishBtn.onClick.AddListener(OnFinishBtn);
    }

    private void OnFinishBtn()
    {
        ELE_Outro.SetValue(audioPlayer);
    }
}
