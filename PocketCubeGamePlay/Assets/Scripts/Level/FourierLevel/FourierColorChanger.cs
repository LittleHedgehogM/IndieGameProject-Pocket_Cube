using UnityEngine;

public class FourierColorChanger : MonoBehaviour
{
    private bool pushTransitions = false;
    
    
    [SerializeField] private Color[] diffuseGradient01;
    [SerializeField] private Color[] diffuseGradient02;
    [SerializeField] private float lerpTime = 0.9f;
    [SerializeField] private Color goalColor;
    [SerializeField] bool isBridgeActive;

    private int currentColorIndex = 0;
    private int targetColorIndex = 1;
    private float targetPoint;
    private Material material;
    public GameObject bridge;

    private bool levelEnter = false;
    private bool isLevelPass = false;
    private int levelFirstEnter = 0;

    /*[Header("Off: Color change on playerEnter | On: Color change on GameStart")]
    [SerializeField]*/
    public static bool transitionOnStart = false;
   
    void Awake()
    {
        material = GetComponent<Renderer>().material;
        
        bridge.SetActive(false);       
    }

    void Update()
    {
        if(transitionOnStart)
        {
            //不在平台内颜色才变化
            if (pushTransitions & !levelEnter & !isLevelPass)
            {
                //print("transition On");
                ColorTransitionLevel();
                //print(material.GetColor("_diffusegradient01"));
            }

            //颜色检测
            if (levelEnter & material.GetColor("_diffusegradient01").ToString("2f") == goalColor.ToString("2f") & levelFirstEnter == 1 & !isLevelPass)
            {
                isLevelPass = true;
                bridge.SetActive(true);
                print(this.gameObject.name + "pass");
            }
        }
        else if (!transitionOnStart)
        {
            //进入平台颜色才变化
            if (pushTransitions & levelEnter & !isLevelPass)
            {
                //print("transition On");
                ColorTransitionLevel();
                //print(material.GetColor("_diffusegradient01"));
            }

            //颜色检测
            if (!levelEnter & material.GetColor("_diffusegradient01")== goalColor & levelFirstEnter == 1 & !isLevelPass)
            {
                isLevelPass = true;
                bridge.SetActive(true);
                print(this.gameObject.name + "pass");
            }
        }

        



        //Bridge controller
        
    }

    //for wwise callback
    public void PushTransitions()
    {
        pushTransitions = true;
        //print("transition On");
    }

    //Color Transitions
    void ColorTransitionLevel()
    {
            
        //print("Level Transition Start");
            targetPoint += Time.deltaTime / lerpTime;
            material.SetColor("_diffusegradient01", Color.Lerp(diffuseGradient01[currentColorIndex], diffuseGradient01[targetColorIndex], targetPoint));
            material.SetColor("_diffusegradient02", Color.Lerp(diffuseGradient02[currentColorIndex], diffuseGradient02[targetColorIndex], targetPoint));

        if (targetPoint >= 1f)
            {
            targetPoint = 0f;
            currentColorIndex = targetColorIndex;
            targetColorIndex++;
            if (targetColorIndex == diffuseGradient01.Length)
                {
                    targetColorIndex = 0;                
                }
                pushTransitions = false;
                //print(currentColorIndex);
            }
        //print("Level1 Transition Stop" + currentColorIndex);
    }

    private void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.CompareTag("Player"))
        {
            levelEnter = true;
            print("Player Enter");
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
            print("Player Exit");
            if (levelFirstEnter == 0)
            {
                levelFirstEnter++;
            }
        }
    }

    





}
