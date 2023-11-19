using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class FourierAniCtl : MonoBehaviour
{
    [SerializeField]Animator ani;
    AnimatorStateInfo stateinfo;
    public static Action PerformCameraFinished;

    private void Start()
    {

            //获取动画层 0 指Base Layer.
            
            //判断是否正在播放walk动画.
                
    }

    private void Update()
    {
        stateinfo = ani.GetCurrentAnimatorStateInfo(0);
        if (!stateinfo.IsName("FourierCam_CubeToPlayer"))
        {
            gameObject.SetActive(false);
            PerformCameraFinished?.Invoke();
        }
    }

}
