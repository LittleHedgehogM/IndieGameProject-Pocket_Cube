using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class eyeVFXManager : MonoBehaviour
{
    [SerializeField] ParticleSystem eye_1;
    [SerializeField] ParticleSystem eye_2;
    [SerializeField] ParticleSystem eye_3;

    private void Start()
    {
        eye_2.Stop();
        eye_3.Stop();

    }

    private void OnEnable()
    {
        EyeEmitter.Eye_Activated += turnOffEyeParticle;
        PlayPointBehaviour.LevelPass += turnOnEyeParticles;
    }

    private void turnOffEyeParticle(string eyeName)
    {
        if (eyeName == "first_eye_show")
        {
            eye_1.Stop();
        }
        else if (eyeName == "sec_eye_show")
        {
            eye_2.Stop();
        }
        else if(eyeName == "third_eye_show")
        {
            eye_3.Stop();
        }
    }
    private void turnOnEyeParticles(int level)
    {
        switch (level)
        {
            case 0:
                eye_2.Play();
                break;
                case 1:
                eye_3.Play();
                break;
        }
    }
}
