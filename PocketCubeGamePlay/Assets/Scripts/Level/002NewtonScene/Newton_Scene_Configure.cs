using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class Newton_Scene_Configure : MonoBehaviour
{
    [System.Serializable]
    public enum CoinType
    {
        A,
        B,
        C,
        None
    }

    [System.Serializable]
    public class CoinTypeConfigure
    {
        public CoinType coinType;
        public int coinWeight;
    }

    public CoinTypeConfigure[] CoinConfigure;

    public CoinType[] Left_Scale;
    public CoinType[] Right_Scale;

    public const int size = 5;

    private void OnValidate()
    {
        if (Left_Scale.Length != size)
        {
            Array.Resize(ref Left_Scale, size);
        }

        if (Right_Scale.Length != size)
        {
            Array.Resize(ref Right_Scale, size);
        }
    }

}
