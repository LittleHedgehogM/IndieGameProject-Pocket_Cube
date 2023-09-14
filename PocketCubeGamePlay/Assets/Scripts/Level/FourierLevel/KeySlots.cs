using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeySlots : MonoBehaviour
{
    List<Transform> children;
    private int callIndex = 0;
    void Awake()
    {
        children = GetChildren(transform);

        

        /*foreach (Transform child in children)
        {
            Renderer mat = child.GetComponent<Renderer>();
            mat.material.color = Color.red;
            
        }*/
    }

    List<Transform> GetChildren(Transform parent)
    {
        List<Transform> children = new List<Transform>();

        foreach(Transform child in parent)
        {
            children.Add(child);
        }

        return children;
    }



    public void callKeySlots()
    {
        if (callIndex < children.Count)
        {
            children[callIndex].GetComponent<Renderer>().material.color = Color.red;
            callIndex++;

            

        }
        else if(callIndex == children.Count) 
        {
            callIndex = 0;
            foreach (Transform child in children)
            {
                Renderer mat = child.GetComponent<Renderer>();
                mat.material.color = Color.white;

            }
        }
    }
}
