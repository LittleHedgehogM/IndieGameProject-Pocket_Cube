using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorScheme : MonoBehaviour
{
	float t = 0;
    float r, g, b;
    float ChangeTimeLength = 1.5f;  //更改颜色的时间长度/时间间隔
    // Start is called before the first frame update
    void Start()
    {
        r = Random.Range(0f, 1f);
        g = Random.Range(0f, 1f);
        b = Random.Range(0f, 1f);
    }

    // Update is called once per frame
    void Update()
    {
        t += Time.deltaTime;
        if (t < ChangeTimeLength)
        {
            Color c = GetComponent<MeshRenderer>().material.color;
            GetComponent<MeshRenderer>().material.color = Color.Lerp(c, new Color(r, g, b, 1f), Time.deltaTime*10);           
        }
        else if (t >= ChangeTimeLength)
        {
            t = 0;
            r = Random.Range(0f, 1f);//随机颜色
            g = Random.Range(0f, 1f);//随机颜色
            b = Random.Range(0f, 1f);//随机颜色
        }        
    }
}
