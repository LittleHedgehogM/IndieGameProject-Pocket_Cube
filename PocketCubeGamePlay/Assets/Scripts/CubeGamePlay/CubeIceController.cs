using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeIceController : MonoBehaviour
{

    [SerializeField] GameObject FrontLeftUpIce;
    [SerializeField] GameObject FrontLeftDownIce;
    [SerializeField] GameObject FrontRightUpIce;
    [SerializeField] GameObject FrontRightDownIce;
    [SerializeField] GameObject BackLeftUpIce;
    [SerializeField] GameObject BackLeftDownIce;
    [SerializeField] GameObject BackRightUpIce;
    [SerializeField] GameObject BackRightDownIce;
    [SerializeField][Range(0f, 1f)] float lerpTime = 0.5f;

    private Material FLUIcematerial;
    private Material FLDIcematerial;
    private Material FRUIcematerial;
    private Material FRDIcematerial;
    private Material BLUIcematerial;
    private Material BLDIcematerial;
    private Material BRUIcematerial;
    private Material BRDIcematerial;


    float alphaVal;
    float r = 255;
    float g = 255;
    float b = 255;

    private void Start()
    {
        FLUIcematerial = FrontLeftUpIce.GetComponent<MeshRenderer>().material;
        FLDIcematerial = FrontLeftDownIce.GetComponent<MeshRenderer>().material;
        FRUIcematerial = FrontRightUpIce.GetComponent<MeshRenderer>().material;
        FRDIcematerial = FrontRightDownIce.GetComponent<MeshRenderer>().material;
        BLUIcematerial = BackLeftUpIce.GetComponent<MeshRenderer>().material;
        BLDIcematerial = BackLeftDownIce.GetComponent<MeshRenderer>().material;
        BRUIcematerial = BackRightUpIce.GetComponent<MeshRenderer>().material;
        BRDIcematerial = BackRightDownIce.GetComponent<MeshRenderer>().material;
        r = FLUIcematerial.GetColor("_Basecolor").r;
        g = FLUIcematerial.GetColor("_Basecolor").g;
        b = FLDIcematerial.GetColor("_Basecolor").b;
        alphaVal = FLUIcematerial.GetColor("_Basecolor").a;
    }


    public IEnumerator HideIceAnim()
    {
        float currentUsedTime = 0;
        float t = 0;

        while(t<1)        
        {
            currentUsedTime += Time.deltaTime;
            t = currentUsedTime / lerpTime;
            float currentAlpha = Mathf.Lerp(alphaVal, 0, t);
            Color currentColor = new Color(r, g, b, currentAlpha);
            FLUIcematerial.SetColor("_Basecolor", currentColor);
            FLDIcematerial.SetColor("_Basecolor", currentColor);
            FRUIcematerial.SetColor("_Basecolor", currentColor);
            FRDIcematerial.SetColor("_Basecolor", currentColor);
            BLUIcematerial.SetColor("_Basecolor", currentColor);
            BLDIcematerial.SetColor("_Basecolor", currentColor);
            BRUIcematerial.SetColor("_Basecolor", currentColor);
            BRDIcematerial.SetColor("_Basecolor", currentColor);
            yield return null;
        }
           
        yield return null;

    }

    public void HideIce()
    {
        FLUIcematerial.SetColor("_Basecolor", new Color(255, 255, 255, 0));
        FLDIcematerial.SetColor("_Basecolor", new Color(255, 255, 255, 0));
        FRUIcematerial.SetColor("_Basecolor", new Color(255, 255, 255, 0));
        FRDIcematerial.SetColor("_Basecolor", new Color(255, 255, 255, 0));
        BLUIcematerial.SetColor("_Basecolor", new Color(255, 255, 255, 0));
        BLDIcematerial.SetColor("_Basecolor", new Color(255, 255, 255, 0));
        BRUIcematerial.SetColor("_Basecolor", new Color(255, 255, 255, 0));
        BRDIcematerial.SetColor("_Basecolor", new Color(255, 255, 255, 0));

    }

}
