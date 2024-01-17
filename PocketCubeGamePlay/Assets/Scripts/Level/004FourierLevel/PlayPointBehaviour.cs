using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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
    public static Action<int> LevelPass;
    public static Action<string> StopShoot;
    public static Action OrderReduce;
    public static Action<int> ScoreChanged;

    public static int inLevel = 1;

    //Audio
    private GameObject audioPlayer;
    //Ball Manage
    public static int globalOrder = 0;//目前场景存在的有效球的数量 有效：可以交互或将可交互
    private int myOrder;
    public static int _uniqOrder = 0;
    private int uniqOrder;
    private bool thisLevelPassed;
    //fly liftime
    private float waitTime;
    private float translationTime;
    private float buffTime_before;
    private float buffTime_after;
    private float buffTime;
    //View
    private Camera mainCamera;
    //Key Lock
    public static bool spaceReady=true;
    private void Awake()
    {
        //init level state
        thisLevelPassed = false;

        audioPlayer = GameObject.Find("WwiseFourier");
        targetPosition = GameObject.Find("AimPoint").transform.position;
        startPosition = transform.position;
        perfectVFX = transform.Find("PerfectVFX").GetComponent<ParticleSystem>();
        sphereMesh = transform.Find("SphereMesh").gameObject;
        _uniqOrder++;
        uniqOrder = _uniqOrder;
        //sphere order detective
        globalOrder++;
        myOrder = globalOrder;
        Debug.Log("Ball Initiated; " + $"Current GlobalOrder:{globalOrder};My order:{myOrder};uniqOrder:{uniqOrder}");
        //Anounce sphere lifetime (waitTime + translationTime + buffTime_before = loopTime)
        switch (inLevel)
        {
            case 1:
                waitTime = 0f;
                translationTime = 1.4f;
                buffTime_before = 0.1f;
                buffTime_after = 0.1f;
                break;
            case 2:
                waitTime = 2.1f;
                translationTime = 0.8f;
                buffTime_before = 0.1f;
                buffTime_after = 0.1f;
                break;
            case 3:
                waitTime = 0.9f;
                translationTime = 0.5f;
                buffTime_before = 0.1f;
                buffTime_after = 0.1f;
                break;
        }
        buffTime = buffTime_before + buffTime_after;
    }
    private void Start()
    {
        mainCamera = Camera.main; 
        StartCoroutine(SphereMove());
        //Debug.Log("Ball Initiated; " + $"Current GlobalOrder:{globalOrder};My order:{myOrder}");
    }

    private void OnEnable()
    {
        OrderReduce += myOrderReduce;
        LevelPass += LevelPassDestroy;
    }
    private void OnDisable()
    {
        OrderReduce -= myOrderReduce;
        LevelPass -= LevelPassDestroy;
    }

    private IEnumerator SphereMove()
    {
        //Debug.Log("Start Move");
        //Pre
        yield return new WaitForSeconds(waitTime);
        //Debug.Log("Start Move");
        float t = 0;
        float currentTime = 0;
        //Debug.Log($"[{uniqOrder}]Starts to Move");
        while ( t < 1)
        {
            Vector3 viewportPosition = mainCamera.WorldToViewportPoint(gameObject.transform.position);
            currentTime += Time.deltaTime;
            t = currentTime / translationTime;
            transform.position = Vector3.Lerp(startPosition, targetPosition, t);
            if (myOrder == 1 && Input.GetKeyDown(KeyCode.Space) && spaceReady && viewportPosition.x > 0 && viewportPosition.x < 1 && viewportPosition.y > 0 && viewportPosition.y < 1 && viewportPosition.z > 0)
            {
                StartCoroutine(SpaceDown("Miss"));              
                yield break;
            }
            yield return null;
        }
        //Debug.Log(thisBallNum + "Arrived");

        //反应时间内按空格销毁 -得分 -perfect
        float responseT = 0f;
        while (responseT <= buffTime)
        {
            responseT += Time.deltaTime;
            //Debug.Log(destroyT);
            if (myOrder == 1 && Input.GetKeyDown(KeyCode.Space))
            {
                Debug.Log(responseT);
                StartCoroutine(SpaceDown("Perfect"));
                yield break;            
            } 
            yield return null;
        }
        //超过反应时间自动销毁 -miss
        print("overtime Miss");
        StartCoroutine(MissDestroy());
    }

    private IEnumerator SpaceDown(string type)
    {
        spaceReady = false;
        
        if (type == "Miss")
        {
            print("Space Down Miss");
            StartCoroutine(MissDestroy());
        }
        else if (type == "Perfect")
        {
            print("Space Down perfect");
            PerfectDestroy();
        }
        yield return new WaitForSeconds(0.5f);
        spaceReady = true;

    }

    //Perfect destroy
    private void PerfectDestroy()
    {
        OrderReduce?.Invoke();
        //VFX & SFX
        perfectVFX.Play();
        AkSoundEngine.PostEvent("Play_L3_Perfect", audioPlayer);
        //delete sphere
        sphereMesh.transform.localScale = Vector3.zero;
        //Increase score
        score++;
        ScoreChanged?.Invoke(score);
        //Debug.Log(score)
        switch (inLevel)
        {
            case 1:
                StartCoroutine(ScoreCalculationLevel1());
                break;
            case 2:
                StartCoroutine(ScoreCalculationLevel2());
                break;
            case 3:
                StartCoroutine(ScoreCalculationLevel3());
                break;
        }
        
    }
    //Pass Level Rule
    private IEnumerator ScoreCalculationLevel1()
    {
        uint currentSwitch;
        AkSoundEngine.GetSwitch("Fourier_Level", audioPlayer, out currentSwitch);
        //half level
        if (currentSwitch == 2867782686 && score == 4)
        {
            AkSoundEngine.SetSwitch("Fourier_Level", "level1_2", audioPlayer);
            score = 0;
            //ScoreChanged?.Invoke(score);
            StopShoot?.Invoke("Half");
            yield return new WaitUntil(() => perfectVFX.isStopped);
            //print("perfect vfx stopped");
            //Destroy Object
            Destroy(gameObject);
            yield break;
        }
        //When scores are enough
        else if (!(currentSwitch == 2867782686) && score == 8)
        {
            print(inLevel);
            LevelPass?.Invoke(inLevel);
            StopShoot?.Invoke("Entire");
            print("Level1_2 pass");
            score = 0;
            inLevel++;
            yield return null;
        }
        //When scores are not enough
        yield return new WaitUntil(() => perfectVFX.isStopped);
        //print("perfect vfx stopped");
        yield return null;
        //Destroy Object
        Destroy(gameObject);
    }
    private IEnumerator ScoreCalculationLevel2()
    {
        //hit perfect effect+ audio
        perfectVFX.Play();
        AkSoundEngine.PostEvent("Play_L3_Perfect", audioPlayer);

        //delete sphere
        sphereMesh.transform.localScale = Vector3.zero;
        //Debug.Log("Current Score:" + score);   
        if (score == 9)
        {
            print(inLevel);
            LevelPass?.Invoke(inLevel);
            StopShoot?.Invoke("Entire");
            score = 0;
            inLevel++;
        }

        yield return new WaitUntil(() => perfectVFX.isStopped);
        //print("perfect vfx stopped");
        yield return null;

        //Destroy Object
        Destroy(gameObject);

    }
    private IEnumerator ScoreCalculationLevel3()
    {
        //hit perfect effect+ audio
        perfectVFX.Play();
        AkSoundEngine.PostEvent("Play_L3_Perfect", audioPlayer);

        //delete sphere
        sphereMesh.transform.localScale = Vector3.zero;
        //Debug.Log(score);   
        if (score == 12)
        {
            print(inLevel);
            LevelPass?.Invoke(inLevel);
            StopShoot?.Invoke("Entire");
            score = 0;
            inLevel++;
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
        //Reduce score
        score = 0;
        ScoreChanged?.Invoke(score);
        //miss show
        //Change Particle Color
        ParticleSystem[] particleSystems = GetComponentsInChildren<ParticleSystem>();
        foreach (var ps in particleSystems)
        {
            ParticleSystem.MainModule main = ps.main;
            main.startColor = new ParticleSystem.MinMaxGradient(particleVanish, Color.white);
        }
        //Mesh Reduce
        //Debug.Log("Start Reduce");
        Vector3 currentScale = sphereMesh.transform.localScale;
        Vector3 targetScale = Vector3.zero;
        float destroyT = 0;
        float currentTime = 0;
        float translationTime = 0.1f;
        //play bad audio
        AkSoundEngine.PostEvent("Play_L3_Bad", audioPlayer);
        while (destroyT < 1)
        {
            currentTime += Time.deltaTime;
            destroyT = currentTime / translationTime;
            sphereMesh.transform.localScale = Vector3.Lerp(currentScale, targetScale, destroyT);
            yield return null;
        }
        //Debug.Log("End Reduce");
        Destroy(gameObject);
    }

    private void myOrderReduce()
    {
        if (myOrder == 0)
        {
            //Debug.Log($"[{uniqOrder}]My order = 0, Stop reduce");
            return;
        }
        else if (myOrder > 0)
        {
            StartCoroutine(SelfReduce());
        }
        
    }

    private IEnumerator SelfReduce()
    {   
        myOrder--;
        //Debug.Log($"[{uniqOrder}]Myorder Reduced to:{myOrder}");
        if (myOrder == 0)
        {
            globalOrder--;
            if (thisLevelPassed)
            {
                globalOrder = 0;
            }
            //Debug.Log($"[{uniqOrder}]Global Reduced to:{globalOrder}");
        }
        yield return null;
    }

    private void LevelPassDestroy(int level)
    {
        StopCoroutine(SelfReduce());
        thisLevelPassed = true;
        globalOrder = 0;
        //Debug.Log($"[{uniqOrder}]Level pass!global Order reset to 0");
        if (myOrder > 0)
        {
            StartCoroutine(SelfDestroy());
        }       
    }
    private IEnumerator SelfDestroy()
    {
        //Debug.Log($"[{uniqOrder}]Level passed!myOder{myOrder}, destroy myself");
        yield return null;
        Destroy(gameObject);
    }
}
