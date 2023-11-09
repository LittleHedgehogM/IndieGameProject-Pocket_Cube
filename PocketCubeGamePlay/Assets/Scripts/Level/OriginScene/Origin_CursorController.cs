using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Origin_CursorController : MonoBehaviour
{
    [SerializeField] Texture2D normalCursor;
    [SerializeField] Texture2D hoverAxisCursor;


    private void Start()
    {
        setNormalCursor();
    }
    public void setNormalCursor()
    {
        CursorMode mode = CursorMode.ForceSoftware;
        Cursor.SetCursor(normalCursor, Vector2.zero, mode); 
    }


    public void setHoverCursor()
    {
        CursorMode mode = CursorMode.ForceSoftware;
        Cursor.SetCursor(hoverAxisCursor, Vector2.zero, mode);
    }


    
}
