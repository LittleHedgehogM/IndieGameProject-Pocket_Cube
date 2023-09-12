using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class FourierColorChanger : MonoBehaviour
{
    FourierPlayer player;
    /*[SerializeField]
    [Header("Level 1")]
    public GameObject prefab1;
    public Color[] myColors1;
    private int currentColorIndex1 = 0;
    private int targetColorIndex1 = 1;
    private float targetPoint1;
    public float lerpTime1 = 0.9f;
    Material material1;*/

    //Level 1 defination
    [System.Serializable]
    public class Level1 
    {
        public GameObject prefab;
        public Color[] myColors;
        public int currentColorIndex = 0;
        public int targetColorIndex = 1;
        public float targetPoint;
        public float lerpTime = 0.9f;
        public Material material;
        public Color goalColor;
        public GameObject bridge;
    }
    public Level1 level1;
    bool transitionLevel1;
    bool isLevel1Pass = false;
    

    //Level 2 defination
    [System.Serializable]
    public class Level2
    {
        public GameObject prefab;
        public Color[] myColors;
        public int currentColorIndex = 0;
        public int targetColorIndex = 1;
        public float targetPoint;
        public float lerpTime = 0.9f;
        public Material material;
        public Color goalColor;
        public GameObject bridge;
    }
    public Level2 level2;
    bool transitionLevel2;
    bool isLevel2Pass = false;
    bool pushTransitions = false;
    

    void Awake()
    {
        level1.material = level1.prefab.GetComponent<Renderer>().material;
        level1.bridge.SetActive(false);

        level2.material = level2.prefab.GetComponent<Renderer>().material;    
        level2.bridge.SetActive(false);
    }

    void Update()
    {
        transitionLevel1 = FourierPlayer.transitionLevel1;
        transitionLevel2 = FourierPlayer.transitionLevel2;

        if (pushTransitions & transitionLevel1 & !isLevel1Pass)
        {
            ColorTransitionLevel1();      
        }     

        if (pushTransitions & transitionLevel2 & !isLevel2Pass)
        {
            ColorTransitionLevel2();
        }


        //颜色检测 level1
        if (!transitionLevel1 & level1.material.color == level1.goalColor)
        {
            isLevel1Pass = true;         
            level1.bridge.SetActive(true);
        }


        if (!transitionLevel2 & level2.material.color == level2.goalColor)
        {
            isLevel2Pass = true;
            level2.bridge.SetActive(true);
        }
    }

    //for wwise callback
    void ColorTransitionLevel1()
    {    
            level1.targetPoint += Time.deltaTime / level1.lerpTime;
            level1.material.color = Color.Lerp(level1.myColors[level1.currentColorIndex], level1.myColors[level1.targetColorIndex], level1.targetPoint);

            if (level1.targetPoint >= 1f)
            {
                level1.targetPoint = 0f;
                level1.currentColorIndex = level1.targetColorIndex;
                level1.targetColorIndex++;
                if (level1.targetColorIndex == level1.myColors.Length)
                {
                    level1.targetColorIndex = 0;
                }
                pushTransitions = false;
                //print(level1.currentColorIndex);
            }                    
    }

    void ColorTransitionLevel2()
    {             
            level2.targetPoint += Time.deltaTime / level2.lerpTime;
            level2.material.color = Color.Lerp(level2.myColors[level2.currentColorIndex], level2.myColors[level2.targetColorIndex], level2.targetPoint);

            if (level2.targetPoint >= 1f)
            {
                level2.targetPoint = 0f;
                level2.currentColorIndex = level2.targetColorIndex;
                level2.targetColorIndex++;
                if (level2.targetColorIndex == level2.myColors.Length)
                {
                    level2.targetColorIndex = 0;
                }
                pushTransitions = false;
                //print(level2.currentColorIndex);
            }       
    }


    public void PushTransitions()
    {
        pushTransitions = true;
    }

    
}
