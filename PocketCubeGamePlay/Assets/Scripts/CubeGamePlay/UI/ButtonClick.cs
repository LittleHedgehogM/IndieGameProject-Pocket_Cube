using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonClick : MonoBehaviour
{
    
    [SerializeField] private string message;
    private Color normalColor = Color.white;
    private Color pressedColor = Color.grey;
    private Image buttonImage;

    public delegate void MyCallbackDelegate(string message);
    public static event MyCallbackDelegate OnMyCallback;

    private void Start()
    {
        buttonImage = GetComponent<Image>();
        buttonImage.color = normalColor;
    }

    private void Update()
    {

        Vector2 mousePosition = Input.mousePosition;
        Vector2 canvasMousePosition = RectTransformUtility.WorldToScreenPoint(null, mousePosition);
        RectTransform buttonRectTransform = GetComponent<RectTransform>();
        bool isPointerInsideButton = RectTransformUtility.RectangleContainsScreenPoint(buttonRectTransform, canvasMousePosition);

        if (isPointerInsideButton)
        {
            if (Input.GetMouseButtonUp(0))
            {
                buttonImage.color = normalColor;
                OnMyCallback?.Invoke(message);
            }
            else if (Input.GetMouseButton(0) || Input.GetMouseButtonDown(0))
            {
                //Audio
                AkSoundEngine.PostEvent("Play_cube_click", gameObject);
                //
                buttonImage.color = pressedColor;
            }
            
        }        
    }

}
