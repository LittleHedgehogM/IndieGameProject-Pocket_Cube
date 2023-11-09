using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectDistance : MonoBehaviour
{

    [SerializeField] private GameObject Eye_L;
    [SerializeField] private GameObject Eye_R;
    [SerializeField] private GameObject player;

    public float getDistanceBetweenPlayerAndLeftEye()
    {
        return GetDistanceBetweenPlayerAndEye(Eye_L);
    }

    public float getDistanceBetweenPlayerAndRightEye()
    {
        return GetDistanceBetweenPlayerAndEye(Eye_R);
    }

    private float GetDistanceBetweenPlayerAndEye(GameObject Eye)
    {
        Vector3 eyePosition = Eye.transform.position;
        Vector3 playerPosition = player.transform.position;
        float distance = Vector3.Distance(eyePosition, playerPosition); 
        return distance;
    }

}
