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
    private float range = 0.02f;

    public void AddCoin(GameObject coin)
    {
         Scale myScale = this.GetComponent<Scale>();

         if (!myScale.isFull())
         {
            Vector3 randomOffset = new Vector3(Random.Range(-range, range), 0, Random.Range(-range, range));
            coin.transform.rotation = coinInitPosition.transform.rotation;
            coin.transform.parent = coinInitPosition.transform;
            coin.transform.localPosition = Vector3.zero;
            coin.transform.localPosition += randomOffset;

            coin.GetComponent<Rigidbody>().isKinematic = false;
            myScale.insertCoin(coin);

            addCoin.Post(coinInitPosition);
            //print("Total weight = " + myScale.getTotalWeight());
        }
    }

  

}
