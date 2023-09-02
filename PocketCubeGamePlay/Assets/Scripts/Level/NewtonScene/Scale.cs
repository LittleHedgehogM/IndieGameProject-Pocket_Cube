using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class Scale : MonoBehaviour
{

    [SerializeField]
    private float heightGap;

    private int totalWeight;

    private int maxNumOfCoins = 5;

    List<GameObject> coinsOnScale = new List<GameObject>();
    [SerializeField] float translationTime;

    float initY;

    private int previousWeight;

    public bool isEmpty()
    {
        return coinsOnScale.Count == 0;
    }

    public bool isFull()
    {
        return coinsOnScale.Count == maxNumOfCoins;
    }
   
    public int getTotalWeight()
    {
        return totalWeight;
    }


    public void InitScalePosition()
    {
        int weightGap = 5 - totalWeight;
        Vector3 startPosition = transform.position;
        initY = startPosition.y;
        transform.position = new Vector3(startPosition.x, startPosition.y + weightGap * heightGap, startPosition.z);
         
    }

    public IEnumerator UpdatePosition()
    {

        int weightGap = 5 - totalWeight;

        float currentUsedTime = 0;
        float t = 0;
        Vector3 startPosition = transform.position;
        Vector3 targetPosition = new Vector3(startPosition.x, initY + weightGap*heightGap, startPosition.z);

        while (t < 1)
        {
            currentUsedTime += Time.deltaTime;
            t = currentUsedTime / translationTime;
            transform.position = Vector3.Lerp(startPosition, targetPosition, t);
            yield return null;
        }
    }

    private void updateWeight()
    {
        previousWeight = totalWeight;
        totalWeight = 0;
        foreach (var coin in coinsOnScale)
        {
            int coin_weight = coin.GetComponent<CoinWeight>().getWeight();
            totalWeight += coin_weight;
        }
        
    }

    public void insertCoin(GameObject coin)
    {
        coinsOnScale.Add(coin);
        coin.transform.parent = transform;
        updateWeight();
    }

    public void popCoin(GameObject coinToPop)
    {
        if (coinsOnScale.Contains(coinToPop))
        {
            coinsOnScale.Remove(coinToPop);
            coinToPop.transform.parent = null;

        }
        updateWeight();
    }

}
