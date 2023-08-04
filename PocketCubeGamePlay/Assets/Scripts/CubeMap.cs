using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CubeMap : MonoBehaviour
{

    CubeState cubeState;

    public Transform up;
    public Transform down;
    public Transform left;        
    public Transform right;
    public Transform front;
    public Transform back;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void Set() 
    {
        cubeState = FindObjectOfType<CubeState>();
        UpdateMap(cubeState.front, front);
        UpdateMap(cubeState.back, back);
        UpdateMap(cubeState.left, left);
        UpdateMap(cubeState.right, right);
        UpdateMap(cubeState.up, up);
        UpdateMap(cubeState.down, down);
    }

    void UpdateMap(List<GameObject> face, Transform side) 
    {
        int i = 0;
        foreach (Transform map in side)
        {            
            if (face[i].name[0] == 'F')
            {
                map.GetComponent<Image>().color = new Color(0.31f, 0.71f, 0.89f, 1);// front color : blue             
            }
            else if (face[i].name[0] == 'B')
            {
                map.GetComponent<Image>().color = new Color(1f, 0.67f, 0.44f, 1); // back color : orange
            }
            else if (face[i].name[0] == 'L')
            {
                map.GetComponent<Image>().color = new Color(0.94f, 0.39f, 0.51f, 1);// left color : red
            }
            else if (face[i].name[0] == 'R')
            {
                map.GetComponent<Image>().color = new Color(1f, 0.93f, 0.45f, 1);// right color : yellow
            }
            else if (face[i].name[0] == 'U')
            {
                map.GetComponent<Image>().color = new Color(0.76f, 0.93f, 0.66f, 1);// up color: green
            }
            else if (face[i].name[0] == 'D')
            {
                map.GetComponent<Image>().color = new Color(0.73f,0.52f, 0.81f, 1);// down color: purple
            }
            i++;
            
        }
    }
}
