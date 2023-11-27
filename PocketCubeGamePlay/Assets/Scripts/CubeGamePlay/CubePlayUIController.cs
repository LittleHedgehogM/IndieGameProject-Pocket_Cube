using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using static System.TimeZoneInfo;
using Button = UnityEngine.UI.Button;
//using static CubePlayManager;

public class CubePlayUIController : MonoBehaviour
{
    [Header("Text Settings")]
    [SerializeField] private TextMeshProUGUI mySwipeCountText;
    [SerializeField] private TextMeshProUGUI myDiagonalCountText;
    [SerializeField] private TextMeshProUGUI myCommutationCountText;
    [SerializeField] private TextMeshProUGUI myIsCubeSolvedText;
    [SerializeField] private TextMeshProUGUI myTipsText;
    
    [Header("Button Settings")]
    public Button DiagonalButton;
    public Button CommutationButton;
    public Button RestartButton;
    public Button FinishButton;
    public Button ResetCameraButton;


    [Header("Skill Settings")]
    [SerializeField] private bool isCommutationUnlocked;
    [SerializeField] private bool isDiagonalUnlocked;

    [SerializeField] private int maxCommutation;
    [SerializeField] private int maxDiagonal;
    [SerializeField] private int suggestedStepCount;

    [Header("Tutorial Settings")]
    [SerializeField] private bool isShowTutorial;
    [SerializeField] private UnityEngine.UI.Image TutorialImage;
    [SerializeField][Range(0,20)] private int tutorialDuration;

    [Header("Finish Settings")]
    [SerializeField] private UnityEngine.UI.Image finishImage;
    [SerializeField][Range(0, 5)] private int pre_finishDuration;
    [SerializeField][Range(0, 5)] private int display_finishDuration;
    private float finishAlpha;

    private int totalSteps;
    private bool isCommutationApplied;
    private bool isDiagonalApplied;

    public static event Action onEnterDiagonalState;
    public static event Action onRestoreDiagonalCheckPoint;

    public static event Action onEnterCommutationState;
    public static event Action onRestoreCommutationCheckPoint;



    private int _swipeCount;
    public int SwipeCount
    {
        get { return _swipeCount; }
        set 
        { 
            _swipeCount = value;           
            UpdateTotalStepsCountText();
        }
    }


    private int _diagonalCount;
    public int DiagonalCount
    {
        get { return _diagonalCount; }
        set
        {
            _diagonalCount = value;
            UpdateTotalStepsCountText();
        }
    }
    private int _commutationCount;
    public int CommutationCount
    {
        get { return _commutationCount; }
        set 
        {
            _commutationCount = value;
            UpdateTotalStepsCountText();
        }
    }

    private bool _solveResult;
    public bool SolveResult
    {
        get { return _solveResult; }
        set
        {
            _solveResult = value;
            UpdateSolveStatusText(_solveResult);
        }
    }

    public int getCurrentSwipeSteps()
    {
        return SwipeCount;
    }

    public bool getIsCommutationApplied() 
    {
        return isCommutationApplied;
    }

    public bool getIsDiagonalApplied()
    {
        return isDiagonalApplied;
    }

    public void restoreTotalStepsDiagonal(int restoreSteps, bool commutationApplied)
    {
        SwipeCount = restoreSteps;
        DiagonalCount = 0;
        isCommutationApplied = commutationApplied;
        CommutationCount = commutationApplied ? 1 : 0;
        if (commutationApplied) {
            CommutationButton.image.color = Color.grey;
        }
        else 
        {
            CommutationButton.image.color = Color.white;
        }
    }

    public void restoreTotalStepsCommutation(int restoreSteps, bool diagonalApplied)
    {
        SwipeCount = restoreSteps;
        CommutationCount = 0;
        isDiagonalApplied = diagonalApplied;
        DiagonalCount = diagonalApplied ? 1 : 0;
        if (diagonalApplied) 
        {
            DiagonalButton.image.color = Color.grey;
        }
        else
        {
            DiagonalButton.image.color = Color.white;

        }
    }

    public void onRestart()
    {
        InitCubePlayUIElements();
    }

    public void InitCubePlayUIElements()
    {
        // disable Skill if they are locked
        // hide restart and finish button
        DiagonalButton.gameObject.SetActive(isDiagonalUnlocked);
        CommutationButton.gameObject.SetActive(isCommutationUnlocked);
        TutorialImage.gameObject.SetActive(isShowTutorial);

        FinishButton.gameObject.SetActive(false);
        RestartButton.gameObject.SetActive(true);

        isCommutationApplied = false;
        isDiagonalApplied = false;

        CommutationButton.image.color = Color.white;
        DiagonalButton.image.color = Color.white;

        SwipeCount = 0;
        CommutationCount = 0;
        DiagonalCount = 0;
        SolveResult = false;
        totalSteps = 0;
        SetTutorialInvisible();
        SetFinishImageInvisible();

    }

    private void OnEnable()
    {
        CubeInPlayPhase.onCubeSolved += CubeSolved;
        SwipeFaceManager.onSwipeFinished += onSwipeFinished;
        DiagonalSkill.onDiagonalFinished += onDiagonalFinished;
        CommutationSkill.onCommutataionFinished += onCommutationFinished;
    }

    private void OnDisable()
    {
        CubeInPlayPhase.onCubeSolved -= CubeSolved;
        SwipeFaceManager.onSwipeFinished += onSwipeFinished;
        DiagonalSkill.onDiagonalFinished += onDiagonalFinished;
        CommutationSkill.onCommutataionFinished += onCommutationFinished;
    }

    
    public void clickDiagonalButton()
    {
       if (!isDiagonalApplied && CubePlayManager.instance.CanUseSkill())
       {
            onEnterDiagonalState?.Invoke();
            DiagonalButton.image.color = Color.grey;
            isDiagonalApplied = true;

        }
       else if (isDiagonalApplied && CubePlayManager.instance.CanRestoreDiagonalSkill())
       {
            onRestoreDiagonalCheckPoint?.Invoke();
            DiagonalButton.image.color = Color.white;
            isDiagonalApplied = false;
            DiagonalCount = 0;

        }
        else if (isDiagonalApplied && !CubePlayManager.instance.CanRestoreDiagonalSkill())
        {
            DiagonalButton.image.color = Color.grey;
        }
    }

    public void clickFinishButton()
    {
        // load another scene....
    }

    public void clickCommutationButton()
    {
        //isCommutationApplied = !isCommutationApplied;
        if (!isCommutationApplied && CubePlayManager.instance.CanUseSkill())
        {
            onEnterCommutationState?.Invoke();
            CommutationButton.image.color = Color.grey;
            isCommutationApplied = true;
        }
        else if (isCommutationApplied && CubePlayManager.instance.CanRestoreCommutationSkill())
        {
            onRestoreCommutationCheckPoint?.Invoke();
            CommutationButton.image.color = Color.white;
            isCommutationApplied = false;
            CommutationCount = 0;

        }
        else if (isCommutationApplied && !CubePlayManager.instance.CanRestoreCommutationSkill())
        {
            CommutationButton.image.color = Color.grey;

        }
    }

    public void onSelectDiagonal()
    {
        ColorBlock buttonColorBlocks = DiagonalButton.colors;
        buttonColorBlocks.normalColor = Color.red;

    }

    void onCommutationFinished()
    {
        CommutationCount = 1;
    }

    void onDiagonalFinished()
    {
        DiagonalCount = 1;
    }

    void onSwipeFinished()
    {
        SwipeCount++;
    }

    private void CubeSolved()
    {
       
       DiagonalButton.transform.localScale = Vector3.zero;
       CommutationButton.transform.localScale = Vector3.zero;
       ResetCameraButton.transform.localScale = Vector3.zero;
       RestartButton.transform.localScale = Vector3.zero;
       myTipsText.transform.localScale = Vector3.zero;
       TutorialImage.transform.localScale = Vector3.zero;
       StartCoroutine(FinishRoutine());
       //FinishButton.gameObject.SetActive(true);

    }

    void UpdateTotalStepsCountText()
    {
        totalSteps = SwipeCount + DiagonalCount + CommutationCount;
        mySwipeCountText.text = totalSteps + " step" + ((totalSteps>1)? "s": "");
    }
    
    void UpdateSolveStatusText(bool isCubeSolved)
    {
        myIsCubeSolvedText.text = "Is Cube Solved? " + (isCubeSolved? "Yes":"No");
    }

    private IEnumerator TutorialInvisible()
    {
        yield return new WaitForSeconds(tutorialDuration);

        float AlphaVal = TutorialImage.color.a;
        float currentTime = 0;
        float translationTime = 1.0f;
        float t = currentTime / translationTime;
        float targetAlpha = 0f;
        while(t<1)
        {
            currentTime += Time.deltaTime;
            t = currentTime / translationTime;
            float currentAlpha = Mathf.Lerp(AlphaVal, targetAlpha, t);
            TutorialImage.color = new Color(TutorialImage.color.r, TutorialImage.color.g, TutorialImage.color.b, currentAlpha);
            yield return null;

        }
        //TutorialImage.transform.localScale = Vector3.zero;
        yield return null;
        
    }

    public void SetTutorialVisible()
    {
        if (TutorialImage.isActiveAndEnabled)
        {
            TutorialImage.color = new Color(TutorialImage.color.r, TutorialImage.color.g, TutorialImage.color.b, 1.0f);

        }

    }

    public void SetTutorialInvisible()
    {
        if (TutorialImage.isActiveAndEnabled)
        {
            StartCoroutine(TutorialInvisible());
            //TutorialImage.color = new Color(TutorialImage.color.r, TutorialImage.color.g, TutorialImage.color.b, 0.3f);
        }
        
    }

    private void SetFinishImageInvisible()
    {
        finishAlpha = finishImage.color.a;
        if (finishImage.isActiveAndEnabled)
        {
            finishImage.gameObject.SetActive(false);
        }

    }

    private IEnumerator FinishRoutine()
    {
        
        yield return new WaitForSeconds(pre_finishDuration);
        FinishButton.gameObject.SetActive(true);
        yield return null;

        /*if (display_finishDuration > 0) 
        { 

            finishImage.gameObject.SetActive(true);
            finishImage.color = new Color(finishImage.color.r, finishImage.color.g, finishImage.color.b, 0);
            float startAlphaVal = 0;
            float targetAlphaVal = finishAlpha;
            float currentTime = 0;
            float translationTime = 0.5f;
            float t = 0;
            *//*show image*//*
            while (t < 1)
            {
                currentTime += Time.deltaTime;
                t = currentTime / translationTime;
                float currentAlpha = Mathf.Lerp(startAlphaVal, targetAlphaVal, t);
                finishImage.color = new Color(finishImage.color.r, finishImage.color.g, finishImage.color.b, currentAlpha);
                yield return null;

            }

            yield return new WaitForSeconds(display_finishDuration);
            FinishButton.gameObject.SetActive(true);

            startAlphaVal = finishAlpha;
            targetAlphaVal = 0;
            currentTime = 0;
            translationTime = 0.5f;
            t = 0;

            *//*hide image*//*
            while (t < 1)
            {
                currentTime += Time.deltaTime;
                t = currentTime / translationTime;
                float currentAlpha = Mathf.Lerp(startAlphaVal, targetAlphaVal, t);
                finishImage.color = new Color(finishImage.color.r, finishImage.color.g, finishImage.color.b, currentAlpha);
                yield return null;
            }
            finishImage.gameObject.transform.localScale = Vector3.zero;
            yield return null;
        
        }
        else
        {
            FinishButton.gameObject.SetActive(true);
            yield return null;
        }*/
    }


    //
    [SerializeField] Button FinishInfoBtn;
    private LevelManager levelManager;

    private void Start()
    {
        FinishButton.onClick.AddListener(OnClickFinishButton);
        FinishInfoBtn.onClick.AddListener(OnClickFinishInfoBtn);

        FinishInfoBtn.gameObject.SetActive(false);
        levelManager = GameObject.Find("Level Manager").GetComponent<LevelManager>();
    }

    private void OnClickFinishButton()
    {
        AkSoundEngine.PostEvent("Play_cube_final_Click", gameObject);
        FinishButton.gameObject.SetActive(false);
        StartCoroutine(FinishImageShow());
    }

    private IEnumerator FinishImageShow()
    {

            finishImage.gameObject.SetActive(true);
            finishImage.color = new Color(finishImage.color.r, finishImage.color.g, finishImage.color.b, 0);
            float startAlphaVal = 0;
            float targetAlphaVal = finishAlpha;
            float currentTime = 0;
            float translationTime = 0.5f;
            float t = 0;
            /*show image*/
            while (t < 1)
            {
                currentTime += Time.deltaTime;
                t = currentTime / translationTime;
                float currentAlpha = Mathf.Lerp(startAlphaVal, targetAlphaVal, t);
                finishImage.color = new Color(finishImage.color.r, finishImage.color.g, finishImage.color.b, currentAlpha);
                yield return null;

            }

        yield return new WaitForSeconds(display_finishDuration);
        FinishInfoBtn.gameObject.SetActive(true);
        yield return null;

    }

    private void OnClickFinishInfoBtn()
    {
        AkSoundEngine.PostEvent("Stop_cube_final_loop", gameObject);
        StartCoroutine(FinishImageHide());    
    }

    private IEnumerator FinishImageHide()
    {

        float startAlphaVal = finishAlpha;
        float targetAlphaVal = 0;
        float currentTime = 0;
        float translationTime = 0.5f;
        float t = 0;

        //*hide image*//*
        while (t < 1)
        {
            currentTime += Time.deltaTime;
            t = currentTime / translationTime;
            float currentAlpha = Mathf.Lerp(startAlphaVal, targetAlphaVal, t);
            finishImage.color = new Color(finishImage.color.r, finishImage.color.g, finishImage.color.b, currentAlpha);
            yield return null;
        }
        finishImage.gameObject.SetActive(false);
        print("finishImageHide");
        switch (PlayerPrefs.GetInt("Level"))
        {
            case 0:
                levelManager.LoadScene("NewtonLevel_GPP_Test");
                break;
            case 1:
                levelManager.LoadScene("ELE_GPP Temp");
                break;
            case 2:
                levelManager.LoadScene("Level_Fourier");
                break;
            case 3:
                levelManager.LoadScene("StartGame");
                break;
        }
        yield return null;
    }


}
