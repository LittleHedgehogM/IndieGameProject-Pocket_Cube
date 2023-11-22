using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DontDestroyObjects : MonoBehaviour
{
    public static DontDestroyObjects Instance;
    //[SerializeField] private Button btnSetting;
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

        }
        else
        {
            Destroy(gameObject);
        }
    }
}
