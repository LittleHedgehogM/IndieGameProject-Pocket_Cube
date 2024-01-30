using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class AddNewCoin : MonoBehaviour
{
    [SerializeField]
    private GameObject coinInitPosition;

    [SerializeField]
    private AK.Wwise.Event addCoin;
    private float range = 0.1f;

    public void AddCoin(GameObject coin, bool playSound)
    {
         Scale myScale = this.GetComponent<Scale>();

         if (!myScale.isFull())
         {
            Vector3 randomOffset = new Vector3(Random.Range(-range, range), 0, Random.Range(-range, range));
            coin.transform.rotation = coinInitPosition.transform.rotation;
            coin.transform.position = coinInitPosition.transform.position + randomOffset;
            coin.transform.parent = coinInitPosition.transform;

            coin.GetComponent<Rigidbody>().isKinematic = false;
            myScale.insertCoin(coin);

            if (playSound) 
            {
                addCoin.Post(coinInitPosition);
            }
            
        }
    }

  

}
