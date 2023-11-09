using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Newton_CursorController : MonoBehaviour
{

    [SerializeField] Texture2D defaultCursor;
    [SerializeField] Texture2D viewCursor;
    [SerializeField] Texture2D selectCursor;
    [SerializeField] Texture2D clickDownCursor;

    private Vector2 defaultOffset;
    private Vector2 viewOffset;
    private Vector2 selectCursorOffset;
    private Vector2 clickDownCursorOffset;

    private CursorMode mode; 

    private void Start()
    {
        setOffset(ref defaultOffset, ref defaultCursor);
        setOffset(ref viewOffset, ref viewCursor);
        setOffset(ref selectCursorOffset, ref selectCursor);
        setOffset(ref clickDownCursorOffset, ref clickDownCursor);
        mode = CursorMode.ForceSoftware;
        setDefaultCursor();
    }

    private void setOffset(ref Vector2 offset, ref Texture2D texture)
    {
        offset = new Vector2(texture.width*0.5f, texture.height * 0.5f);

    }

    public void setDefaultCursor()
    {
        Cursor.SetCursor(defaultCursor, defaultOffset, mode);
    }

    public void setViewCursor()
    {
        Cursor.SetCursor(viewCursor, viewOffset, mode);
    }

    public void setSelectCursor()
    {
        Cursor.SetCursor(selectCursor, selectCursorOffset, mode);
    }

    public void setClickDownCursor()
    {
        Cursor.SetCursor(clickDownCursor, clickDownCursorOffset, mode);
    }

}
