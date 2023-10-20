using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FourierBridgeController : MonoBehaviour
{
    //Level 1
    [SerializeField] GameObject bridge_1;
    [SerializeField] FourierColorChanger level_1;

    [SerializeField] GameObject bridge_2;
    [SerializeField] FourierColorChanger level_2;
    [SerializeField] FourierColorChanger level_3;

    //[Header("Level Obstacle")]
    //[SerializeField] GameObject obstacle_1;
    //[SerializeField] GameObject obstacle_2;
    //[SerializeField] GameObject obstacle_3;

    
    void Start()
    {
        bridge_1.SetActive(false);
        bridge_2.SetActive(false);

        
    }

    // Update is called once per frame
    void Update()
    {
        if (level_1.isLevelPass && !bridge_1.activeSelf)
        {
            bridge_1.SetActive(true);
            //obstacle_1.SetActive(false);
        }

        if (level_2.isLevelPass && level_3.isLevelPass && !bridge_2.activeSelf)
        {
            bridge_2.SetActive(true);
            //obstacle_2.SetActive(false);
        }

        
    }


}
