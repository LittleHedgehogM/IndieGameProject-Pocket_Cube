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

    [SerializeField] private Sprite NormalSprite;
    [SerializeField] private Sprite pressedSprite;
    [SerializeField] private Sprite hoverSprite;
    CubePlayUIController myUIController;
    CubePlayManager myManager;

    [System.Serializable]
    enum ButtonType
    {
        Skill,
        Reset
    }

    [SerializeField] private ButtonType buttonType;
    bool clicked = false;
    private void Start()
    {
        buttonImage = GetComponent<Image>();
        buttonImage.color = normalColor;
        myUIController = FindObjectOfType<CubePlayUIController>();
        clicked = false;

    }

    private void OnEnable()
    {
        CubePlayUIController.restoreCommutationButton += restoreCommutationButton;
        CubePlayUIController.restoreDiagonalButton    += restoreDiagonalButton;
    }

    private void OnDisable()
    {
        CubePlayUIController.restoreCommutationButton -= restoreCommutationButton;
        CubePlayUIController.restoreDiagonalButton -= restoreDiagonalButton;


    }

    private void restoreCommutationButton()
    {
        if (message.Contains("Commutation"))
        {
            buttonImage.sprite = NormalSprite;
            clicked = false;
        }
    }

    private void restoreDiagonalButton()
    {
        if (message.Contains("Diagonal"))
        {
            buttonImage.sprite = NormalSprite;
            clicked = false;    
        }
    }

    private bool isDiagonalButton()
    {
        return message.Contains("Diagonal");
    }

    private bool isCommutationButton()
    {
        return message.Contains("Commutation");
    }

    private void SkillButtonUpdate(bool isPointerInsideButton)
    {

        if (isPointerInsideButton)
        {
            if (Input.GetMouseButtonUp(0))
            {
                OnMyCallback?.Invoke(message);
                clicked = !clicked;

            }
            else if (Input.GetMouseButton(0) || Input.GetMouseButtonDown(0))
            {
                
            }
            else
            {
                if (isCommutationButton() && myUIController.CommutationCount == 0 /*&& !clicked*/)
                {
                    buttonImage.sprite = hoverSprite;
                }
                else if (isDiagonalButton() && myUIController.DiagonalCount == 0 /*&& !clicked*/)
                {
                    buttonImage.sprite = hoverSprite;
                }
            }

        }
        else
        {
            if((isCommutationButton() && myUIController.CommutationCount == 0 /* &&!clicked*/)
                    || (isDiagonalButton() && myUIController.DiagonalCount == 0 /* && !clicked*/))
            {
                    buttonImage.sprite = NormalSprite;
            }
        }

        if ( (isCommutationButton() && (myUIController.CommutationCount == 1 || clicked))
            || (isDiagonalButton() && (myUIController.DiagonalCount == 1 || clicked)))
        {
            buttonImage.sprite = pressedSprite;
            
        }
        
        //  recover checkpoint
        // if checkpoint is recovered

    }

    private void ResetButtonUpdate(bool isPointerInsideButton)
    {
        if (isPointerInsideButton)
        {
            if (Input.GetMouseButtonUp(0))
            {
                buttonImage.sprite = NormalSprite;
                OnMyCallback?.Invoke(message);
            }
            else if (Input.GetMouseButton(0) || Input.GetMouseButtonDown(0))
            {
                buttonImage.sprite = pressedSprite;
            }
            else if (!Input.GetMouseButton(1))
            {
                buttonImage.sprite = hoverSprite;

            }

        }
        else
        {
            buttonImage.sprite = NormalSprite;
        }
    }

    private void Update()
    {

        if (CubePlayManager.instance.isConfigurationPhase())
        {

            return;
        }
        Vector2 mousePosition = Input.mousePosition;
        Vector2 canvasMousePosition = RectTransformUtility.WorldToScreenPoint(null, mousePosition);
        RectTransform buttonRectTransform = GetComponent<RectTransform>();
        bool isPointerInsideButton = RectTransformUtility.RectangleContainsScreenPoint(buttonRectTransform, canvasMousePosition);
        switch (buttonType)
        {
            case ButtonType.Reset:{ ResetButtonUpdate(isPointerInsideButton); break; }
            case ButtonType.Skill:{ SkillButtonUpdate(isPointerInsideButton); break; }
            default: { break; }
        }

        //Audio Button Click
        if (isPointerInsideButton && Input.GetMouseButtonDown(0))
        {
            
            AkSoundEngine.PostEvent("Play_cube_click", gameObject);
            
        }
    }

}
