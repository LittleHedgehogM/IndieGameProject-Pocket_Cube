using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorGradient : MonoBehaviour
{
	[SerializeField] private Color[] diffuseGradient01;
    [SerializeField] private Color[] diffuseGradient02;
    private Material material;
    // private float speed = 0.01f;

    private Color customColor_01 = new Color(1f, 0.2f, 0.2f, 1.0f);
    private Color customColor_02 = new Color32(127, 242, 44, 255);
    private Color customColor_03 = new Color32(255, 255, 255, 255);
    private Color customColor_04 = new Color32(1, 1, 1, 1);

    private Color originalColor;
    private Color aimColor;

    public float lerpTime = 0.9f;

    float t = 0;
    float time_Light = 0;
    float time_Dark = 0;
    // float ChangeTimeLength = 0.75f;  //更改颜色的时间长度/时间间隔
    float ChangeTimeLength_Light = 0.4f;
    float ChangeTimeLength_Dark = 1.5f;
    float ChangeTimeLength; //= ChangeTimeLength_Light + ChangeTimeLength_Dark;
    private int targetColorIndex = 0;
    private int targetColorIndex_Light = 0;
    private int targetColorIndex_Dark = 0;


    // Start is called before the first frame update
    void Start()
    {
    	material = GetComponent<Renderer>().material;
        Debug.Log(material.GetColor("_diffusegradient01"));
        Debug.Log(material.GetColor("_diffusegradient02"));
        originalColor = material.GetColor("_diffusegradient01");
        aimColor = material.GetColor("_diffusegradient02");
        // material.SetColor("_diffusegradient01", customColor);
        // Debug.Log(material.GetColor("_diffusegradient01"));
        // material.GetColor("_diffusegradient01");
        // material.SetColor("_diffusegradient01", customColor_03);
        // material.SetColor("_diffusegradient02", customColor_04);
    ChangeTimeLength = ChangeTimeLength_Light + ChangeTimeLength_Dark;

    }

    // Update is called once per frame
    void Update()
    {
        t += Time.deltaTime;
        time_Light += Time.deltaTime;
        time_Dark  += Time.deltaTime;

    	// if(Input.GetKeyDown("space")){
    	// 	GradientColorLerp();
    	// 	targetColorIndex++;
    	// }
/*
        if (t < ChangeTimeLength){
    		// GradientColorLerpBoth();
    		// GradientColorLerpLightPart();
    		GradientColorLerpAsynch();
        }else{
        	t = 0;
        	targetColorIndex++;
        }*/

        GradientColorLerpConcentric();

    	if(Input.GetKeyDown("space")){
			targetColorIndex = 0;
			targetColorIndex_Light = 0;
			targetColorIndex_Dark = 0;
    	}


        // material.SetColor("_diffusegradient01", Color.Lerp(originalColor, customColor_01, speed));
/*        if (t < ChangeTimeLength)
        {
        	material.SetColor("_diffusegradient01", Color.Lerp(originalColor, customColor_03, Time.deltaTime*10));
        	material.SetColor("_diffusegradient02", Color.Lerp(aimColor, customColor_04, Time.deltaTime*10));
            // Color c = GetComponent<MeshRenderer>().material.color;
            // GetComponent<MeshRenderer>().material.color = Color.Lerp(c, new Color(r, g, b, 1f), Time.deltaTime*10);           
        }
        else if (t >= ChangeTimeLength)
        {
        Debug.Log(material.GetColor("_diffusegradient01"));
        }*/

    }
    void GradientColorLerpBoth(){
        material.SetColor("_diffusegradient01", Color.Lerp(material.GetColor("_diffusegradient01"), diffuseGradient01[targetColorIndex], Time.deltaTime*10));
        material.SetColor("_diffusegradient02", Color.Lerp(material.GetColor("_diffusegradient02"), diffuseGradient02[targetColorIndex], Time.deltaTime*10));
    }

    void GradientColorLerpLightPart(){
        material.SetColor("_diffusegradient01", Color.Lerp(material.GetColor("_diffusegradient01"), diffuseGradient01[targetColorIndex_Light], Time.deltaTime/t));
    }
    void GradientColorLerpDarkPart(){
        material.SetColor("_diffusegradient02", Color.Lerp(material.GetColor("_diffusegradient02"), diffuseGradient02[targetColorIndex_Dark], Time.deltaTime/t));
    }
    //boundary check for targetColorIndex

    void GradientColorLerpAsynch(){
    	if (t < ChangeTimeLength_Light){
    		GradientColorLerpLightPart();
    	}else if ((ChangeTimeLength - t) < ChangeTimeLength_Dark){
    		GradientColorLerpDarkPart();
    	}
    }
    void GradientColorLerpConcentric(){
    	if (time_Light < ChangeTimeLength_Light){
    		GradientColorLerpLightPart();
    	}else{
    		time_Light = 0;
			targetColorIndex_Light++;
    	}

    	if(time_Dark < ChangeTimeLength_Dark){
    		GradientColorLerpDarkPart();
    	}else{
    		time_Dark = 0;
    		targetColorIndex_Dark++;
    	}
    }
}
