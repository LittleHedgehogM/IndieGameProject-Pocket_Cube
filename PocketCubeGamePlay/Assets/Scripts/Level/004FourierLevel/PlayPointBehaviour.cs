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

    public static int score = 0;
    

    
    public static Action<int> LevelPass;
    public static Action StopShoot;

    public static int passedLevel = 0;
    private void Awake()
    {
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
        //Debug.Log("Start Move");
        float t = 0;
        float currentTime = 0;
        float translationTime = 0.5f;

        while ( t < 1)
        {
            currentTime += Time.deltaTime;
            t = currentTime / translationTime;
            transform.position = Vector3.Lerp(startPosition, targetPosition, t);
            yield return null;
        }

        //反应时间内按空格销毁 -perfect
        float responseT = 0f;
        while (responseT < 0.5f )
        {
            responseT += Time.deltaTime;
            //Debug.Log(destroyT);
            if (Input.GetKeyDown(KeyCode.Space))
            {
               PerfectDestroy();                  
            }
            yield return null;
        }

        //超过反应时间自动销毁 -miss
        StartCoroutine(MissDestroy());
        yield return null;
    }




    //Perfect destroy
    private void PerfectDestroy()
    {
        StopCoroutine(SphereMove());
        Debug.Log("Perfect");
        //hit perfect effect
        perfectVFX.Play();
        
        //calculate score
        score++;
        //Debug.Log(score);
        if (score == 3)
        {
                print(passedLevel);
                LevelPass?.Invoke(passedLevel);
                StopShoot?.Invoke();
                score = 0;
                passedLevel++;
        }

        StartCoroutine(SphereReduce());
  
    }

    //Miss destroy
    private IEnumerator MissDestroy()
    {
        StopCoroutine(SphereMove());
        Debug.Log("Miss");
        StartCoroutine(SphereReduce());

        yield return null;
    }

    private IEnumerator SphereReduce()
    {
        //Mesh Reduce
        Debug.Log("Start Reduce");
        Vector3 currentScale = sphereMesh.transform.localScale;
        Vector3 targetScale = Vector3.zero;
        float destroyT = 0;
        float currentTime = 0;
        float translationTime = 0.5f;

        while (destroyT < 1)
        {
            currentTime += Time.deltaTime;
            destroyT = currentTime / translationTime;
            sphereMesh.transform.localScale = Vector3.Lerp(currentScale, targetScale, destroyT);
            yield return null;
        }
        Debug.Log("End Reduce");
        yield return new WaitUntil(() => perfectVFX.isStopped);
        print("perfect vfx stopped");
        yield return null;

        //Destroy Object
        Destroy(gameObject);
    }
}
