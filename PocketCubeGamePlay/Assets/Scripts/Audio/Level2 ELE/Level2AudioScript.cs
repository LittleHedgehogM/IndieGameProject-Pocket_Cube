using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level2AudioScript : MonoBehaviour
{
    [SerializeField] private AK.Wwise.Bank ELE_Bank;
    [SerializeField] private AK.Wwise.Event ELE_AMB;
    [SerializeField] private AK.Wwise.Event ELE_BGM;
    

    //[SerializeField] private Collider cube;
    [SerializeField] private GameObject audioPlayer;


    private void Start()
    {
        ELE_Bank.Load(audioPlayer);


        ELE_AMB.Post(audioPlayer);
        ELE_BGM.Post(audioPlayer);
        AkSoundEngine.SetSwitch("ELE_BGM", "Intro", audioPlayer);
        
    }

    

}
