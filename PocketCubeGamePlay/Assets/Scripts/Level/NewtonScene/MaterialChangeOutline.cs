using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaterialChangeOutline : MonoBehaviour
{
    GameObject go;

    Material material;
    Material[] materials;
    [SerializeField][Range(0, 0.2f)] float normalOutlineWidth;
    [SerializeField][Range(0, 0.2f)] float highlightOutlineWidth;
    [SerializeField] Color normalOutlineColor;
    [SerializeField] Color highlightOutlineColor;
    private bool enableMaterialChange = false;


    // Start is called before the first frame update
    void Start()
    {
        go = this.gameObject;
        //material = go.GetComponent<MeshRenderer>().material;
        materials = go.GetComponent<MeshRenderer>().materials;
        ResetMaterial();
    }

    public void SetEnableMaterialChange(bool enable)
    {
        enableMaterialChange = enable;
    }

    public void HighlightMaterial()
    {
        foreach (Material mat in materials)
        {
            mat.SetFloat("_OutlineWidth", highlightOutlineWidth);
            mat.SetColor("_Color0", highlightOutlineColor);
        }
        
    }

    public void ResetMaterial()
    {
        foreach(Material mat in materials)
        {
            mat.SetFloat("_OutlineWidth", normalOutlineWidth);
            mat.SetColor("_Color0", normalOutlineColor);
        }
        
    }


    private void OnMouseEnter()
    {
        if (enableMaterialChange)
        {
            HighlightMaterial();
        }
       
    }

    private void OnMouseExit()
    {
        ResetMaterial();
    }

}
