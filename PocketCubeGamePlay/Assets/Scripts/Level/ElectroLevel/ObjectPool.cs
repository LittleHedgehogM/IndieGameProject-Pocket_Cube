using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    [SerializeField] private GameObject vfx;
    [SerializeField] private int poolSize; 
    private List<GameObject> objectPool = new List<GameObject>();

    void Start()
    {
        for (int i = 0; i < poolSize; i++)
        {
            GameObject obj = Instantiate(vfx);
            obj.SetActive(false);
            objectPool.Add(obj);
        }
    }

    public delegate void ObjectCallback();

    public GameObject GetObjectFromPoolWithCallback(ObjectCallback callback)
    {
        GameObject obj = GetObjectFromPool();

        if (obj != null)
        {
            StartCoroutine(WaitForEffectAndCallback(obj, callback));
        }

        return obj;
    }

    private IEnumerator WaitForEffectAndCallback(GameObject obj, ObjectCallback callback)
    {
        ParticleSystem particleSystem = obj.GetComponent<ParticleSystem>();

        yield return new WaitWhile(() => particleSystem.isPlaying);

        callback.Invoke();

        ReturnObjectToPool(obj);
    }


    public GameObject GetObjectFromPool()
    {
        foreach (GameObject obj in objectPool)
        {
            if (!obj.activeInHierarchy)
            {
                obj.SetActive(true);
                return obj;
            }
        }
        return null;
    }

    public void ReturnObjectToPool(GameObject obj)
    {
        obj.SetActive(false);
    }
}