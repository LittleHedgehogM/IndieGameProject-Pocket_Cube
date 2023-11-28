using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level2AudioTrigger : MonoBehaviour
{
    [SerializeField] private AK.Wwise.Switch ELE_P2;
    [SerializeField] private GameObject audioPlayer;

    private void OnMouseDown()
    {
        ELE_P2.SetValue(audioPlayer);
    }
}
