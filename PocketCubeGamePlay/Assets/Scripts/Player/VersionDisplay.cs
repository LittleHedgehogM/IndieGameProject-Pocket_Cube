using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VersionDisplay : MonoBehaviour
{
    [SerializeField] Text versionNum;

    private void Awake()
    {
        if (!(PlayerPrefs.GetString("Version") == null))
        {
            versionNum.text = PlayerPrefs.GetString("Version");
        }
        else
        {
            versionNum.text = "empty";
        }
    }

}
