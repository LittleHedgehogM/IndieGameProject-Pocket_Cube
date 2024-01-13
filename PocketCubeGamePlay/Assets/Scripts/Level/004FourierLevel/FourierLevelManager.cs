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
            case 0:
                levelPassVFX_1.Play();
                animator_1.SetTrigger("first_pass");
                //StartCoroutine(LevelChangeColor());
                
                bridge_1.gameObject.SetActive(true);
                
                Debug.Log(levelPassVFX_1.name);
                break;

            case 1: 
                levelPassVFX_2.Play();
                animator_2.SetTrigger("sec_pass");
                bridge_2.gameObject.SetActive(true);

                break;

            case 2: 
                levelPassVFX_3.Play();
                animator_3.SetTrigger("third_pass");
                cubeUnlock.Play();
                CubeUnlocked?.Invoke();
                break;

        }

        FourierPlayer.playerMovementEnabled = true;
    }

    /*private void BridgeShow(int level)
    {
        switch(level)
        {
            case 0:
                
                break;
        }
    }*/
    /*private IEnumerator LevelChangeColor()
    {
        Material material = level1.GetComponent<Renderer>().material;
        Color currentColor01 = material.GetColor("_diffusegradient01");
        Color currentColor02 = material.GetColor("_diffusegradient02");

        float currentTime = 0;
        float targetPoint = 0;
        float transitionTimeColor = 0.5f;
        while (targetPoint < 1)
        {

            currentTime += Time.deltaTime;
            targetPoint = currentTime / transitionTimeColor;

            material.SetColor("_diffusegradient01", Color.Lerp(currentColor01, levelColor1_01, targetPoint));
            material.SetColor("_diffusegradient02", Color.Lerp(currentColor02, levelColor1_02, targetPoint));

            yield return null;
        }
        bridge1.gameObject.SetActive(true);
        yield return null;
    }*/

    /*    [SerializeField] private Color goalColor;
        //[SerializeField] bool isBridgeActive;


        private Material material;
        //public GameObject bridge;

        private bool levelEnter = false;
        public bool isLevelPass = false;
        private int levelFirstEnter = 0;

        //[SerializeField] 
        private ParticleSystem levelPassFx;
        [SerializeField] ParticleSystem cubeLevelPassFx;

        void Awake()
        {
            material = GetComponent<Renderer>().material; 
            levelPassFx = GetComponentInChildren<ParticleSystem>();

        }

        void Update()
        {


            if (levelEnter & material.GetColor("_diffusegradient01") == goalColor & levelFirstEnter == 1 & !isLevelPass)
            {
                isLevelPass = true;
                levelPassFx.Play();
                AkSoundEngine.PostEvent("Play_Unlock", gameObject);
                cubeLevelPassFx.Play();

                //levelExit.SetActive(false);
                //print(this.gameObject.name + "pass");
            }

        }


        private void OnCollisionEnter(Collision col)
        {
            if (col.gameObject.CompareTag("Player"))
            {
                //print(levelFirstEnter + gameObject.name);
                levelEnter = true;           
            }
        }

        private void OnCollisionExit(Collision col)
        {
            if (col.gameObject.CompareTag("Player"))
            {
                levelEnter = false;
                //print("Player Exit" + gameObject.name);
                if (levelFirstEnter == 0)
                {
                    levelFirstEnter++;
                }
            }
        }*/

}
