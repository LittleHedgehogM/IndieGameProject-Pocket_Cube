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

    [SerializeField] private List<int> pattern;

    [SerializeField] private GameObject eyeInner_1;
    [SerializeField] private GameObject eyeInner_2;
    [SerializeField] private GameObject eyeInner_3;

    static public int currentBeat;

    private GameObject audioPlayer;
    private void Awake()
    {
        audioPlayer = GameObject.Find("WwiseFourier");
        eyeInner_1.SetActive(false);
        eyeInner_2.SetActive(false);
        eyeInner_3.SetActive(false);
    }

    private void OnEnable()
    {
        EyeEmitter.Eye_Activated += EyeAnimationTrigger;
        EyeEmitter.Eye_InPosition += StoreEyeName;
        RhythmCallBack.Rhythm_Bar += SpawnCall;
        PlayPointBehaviour.StopShoot += StopShoot;
        
    }

    private void EyeAnimationTrigger(string eyeName)
    {
        
        Animator animator = transform.Find(eyeName).gameObject.GetComponent<Animator>();
        animator.SetTrigger(eyeName);
        AkSoundEngine.PostEvent("Play_Level3_Eyesopen", audioPlayer);
        FourierPlayer.playerMovementEnabled = false;
    }

    private void StoreEyeName(string eyeName)
    {
        _eyeName = eyeName;
        _readyToShoot = true;
        if (eyeName.Contains("first"))
        {
            eyeInner_1.SetActive(true);
        }
        else if (eyeName.Contains("sec"))
        {
            eyeInner_2.SetActive(true);
        }
        else if (eyeName.Contains("third"))
        {
            eyeInner_3.SetActive(true);
        }

    }

    private void StopShoot()
    {
        _readyToShoot = false;
        AkSoundEngine.PostEvent("Play_Level3_Getit", audioPlayer);
        eyeInner_1.SetActive(false);
        eyeInner_2.SetActive(false);
        eyeInner_3.SetActive(false);
    }

    private void SpawnCall(int beat)
    {
        if (!_readyToShoot)
        {
            return;
        }
        GameObject go = transform.Find(_eyeName).gameObject;

        currentBeat = PlayPointBehaviour.inLevel * 100 + beat;
        StartCoroutine(PatternSpawn(currentBeat, go));       
    }

    private IEnumerator PatternSpawn(int currentBeat, GameObject go)
    {
        print($"{currentBeat}" + "Born");
        if (!pattern.Contains(currentBeat))
        {
            yield break;
        }
        GameObject newGo = Instantiate(playPointPrefab, relativeSpawnPoints[Random.Range(0, relativeSpawnPoints.Count)].position, Quaternion.identity);
        newGo.transform.Find("SphereMesh").GetComponent<Renderer>().material = go.GetComponent<Renderer>().material;

        yield return null;
    }
}
