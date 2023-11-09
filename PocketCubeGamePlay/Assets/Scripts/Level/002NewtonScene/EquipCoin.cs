using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipCoin : MonoBehaviour
{

    [SerializeField]
    Transform PlayerCoinHolder;

    //bool isCoinEquipped = false;

    private GameObject coinEquipped;

    public bool canEquip()
    {
        return coinEquipped==null;
    }

    public void Equip(GameObject coin)
    {
        if (canEquip())
        {

            // play equip coin animation

            coin.transform.position = PlayerCoinHolder.transform.position;
            coin.transform.rotation = PlayerCoinHolder.transform.rotation;
            coin.transform.parent   = PlayerCoinHolder.transform;
            coin.GetComponent<Rigidbody>().isKinematic = true;
            //isCoinEquipped = true;
            coinEquipped   = coin;
        }
    }

    public GameObject getEquipped()
    {
        return coinEquipped;
    }

    public void Drop()
    {
        if (coinEquipped!=null)
        {
            //isCoinEquipped = false;
            coinEquipped.transform.parent = null;
            coinEquipped = null;
        }
        
    }

}
