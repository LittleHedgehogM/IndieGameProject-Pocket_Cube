using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class Newton_Scene_VFX_Controller : MonoBehaviour
{
    //[SerializeField] private GameObject cube_VFX;
    [SerializeField] private GameObject put_coin_VFX;
    [SerializeField] private GameObject take_coin_VFX;

    //[SerializeField] private Transform CubeTransform;
    [SerializeField] private Transform LeftScaleTransform;
    [SerializeField] private Transform RightScaleTransform;
    [SerializeField] private Transform LeftEyeTransform;
    [SerializeField] private Transform RightEyeTransform;

    [SerializeField] private Transform putCoinOffsetTransformLeft;
    [SerializeField] private Transform putCoinOffsetTransformRight;


    //private GameObject cube_VFX_Instance = null;
    //private GameObject DisplayEye_VFX_Instance = null;
    private GameObject DisplayScale_VFX_Instance = null;
    private GameObject put_coin_VFX_Instance = null;


    public void PlayCubeVFX()
    {
        //if (cube_VFX_Instance == null)
        //{
        //    cube_VFX_Instance = Instantiate(cube_VFX, CubeTransform.position, cube_VFX.transform.rotation);
        //    cube_VFX_Instance.transform.SetParent(CubeTransform);
        //    cube_VFX_Instance.GetComponent<ParticleSystem>().Play();
        //}
    }




    public void PlayDisplayEyeAndScaleVFX(Transform scaleTransform)
    {
        if (DisplayScale_VFX_Instance == null)
        {
            if (scaleTransform == LeftScaleTransform)
            {               
                DisplayScale_VFX_Instance = Instantiate(take_coin_VFX, LeftScaleTransform.position, LeftScaleTransform.transform.rotation);
                DisplayScale_VFX_Instance.transform.SetParent(LeftScaleTransform);
                DisplayScale_VFX_Instance.GetComponent<ParticleSystem>().Play();


            }
            else if (scaleTransform == RightScaleTransform)
            {
                DisplayScale_VFX_Instance = Instantiate(take_coin_VFX, RightScaleTransform.position, RightScaleTransform.transform.rotation);
                DisplayScale_VFX_Instance.transform.SetParent(RightScaleTransform);
                DisplayScale_VFX_Instance.GetComponent<ParticleSystem>().Play();


            }

            //var main = DisplayScale_VFX_Instance.GetComponent<ParticleSystem>().main;
            //main.loop = false;
        }

    }

    public void StopDisplayEyeAndScaleVFX()
    {
        if (DisplayScale_VFX_Instance != null)
        {
            DisplayScale_VFX_Instance.GetComponent<ParticleSystem>().Stop();
            DisplayScale_VFX_Instance = null;
        }
        
    }

    public void PlayScalePutVFX(Transform scaleTransform)
    {
        //if (put_coin_VFX_Instance == null)
        //{
            if (scaleTransform == LeftScaleTransform)
            {
                put_coin_VFX_Instance = Instantiate(put_coin_VFX, putCoinOffsetTransformLeft.position, put_coin_VFX.transform.rotation);
                put_coin_VFX_Instance.transform.SetParent(putCoinOffsetTransformLeft);
                put_coin_VFX_Instance.GetComponent<ParticleSystem>().Play();

            }
            else if (scaleTransform == RightScaleTransform)
            {
                put_coin_VFX_Instance = Instantiate(put_coin_VFX, putCoinOffsetTransformRight.position, put_coin_VFX.transform.rotation);
                put_coin_VFX_Instance.transform.SetParent(putCoinOffsetTransformRight);
                put_coin_VFX_Instance.GetComponent<ParticleSystem>().Play();
            }

            //var main = put_coin_VFX_Instance.GetComponent<ParticleSystem>().main;
            //main.loop = false;
        //}
    }

    public void StopScalePutVFX()
    {
        if (put_coin_VFX_Instance != null)
        {
            put_coin_VFX_Instance.GetComponent<ParticleSystem>().Stop();
            put_coin_VFX_Instance = null;
        }
    }
}
