using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayPointBehaviour : MonoBehaviour
{
    private Vector3 targetPosition;
    private Vector3 startPosition;

    public static int score = 0;
    

    
    public static Action<int> LevelPass;
    public static Action StopShoot;

    public static int passedLevel = 0;
    private void Awake()
    {
        targetPosition = GameObject.Find("AimPoint").transform.position;
        startPosition = transform.position;
    }
    private void Start()
    {

        StartCoroutine(SphereMove());
    }

    

    private IEnumerator SphereMove()
    {
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
        //反应时间内按空格销毁
        float destroyT = 0f;
        while (destroyT < 1f )
        {
            destroyT += Time.deltaTime;
            //Debug.Log(destroyT);
            if (Input.GetKeyDown(KeyCode.Space))
            {
                
                SphereDestroy();    
                
            }
            yield return null;
        }
        //超过反应时间自动销毁
        StartCoroutine(SphereAutoDestroy());
        yield return null;
    }

    //Manually destroy
    private void SphereDestroy()
    {
        score++;
        Debug.Log(score);
        StopCoroutine(SphereMove());
        if (score == 3)
        {
                print(passedLevel);
                LevelPass?.Invoke(passedLevel);
                StopShoot?.Invoke();
                score = 0;
                passedLevel++;
  
        }
        Destroy(gameObject);

       
        
    }

    //Auto destroy
    private IEnumerator SphereAutoDestroy()
    {
        StopCoroutine(SphereMove());
        Destroy(gameObject);
        yield return null;
    }
}
