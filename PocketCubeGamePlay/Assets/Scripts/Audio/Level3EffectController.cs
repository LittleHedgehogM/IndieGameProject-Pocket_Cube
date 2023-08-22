using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level3EffectController : MonoBehaviour
{
    public AK.Wwise.Event OnEffect;
    public AK.Wwise.Event OffEffect;
    public GameObject AudioPlayer;
    bool effectOn = true;

    // Start is called before the first frame update
   

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider col)
    {
        
        if (col.CompareTag("Player"))
        {

            print("enter");

            
            if (!effectOn)
            {
                effectOn = true;
                OnEffect.Post(AudioPlayer);
                GetComponent<Renderer>().material.color = Color.white;
            }

            else if (effectOn)
            {
                effectOn = false;
                OffEffect.Post(AudioPlayer);
                
                GetComponent<Renderer>().material.color = Color.green;
            }
        }
    }
}
