using System;
using System.Collections;
using UnityEngine;

public class FourierLevelManager : MonoBehaviour
{
    [SerializeField] ParticleSystem levelPassVFX_1;    
    [SerializeField] ParticleSystem levelPassVFX_2;
    [SerializeField] ParticleSystem levelPassVFX_3;

    [SerializeField] ParticleSystem cubeUnlock;

    [SerializeField] Animator animator_1;
    [SerializeField] Animator animator_2;
    [SerializeField] Animator animator_3;

    [SerializeField] GameObject bridge_1;
    [SerializeField] GameObject bridge_2;
    /*[SerializeField] GameObject level1;
    [SerializeField] Color levelColor1_01;
    [SerializeField] Color levelColor1_02;*/

    //[SerializeField] GameObject bridge1;

    public static Action CubeUnlocked;
    private void OnEnable()
    {
        PlayPointBehaviour.LevelPass += LevelPassShow;
    }

    private void Awake()
    {
        bridge_1.gameObject.SetActive(false);
        bridge_2.gameObject.SetActive(false);
    }

    private void LevelPassShow(int level)
    {
        
        
        switch (level)
        {
            case 1:
                levelPassVFX_1.Play();
                animator_1.SetTrigger("first_pass");
                //StartCoroutine(LevelChangeColor());
                
                bridge_1.gameObject.SetActive(true);
                
                Debug.Log(levelPassVFX_1.name);
                break;

            case 2: 
                levelPassVFX_2.Play();
                animator_2.SetTrigger("sec_pass");
                bridge_2.gameObject.SetActive(true);

                break;

            case 3: 
                levelPassVFX_3.Play();
                animator_3.SetTrigger("third_pass");
                cubeUnlock.Play();
                CubeUnlocked?.Invoke();
                break;

        }

        FourierPlayer.playerMovementEnabled = true;
    }


}
