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

    public void AddCoin(GameObject coin)
    {
         Scale myScale = this.GetComponent<Scale>();

         if (!myScale.isFull())
         {
            Vector3 randomOffset = new Vector3(Random.Range(-0.1f, 0.1f), 0, Random.Range(-0.1f, 0.1f));
            //GameObject coin = Instantiate(coinToAdd, 
            //                              coinInitPosition.transform.position+randomOffset, 
            //                              coinToAdd.transform.rotation);

            coin.transform.position = coinInitPosition.transform.position + randomOffset;
            coin.transform.rotation = coinInitPosition.transform.rotation;            
            coin.GetComponent<Rigidbody>().isKinematic = false;

            

            myScale.insertCoin(coin);

            addCoin.Post(coinInitPosition);


            //print("Total weight = " + myScale.getTotalWeight());
        }
    }

  

}
