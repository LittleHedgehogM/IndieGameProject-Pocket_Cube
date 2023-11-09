using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Origin_CursorController : MonoBehaviour
{
    [SerializeField] Texture2D normalCursor;
    [SerializeField] Texture2D hoverAxisCursor;
    private Vector2 normalOffset;
    private Vector2 hoverOffset;
    private CursorMode mode;

    private void Start()
    {
        setOffset(ref normalOffset, ref normalCursor);
        setOffset(ref hoverOffset, ref hoverAxisCursor);
        mode = CursorMode.ForceSoftware;
        setNormalCursor();
    }

    private void setOffset(ref Vector2 offset, ref Texture2D texture)
    {
        offset = new Vector2(texture.width * 0.5f, texture.height * 0.5f);

    }

    public void setNormalCursor()
    {
        Cursor.SetCursor(normalCursor, Vector2.zero, mode); 
    }


    public void setHoverCursor()
    {
        Cursor.SetCursor(hoverAxisCursor, Vector2.zero, mode);
    }


    
}
