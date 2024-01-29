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

    //[SerializeField] private List<int> pattern;
    

    [SerializeField] private GameObject eyeInner;
    [SerializeField] private GameObject eyeBeat;

    private bool innerReady = false;

    static public int currentBeat;

    private GameObject audioPlayer;
    private void Awake()
    {
        audioPlayer = GameObject.Find("WwiseFourier");
        eyeInner.SetActive(false);

        //reset fourier status
        PlayPointBehaviour.inLevel = 1;

    }

    private void OnEnable()
    {
        EyeEmitter.Eye_Activated += EyeAnimationTrigger;
        EyeEmitter.Eye_InPosition += StoreEyeName;
        RhythmCallBack.Rhythm_Beat += SpawnCall;
        PlayPointBehaviour.StopShoot += StopShoot;
        RhythmCallBack.Rhythm_Bar += InnerParticleShoot;
    }
    private void OnDisable()
    {
        EyeEmitter.Eye_Activated -= EyeAnimationTrigger;
        EyeEmitter.Eye_InPosition -= StoreEyeName;
        RhythmCallBack.Rhythm_Beat -= SpawnCall;
        RhythmCallBack.Rhythm_Bar -= InnerParticleShoot;
        PlayPointBehaviour.StopShoot -= StopShoot;
    }

    private void EyeAnimationTrigger(string eyeName)
    {
        
        Animator animator = transform.Find(eyeName).gameObject.GetComponent<Animator>();
        animator.SetTrigger(eyeName);

        AkSoundEngine.PostEvent("Play_Level3_Eyesopen", audioPlayer);
        Debug.Log("Eye_Activated");
        AkSoundEngine.SetSwitch("Fourier_Level",$"level{PlayPointBehaviour.inLevel}_1", audioPlayer);
        Debug.Log($"level{PlayPointBehaviour.inLevel}_1");
        FourierPlayer.playerMovementEnabled = false;
    }

    private void StoreEyeName(string eyeName)
    {
        _eyeName = eyeName;
        StartCoroutine(WaitToShoot());
        innerReady = true;    
        
    }
    private void InnerParticleShoot()
    {
        if (!innerReady) return;
        eyeBeat.SetActive(true);
    }

    private void StopShoot(string levelState)
    {
        if (levelState == "Entire")
        {
            _readyToShoot = false;
            innerReady = false;
            AkSoundEngine.PostEvent("Play_Level3_Getit", audioPlayer);
            eyeInner.SetActive(false);
        }
        else if (levelState == "Half")
        {
            _readyToShoot = false;
            StartCoroutine(WaitToShoot());
        }       
    }

    private IEnumerator WaitToShoot()
    {
        yield return new WaitForSeconds(2f);
        _readyToShoot = true;
        yield return null;
    }

    private void SpawnCall()
    {
        if (!_readyToShoot)
        {
            return;
        }
        GameObject go = transform.Find(_eyeName).gameObject;
        GameObject newGo = Instantiate(playPointPrefab, relativeSpawnPoints[Random.Range(0, relativeSpawnPoints.Count)].position, Quaternion.identity);
        newGo.name = $"[{PlayPointBehaviour._uniqOrder}]";
        newGo.transform.Find("SphereMesh").GetComponent<Renderer>().material = go.GetComponent<Renderer>().material;         
    }
}
