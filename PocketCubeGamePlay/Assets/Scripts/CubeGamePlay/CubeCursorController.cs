using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeCursorController : MonoBehaviour
{
    [SerializeField] private Texture2D normalTexture;
    [SerializeField] private Texture2D rotateTexture;
    [SerializeField] private Texture2D swipeTexture;
    [SerializeField] private Texture2D skillTexture;


    private void Start()
    {
        setNormalCursor();
    }

    public void setNormalCursor()
    {
        CursorMode mode = CursorMode.ForceSoftware;
        Cursor.SetCursor(normalTexture, Vector2.zero, mode);
    }

    public void setRotationCursor()
    {
        CursorMode mode = CursorMode.ForceSoftware;
        Cursor.SetCursor(rotateTexture, Vector2.zero, mode);
    }

    public void setSwipeCursor()
    {
        CursorMode mode = CursorMode.ForceSoftware;
        Cursor.SetCursor(swipeTexture, Vector2.zero, mode);
    }

    public void setSkillCursor(){
        CursorMode mode = CursorMode.ForceSoftware;
        Cursor.SetCursor(skillTexture, Vector2.zero, mode);
    }

}
