using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;

public class EyeCtl : MonoBehaviour
{
    public GameObject playPointPrefab;
    [SerializeField] private List<Transform> relativeSpawnPoints;
    [SerializeField] private Transform aimPoint;
    [SerializeField] private float speed;
    private string _eyeName;
    private bool _readyToShoot = false;
    private void OnEnable()
    {
        EyeEmitter.Eye_Activated += EyeAnimationTrigger;
        EyeEmitter.Eye_InPosition += StoreEyeName;
        RhythmCallBack.Rhythm_Bar += SpawnPoints;
        PlayPointBehaviour.StopShoot += StopShoot;
    }

    private void EyeAnimationTrigger(string eyeName)
    {
        
        Animator animator = transform.Find(eyeName).gameObject.GetComponent<Animator>();
        animator.SetTrigger(eyeName);
        FourierPlayer.playerMovementEnabled = false;
    }

    private void StoreEyeName(string eyeName)
    {
        _eyeName = eyeName;
        _readyToShoot = true;
    }

    private void StopShoot()
    {
        _readyToShoot = false;
    }

    private void SpawnPoints(int beat)
    {
        if (!_readyToShoot)
        {
            return;
        }
        GameObject go = transform.Find(_eyeName).gameObject;
        if (beat > 1)
        {
            GameObject newGo = Instantiate(playPointPrefab, relativeSpawnPoints[Random.Range(0, relativeSpawnPoints.Count)].position, Quaternion.identity);
            newGo.GetComponent<Renderer>().material = go.GetComponent<Renderer>().material;
        }
        
    }
    
}
