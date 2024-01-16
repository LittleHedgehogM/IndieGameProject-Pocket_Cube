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
    //sphere lifetime 
    private float waitTime = 0f;
    private float translationTime = 1.4f;
    private float buffTime = 0.5f;


    public static Action<int> LevelPass;
    public static Action<string> StopShoot;
    public static Action OrderReduce;
    public static Action SelfElimination;

    public static int inLevel = 1;

    //Audio
    private GameObject audioPlayer;

    public static int globalOrder = 0;
    private int myOrder;

    private void Awake()
    {
        audioPlayer = GameObject.Find("WwiseFourier");
        targetPosition = GameObject.Find("AimPoint").transform.position;
        startPosition = transform.position;
        perfectVFX = transform.Find("PerfectVFX").GetComponent<ParticleSystem>();
        sphereMesh = transform.Find("SphereMesh").gameObject;

        //sphere order detective
        globalOrder++;
        myOrder = globalOrder;
        //Debug.Log("Ball Initiated; " + $"Current GlobalOrder:{globalOrder};My order:{myOrder}");
    }
    private void Start()
    {

        StartCoroutine(SphereMove());
    }

    private void OnEnable()
    {
        OrderReduce += ReduceSphereOrder;
        SelfElimination += LevelPassDestroy;
    }


    private IEnumerator SphereMove()
    {
        int thisBallNum = EyeCtl.currentBeat;
        //Pre
        yield return new WaitForSeconds(waitTime);
        //Debug.Log("Start Move");
        float t = 0;
        float currentTime = 0;
        //fly time
        //float translationTime = 0.8f;

        while ( t < 1)
        {
            currentTime += Time.deltaTime;
            t = currentTime / translationTime;
            transform.position = Vector3.Lerp(startPosition, targetPosition, t);
            if (myOrder == 1 && Input.GetKeyDown(KeyCode.Space))
            {
                //Debug.Log("Ball Destroy; " + $"Current GlobalOrder:{globalOrder};My order:{myOrder}");
                StartCoroutine(MissDestroy());
                yield return null;
                
                yield break;

            }
            yield return null;
        }
        //Debug.Log(thisBallNum + "Arrived");

        //反应时间内按空格销毁 -perfect
        float responseT = 0f;
        while (responseT <= buffTime)
        {
            responseT += Time.deltaTime;
            //Debug.Log(destroyT);

            //Score system
            if (myOrder == 1 && Input.GetKeyDown(KeyCode.Space))
            {
                /*if (responseT < 0.2f)
                {
                    Debug.Log(responseT);
                    StartCoroutine(MissDestroy());
                    yield break;
                }*/
                if (responseT <= buffTime)
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
        OrderReduce?.Invoke();
        //Debug.Log("Perfect");
        //hit perfect effect+ audio
        perfectVFX.Play();
        AkSoundEngine.PostEvent("Play_L3_Perfect", audioPlayer);

        //delete sphere
        sphereMesh.transform.localScale = Vector3.zero;

        score++;
        //Debug.Log(score);
        
        uint currentSwitch;
        AkSoundEngine.GetSwitch("Fourier_Level", audioPlayer, out currentSwitch);

        //half level
        if (currentSwitch == 2867782686 && score == 4)
        {
            AkSoundEngine.SetSwitch("Fourier_Level", "level1_2", audioPlayer);
            score = 0;
            StopShoot?.Invoke("Half");
            yield return new WaitUntil(() => perfectVFX.isStopped);
            //print("perfect vfx stopped");
            //Destroy Object
            
            Destroy(gameObject);
            yield break;
        }
        //whole level
        else if (!(currentSwitch == 2867782686) && score == 6)
        {
                print(inLevel);
                LevelPass?.Invoke(inLevel);
                StopShoot?.Invoke("Entire");
                score = 0;
                inLevel++;
                SelfElimination?.Invoke();
        }

        yield return new WaitUntil(() => perfectVFX.isStopped);
        //print("perfect vfx stopped");
        yield return null;

        //Destroy Object
        Destroy(gameObject);

    }

    //Miss destroy
    private IEnumerator MissDestroy()
    {
        OrderReduce?.Invoke();
        //Debug.Log("Miss");
        //Reduce score
        score = 0;

        //miss show
        //Change Particle Color
        ParticleSystem[] particleSystems = GetComponentsInChildren<ParticleSystem>();
        foreach (var ps in particleSystems)
        {
            ParticleSystem.MainModule main = ps.main;
            main.startColor = new ParticleSystem.MinMaxGradient(particleVanish, Color.white);
        }
        //play bad audio
        AkSoundEngine.PostEvent("Play_L3_Bad", audioPlayer);

        //yield return new WaitForSeconds(0.3f);
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

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("RhythmSphere"))
        {

            //spaceSwitch = true;
        }
    }

    public void ReduceSphereOrder()
    {

        //Debug.Log("Start ReduceSphereOrder;" + $"Current GlobalOrder:{globalOrder};My order:{myOrder}");
        if (myOrder == 0)
        {
            //Debug.Log("Already reduced");
            return;
        }
        else if (myOrder == 1)
        {
            globalOrder = globalOrder - 1;
            myOrder = 0;
            //Debug.Log("End ReduceSphereOrder;" + $"Current GlobalOrder:{globalOrder};My order:{myOrder}");
        }
        else if (myOrder > 1)
        {
            myOrder = myOrder - 1;
            //Debug.Log("End ReduceSphereOrder;" + $"Current GlobalOrder:{globalOrder};My order:{myOrder}");
        }
        
    }

    private void LevelPassDestroy()
    {
        if (!(myOrder == 0) )
        {
            StartCoroutine(SelfDestroy());
        }
        
    }
    private IEnumerator SelfDestroy()
    {
        
        ReduceSphereOrder();
        yield return null;
        Destroy(gameObject);
    }
}
