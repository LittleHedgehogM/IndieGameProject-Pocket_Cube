using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayPointBehaviour : MonoBehaviour
{
    private Vector3 targetPosition;
    private Vector3 startPosition;

    private ParticleSystem perfectVFX;
    private GameObject sphereMesh;

    [SerializeField]private Color particleVanish;

    public static int score = 0;
    private float buffTime = 0.8f;
    
    public static Action<int> LevelPass;
    public static Action StopShoot;

    public static int inLevel = 1;

    //Audio
    private GameObject audioPlayer;

    private void Awake()
    {
        audioPlayer = GameObject.Find("WwiseFourier");
        targetPosition = GameObject.Find("AimPoint").transform.position;
        startPosition = transform.position;
        perfectVFX = transform.Find("PerfectVFX").GetComponent<ParticleSystem>();
        sphereMesh = transform.Find("SphereMesh").gameObject;
    }
    private void Start()
    {

        StartCoroutine(SphereMove());
    }

    

    private IEnumerator SphereMove()
    {
        int thisBallNum = EyeCtl.currentBeat;
        //Pre
        yield return new WaitForSeconds(0.2f);
        //Debug.Log("Start Move");
        float t = 0;
        float currentTime = 0;
        //fly time
        float translationTime = 0.8f;

        while ( t < 1)
        {
            currentTime += Time.deltaTime;
            t = currentTime / translationTime;
            transform.position = Vector3.Lerp(startPosition, targetPosition, t);
            if (Input.GetKeyDown(KeyCode.Space))
            {
                StartCoroutine(MissDestroy());
                yield break;

            }
            yield return null;
        }
        Debug.Log(thisBallNum + "Arrived");

        //反应时间内按空格销毁 -perfect
        float responseT = 0f;
        while (responseT <= buffTime)
        {
            responseT += Time.deltaTime;
            //Debug.Log(destroyT);

            //Score system
            if (Input.GetKeyDown(KeyCode.Space))
            {
                if (responseT < 0.2f)
                {
                    Debug.Log(responseT);
                    StartCoroutine(MissDestroy());
                    yield break;
                }
                else if (responseT >= 0.2f && responseT <= buffTime)
                {
                    Debug.Log(responseT);
                    StartCoroutine(PerfectDestroy());
                    yield break;
                }
            } 
            yield return null;
        }

        //超过反应时间自动销毁 -miss
        StartCoroutine(MissDestroy());
        yield return null;
    }




    //Perfect destroy
    private IEnumerator PerfectDestroy()
    {
        
        Debug.Log("Perfect");
        //hit perfect effect+ audio
        perfectVFX.Play();
        //AkSoundEngine.PostEvent("Play_L3_perfect_A", audioPlayer);

        //delete sphere
        sphereMesh.transform.localScale = Vector3.zero;

        //calculate score
        score++;
        //Debug.Log(score);
        if (score == 6)
        {
                print(inLevel);
                LevelPass?.Invoke(inLevel);
                StopShoot?.Invoke();
                score = 0;
                inLevel++;
        }

        yield return new WaitUntil(() => perfectVFX.isStopped);
        print("perfect vfx stopped");
        yield return null;

        //Destroy Object
        Destroy(gameObject);

    }

    //Miss destroy
    private IEnumerator MissDestroy()
    {   
        Debug.Log("Miss");
        //Reduce score
        score = 0;

        //miss show
        //Change Particle Color
        ParticleSystem[] particleSystems = GetComponentsInChildren<ParticleSystem>();
        foreach (var ps in particleSystems)
        {
            ParticleSystem.MainModule main = ps.main;
            main.startColor = new ParticleSystem.MinMaxGradient(particleVanish, Color.white);
;
        }
        //play bad audio
        AkSoundEngine.PostEvent("Play_L3_Bad", audioPlayer);

        yield return new WaitForSeconds(0.3f);
        //Mesh Reduce
        //Debug.Log("Start Reduce");
        Vector3 currentScale = sphereMesh.transform.localScale;
        Vector3 targetScale = Vector3.zero;
        float destroyT = 0;
        float currentTime = 0;
        float translationTime = 0.3f;

        while (destroyT < 1)
        {
            currentTime += Time.deltaTime;
            destroyT = currentTime / translationTime;
            sphereMesh.transform.localScale = Vector3.Lerp(currentScale, targetScale, destroyT);
            yield return null;
        }
        //Debug.Log("End Reduce");


        //Destroy Object
        Destroy(gameObject);

        yield return null;
    }

     
    
}
