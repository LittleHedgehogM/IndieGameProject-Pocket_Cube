using UnityEngine;

public class FourierColorChanger : MonoBehaviour
{ 
    [SerializeField] private Color[] diffuseGradient01;
    [SerializeField] private Color[] diffuseGradient02;
    private int currentColorIndex = 0;
    private int targetColorIndex = 1;

    /*[SerializeField] private Color[] offDiffuseGradient01;
    [SerializeField] private Color[] offDiffuseGradient02;
    private int currentColorIndexOff = 0;
    private int targetColorIndexOff = 1;*/

    public float lerpTime;
    [SerializeField] private Color goalColor;
    //[SerializeField] bool isBridgeActive;

    
    private float targetPoint;
    private Material material;
    //public GameObject bridge;

    private bool levelEnter = false;
    public bool isLevelPass = false;
    private int levelFirstEnter = 0;
    private bool pushTransitions = false;

    /*[Header("Off: Color change on playerEnter | On: Color change on GameStart")]
    [SerializeField]*/
    //public int transitionOnStart;

    void Awake()
    {
        material = GetComponent<Renderer>().material;
        
        //bridge.SetActive(false);   
        
        
        
        
    }

    void Update()
    {

        /*switch (transitionOnStart)
        {
            case 0:
                //不在平台内颜色才变化
                if (GetBeats.pushTransitions & !levelEnter & !isLevelPass)
                {
                    //print("transition On");
                    ColorTransitionLevel();
                    //print(material.GetColor("_diffusegradient01"));
                }

                //颜色检测
                if (levelEnter & material.GetColor("_diffusegradient01") == goalColor & levelFirstEnter == 1 & !isLevelPass)
                {
                    isLevelPass = true;
                    bridge.SetActive(true);
                    print(this.gameObject.name + "pass");
                }
                break;

            case 1:
                //进入平台颜色才变化
                if (GetBeats.pushTransitions & levelFirstEnter == 1 & !isLevelPass)
                {
                    //print("transition On");
                    ColorTransitionLevel();
                    //print(material.GetColor("_diffusegradient01"));
                }

                //颜色检测
                if (!levelEnter & material.GetColor("_diffusegradient01") == goalColor & levelFirstEnter == 1 & !isLevelPass)
                {
                    isLevelPass = true;
                    bridge.SetActive(true);
                    print(this.gameObject.name + "pass");
                }
                break;
            case 2:
                if(GetBeats.pushTransitions & !levelEnter & !isLevelPass)
                {
                    OffColorTransition();
                }
                else if (GetBeats.pushTransitions & levelEnter & !isLevelPass)
                {
                    //print("transition On");
                    ColorTransitionLevel();
                    //print(material.GetColor("_diffusegradient01"));
                }

                //颜色检测
                if (!levelEnter & material.GetColor("_diffusegradient01") == goalColor & levelFirstEnter == 1 & !isLevelPass)
                {
                    isLevelPass = true;
                    bridge.SetActive(true);
                    print(this.gameObject.name + "pass");
                }
                break;
        }*/

        if (pushTransitions & !levelEnter & !isLevelPass)
        {
            //print("transition On");
            ColorTransitionLevel();
            //print(material.GetColor("_diffusegradient01"));
        }

        //颜色检测
        if (levelEnter & material.GetColor("_diffusegradient01") == goalColor & levelFirstEnter == 1 & !isLevelPass)
        {
            isLevelPass = true;
            
            print(this.gameObject.name + "pass");
        }



        //Bridge controller

    }

    //for wwise callback
    

    //Color Transitions
    public void ColorTransitionLevel()
    {          
        //
        targetPoint += Time.deltaTime / lerpTime;
        material.SetColor("_diffusegradient01", Color.Lerp(diffuseGradient01[currentColorIndex], diffuseGradient01[targetColorIndex], targetPoint));
        material.SetColor("_diffusegradient02", Color.Lerp(diffuseGradient02[currentColorIndex], diffuseGradient02[targetColorIndex], targetPoint));
        if (targetPoint >= 1f)
        {
            //print(targetPoint);
            targetPoint = 0f;
            currentColorIndex = targetColorIndex;
            //print(currentColorIndex);

            targetColorIndex ++;
            //(targetColorIndex);

            if (targetColorIndex == diffuseGradient01.Length)
            {
                targetColorIndex = 0;
                //print("re");
            }

            pushTransitions = false;
            //print("Transition over");
        }
            
        //print("Level1 Transition Stop" + currentColorIndex);
    }

    /*void OffColorTransition()//未激活时颜色变化
    {
        targetPoint += Time.deltaTime / lerpTime;
        material.SetColor("_diffusegradient01", Color.Lerp(offDiffuseGradient01[currentColorIndexOff], offDiffuseGradient01[targetColorIndexOff], targetPoint));
        material.SetColor("_diffusegradient02", Color.Lerp(offDiffuseGradient02[currentColorIndexOff], offDiffuseGradient02[targetColorIndexOff], targetPoint));

        if (targetPoint >= 1f)
        {
            targetPoint = 0f;
            if(targetColorIndexOff == 1)
            {
                currentColorIndexOff = 1;
                targetColorIndexOff = 0;
            }
            else if (targetColorIndexOff == 0)
            {
                currentColorIndexOff = 0;
                targetColorIndexOff = 1;
            }
            GetBeats.pushTransitions = false;
            //print(currentColorIndex);
        }
    }*/

    private void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.CompareTag("Player"))
        {
            
            levelEnter = true;
            print("Player Enter" + gameObject.name);
            if ( levelFirstEnter == 0)
            {
                levelFirstEnter++;
            }                 
        }
    }
    private void OnCollisionExit(Collision col)
    {
        if (col.gameObject.CompareTag("Player"))
        {
            levelEnter = false;
            print("Player Exit" + gameObject.name);
            if (levelFirstEnter == 0)
            {
                levelFirstEnter++;
            }
        }
    }

    public void PushTransitions()
    {
        pushTransitions = true;

        //print("transition On");
    }






}
