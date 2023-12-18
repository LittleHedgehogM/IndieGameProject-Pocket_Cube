using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SetVolumnRTPC : MonoBehaviour
{
    public Slider slider;
    public AK.Wwise.RTPC VolumnRTPC;

    // 使用此函数进行初始化。
    // Start is called before the first frame update
    void Start()
    {
        slider.value = 75f;
    }

    // Update is called once per frame


    public void VolumnChange()
    {
        //float sliderValue = thisSlider.value;

        VolumnRTPC.SetGlobalValue(slider.value);
        
    }
}
