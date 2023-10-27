using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Electro_MeshControl : MonoBehaviour
{
    Vector3 originScale = Vector3.one;

    private void Start()
    {

        originScale = this.transform.localScale;

    }
    public void Show() 
    {
        this.transform.localScale = originScale;
    }

    public void Hide()
    {
        this.transform.localScale = Vector3.zero;
    }

}
