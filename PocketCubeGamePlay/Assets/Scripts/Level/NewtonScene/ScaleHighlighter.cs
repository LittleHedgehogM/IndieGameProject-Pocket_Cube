using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScaleHighlighter : MonoBehaviour
{
    [SerializeField] 
    private GameObject highlighter;

    private void Start()
    {
        disableHighlight();
    }
    public void HighlightScale()
    {
        if (highlighter != null)
        {
            highlighter.SetActive(true);
        }
    }

    public void disableHighlight()
    {
        if (highlighter != null)
        {
            highlighter.SetActive(false);
        }
            
    }

}
