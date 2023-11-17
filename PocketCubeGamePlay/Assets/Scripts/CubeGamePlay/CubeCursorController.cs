using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeCursorController : MonoBehaviour
{
    [SerializeField] private Texture2D normalTexture;
    [SerializeField] private Texture2D rotateTexture;
    [SerializeField] private Texture2D swipeTexture;
    [SerializeField] private Texture2D skillTexture;
    Vector2 normalOffset;
    Vector2 rotateOffset;
    Vector2 swipeOffset;
    Vector2 skillOffset;
    CursorMode mode = CursorMode.ForceSoftware;

    private void Start()
    {
        normalOffset = new Vector2(normalTexture.width * 0.5f, normalTexture.height * 0.5f);
        rotateOffset = new Vector2(rotateTexture.width * 0.5f, rotateTexture.height * 0.5f);
        swipeOffset  = new Vector2(swipeTexture.width * 0.5f,  swipeTexture.height * 0.5f);
        skillOffset  = new Vector2(skillTexture.width * 0.5f,  skillTexture.height * 0.5f);
        setNormalCursor();
    }

    public void setNormalCursor()
    {
        Cursor.SetCursor(normalTexture, normalOffset, mode);
    }

    public void setRotationCursor()
    {
        Cursor.SetCursor(rotateTexture, rotateOffset, mode);
    }

    public void setSwipeCursor()
    {
        Cursor.SetCursor(swipeTexture, swipeOffset, mode);
    }

    public void setSkillCursor(){
        Cursor.SetCursor(skillTexture, skillOffset, mode);
    }

}
