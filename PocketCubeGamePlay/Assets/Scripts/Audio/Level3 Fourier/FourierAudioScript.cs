using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FourierAudioScript : MonoBehaviour
{
    [SerializeField] private AK.Wwise.Bank Fourier_Bank;
    //[SerializeField] private AK.Wwise.Event Fourier_AMB;
    //[SerializeField] private AK.Wwise.Event Fourier_BGM;


    //[SerializeField] private Collider cube;
    private GameObject audioPlayer;


    private void Start()
    {
        audioPlayer = GameObject.Find("WwiseGlobal");
        Fourier_Bank.Load(audioPlayer);


        //Fourier_AMB.Post(audioPlayer);
        //Fourier_BGM.Post(audioPlayer);
        //AkSoundEngine.SetSwitch("ELE_BGM", "Intro", audioPlayer);

    }


}
