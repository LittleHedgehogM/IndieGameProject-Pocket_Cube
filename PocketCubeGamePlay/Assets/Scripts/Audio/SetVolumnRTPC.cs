using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SetVolumnRTPC : MonoBehaviour
{
    private Slider slider;
    private GameObject audioPlayer;
    public AK.Wwise.RTPC VolumnRTPC;
    private float currentVol;
    // 使用此函数进行初始化。
    // Start is called before the first frame update
    private void Awake()
    {
         slider = GetComponent<Slider>();
         audioPlayer = GameObject.Find("WwiseGlobal");
    }
    void Start()
    {
        currentVol = VolumnRTPC.GetValue(audioPlayer);
        Debug.Log(currentVol);
        slider.value = currentVol;
    }

    // Update is called once per frame


    public void VolumnChange()
    {
        //currentVol = slider.value;

        VolumnRTPC.SetValue(audioPlayer,slider.value);
       
    }
}
