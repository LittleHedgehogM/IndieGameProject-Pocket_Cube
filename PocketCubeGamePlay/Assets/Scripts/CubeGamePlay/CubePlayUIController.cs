using System;
using System.Collections;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.UIElements;
using static System.TimeZoneInfo;
using Button = UnityEngine.UI.Button;
using Image = UnityEngine.UI.Image;
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

    [SerializeField][Range(0,40)] private int tutorialDuration;

    [Header("Tutorial Panel Settings")]
    [SerializeField] private bool isShowTutorialPanel;
    [SerializeField][Range(0, 3)] private int pre_tutorialPanelDuration;
    [SerializeField] private UnityEngine.UI.Image TutorialPanelImage;
    [SerializeField] UnityEngine.UI.Image switchLoader;
    [SerializeField] Sprite[] tutorialSprites;
    [SerializeField] float translationTime;
    [SerializeField] Image GoPrevButton;
    [SerializeField] Image GoNextButton;
    [SerializeField] Image ConfirmButton;

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

    public static event Action TutorialPanelHide;

    public static event Action restoreCommutationButton;
    public static event Action restoreDiagonalButton;

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
        if (CommutationCount == 0) {
            restoreCommutationButton?.Invoke();
        }
        //if (commutationApplied) {
        //    CommutationButton.image.color = Color.grey;
        //}
        //else 
        //{
        //    CommutationButton.image.color = Color.white;
        //}
    }

    public void restoreTotalStepsCommutation(int restoreSteps, bool diagonalApplied)
    {
        SwipeCount = restoreSteps;
        CommutationCount = 0;
        isDiagonalApplied = diagonalApplied;
        DiagonalCount = diagonalApplied ? 1 : 0;
        if (DiagonalCount == 0) 
        {
            restoreDiagonalButton?.Invoke();
        }
        //if (diagonalApplied) 
        //{
        //    DiagonalButton.image.color = Color.grey;
        //}
        //else
        //{
        //    DiagonalButton.image.color = Color.white;

        //}
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

        if (TutorialImage!=null) 
        {
            TutorialImage.gameObject.SetActive(isShowTutorial);
        }
        if (TutorialPanelImage!=null) 
        {
            TutorialPanelImage.gameObject.SetActive(false);
        }

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
            //DiagonalButton.image.color = Color.grey;
            isDiagonalApplied = true;

        }
       else if (isDiagonalApplied && CubePlayManager.instance.CanRestoreDiagonalSkill())
       {
            onRestoreDiagonalCheckPoint?.Invoke();
            //DiagonalButton.image.color = Color.white;
            isDiagonalApplied = false;
            DiagonalCount = 0;

        }
        else if (isDiagonalApplied && !CubePlayManager.instance.CanRestoreDiagonalSkill())
        {
            //DiagonalButton.image.color = Color.grey;
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
            //CommutationButton.image.color = Color.grey;
            isCommutationApplied = true;
        }
        else if (isCommutationApplied && CubePlayManager.instance.CanRestoreCommutationSkill())
        {
            onRestoreCommutationCheckPoint?.Invoke();
            //CommutationButton.image.color = Color.white;
            isCommutationApplied = false;
            CommutationCount = 0;

        }
        else if (isCommutationApplied && !CubePlayManager.instance.CanRestoreCommutationSkill())
        {
            //CommutationButton.image.color = Color.grey;

        }
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

        //Savedata
        SaveLevel(PlayerPrefs.GetInt("Level"));
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

    /* ================ Tutorial Panel ==================== */
    private int index = 0;
    Color transparentWhite;
    
    public void initTutorialPanel()
    {
        Assert.IsNotNull(tutorialSprites);
        transparentWhite = new Color(Color.white.r, Color.white.g, Color.white.b, 0);
        index = 0;

        TutorialPanelImage.color = transparentWhite;
        TutorialPanelImage.transform.localScale = Vector3.zero;
        TutorialPanelImage.sprite = tutorialSprites[index];

        switchLoader.color = new Color(switchLoader.color.r, switchLoader.color.g, switchLoader.color.b, 0);
        switchLoader.gameObject.SetActive(false);

        GoPrevButton.color = transparentWhite;
        GoPrevButton.transform.localScale = Vector3.zero;

        GoNextButton.color = transparentWhite;
        GoNextButton.transform.localScale = Vector3.zero;

        ConfirmButton.color = transparentWhite;
        ConfirmButton.transform.localScale = Vector3.zero;


    }

    public void TutorialStarts()
    {


        initTutorialPanel();
        if (isShowTutorialPanel)
        {
           index = 0;

           TutorialPanelImage.sprite = tutorialSprites[index];
           TutorialPanelImage.gameObject.SetActive(true);
           StartCoroutine(showImage(TutorialPanelImage,  pre_tutorialPanelDuration, translationTime));
           if (tutorialSprites.Length == 1)
           {
                showConfirmButton();
                switchLoader.gameObject.SetActive(false);

            }
            else if (tutorialSprites.Length > 1)
           {
                StartCoroutine(showImage(GoNextButton, 0, 0.2f));
           } 
        }
        

    }

    public void goPreviousTutorial()
    {
        if (index >= 1 && index <= tutorialSprites.Length - 1)
        {

            index--;
            StartCoroutine(switchTutorialImage(tutorialSprites[index]));
            showGoNextButton();
            hideConfirmButton();
        }

        if (index == 0)
        {
            hideGoPrevButton();
        }

    }

    public void goNextTutorial()
    {
        if (index >= 0 && index < tutorialSprites.Length - 1)
        {
            index++;
            StartCoroutine(switchTutorialImage(tutorialSprites[index]));
            showGoPrevButton();
        }

        if (index == tutorialSprites.Length - 1)
        {
            hideGoNextButton();
            showConfirmButton();
        }

    }
    public bool getShowTutorialPanel()
    {
        return isShowTutorialPanel;
    }

    public void closeTutorial()
    {
        StartCoroutine(hideTutorial());
    }

    private void showGoPrevButton()
    {
        GoPrevButton.gameObject.SetActive(true);
        StartCoroutine(showImage(GoPrevButton, 0, 0.2f));
    }

    private void showGoNextButton()
    {
    GoNextButton.gameObject.SetActive(true);
        StartCoroutine(showImage(GoNextButton, 0, 0.2f));
    }


    private void hideGoPrevButton()
    {
        StartCoroutine(hideImage(GoPrevButton, 0.2f));
    }

    private void hideGoNextButton()
    {
        StartCoroutine(hideImage(GoNextButton, 0.2f));
    }

    private void showConfirmButton()
    {
        ConfirmButton.gameObject.SetActive(true);
        StartCoroutine(showImage(ConfirmButton, 0, 0.2f));
    }

    private void hideConfirmButton()
    {
        StartCoroutine(hideImage(ConfirmButton, 0.2f));
    }

    private IEnumerator showImage(Image img, float pre_duration, float duration)
    {
        if (img.color.a > 0)
        {
            yield return null;
        }
        else
        {

            yield return new WaitForSeconds(pre_duration);
            img.transform.localScale = Vector3.one;

            float currentTime = 0;
            float t = currentTime / duration;
            float targetAlpha = 1;

            while (t < 1)
            {
                currentTime += Time.deltaTime;
                t = currentTime / duration;
                float currentAlpha = Mathf.Lerp(0, targetAlpha, t);
                img.color = new Color(img.color.r, img.color.g, img.color.b, currentAlpha);
                yield return null;

            }


        }
    }

    private IEnumerator hideImage(Image img, float duration)
    {

        if (img.color.a == 0 || img.transform.localScale == Vector3.zero)
        {
            yield return null;
        }
        else
        {
            float currentTime = 0;
            float t = currentTime / duration;
            float targetAlpha = 0;

            while (t < 1)
            {
                currentTime += Time.deltaTime;
                t = currentTime / duration;
                float currentAlpha = Mathf.Lerp(1, targetAlpha, t);
                img.color = new Color(img.color.r, img.color.g, img.color.b, currentAlpha);
                yield return null;

            }

            img.transform.localScale = Vector3.zero;
            yield return null;
        }

    }

    private IEnumerator switchTutorialImage(Sprite next)
    {

        float currentTime = 0;
        float t = currentTime / translationTime;

        switchLoader.gameObject.SetActive(true);
        while (t < 1)
        {
            currentTime += Time.deltaTime;
            t = currentTime / translationTime;
            float currentAlpha = Mathf.Lerp(0, 1, t);
            switchLoader.color = new Color(switchLoader.color.r, switchLoader.color.g, switchLoader.color.b, currentAlpha);

            yield return null;

        }

        TutorialPanelImage.sprite = next;
        yield return null;

        currentTime = 0;
        t = currentTime / translationTime;
        while (t < 1)
        {
            currentTime += Time.deltaTime;
            t = currentTime / translationTime;
            float currentAlpha = Mathf.Lerp(1, 0, t);
            switchLoader.color = new Color(switchLoader.color.r, switchLoader.color.g, switchLoader.color.b, currentAlpha);

            yield return null;

        }
        switchLoader.gameObject.SetActive(false);

        yield return null;


    }

    private IEnumerator hideTutorial()
    {
        StartCoroutine(hideImage(ConfirmButton, 0.1f));
        hideGoPrevButton();
        hideGoNextButton();
        float currentTime = 0;
        float t = currentTime / 1.0f;

        while (t < 1)
        {
            currentTime += Time.deltaTime;
            t = currentTime / (translationTime * 2);
            float currentAlpha = Mathf.Lerp(1, 0, t);
            TutorialPanelImage.color = new Color(TutorialPanelImage.color.r,
                                                                  TutorialPanelImage.color.g,
                                                                  TutorialPanelImage.color.b, currentAlpha);
            yield return null;

        }

        TutorialPanelHide?.Invoke();
        TutorialPanelImage.transform.localScale = Vector3.zero;
        yield return null;
    }


    /* ================ Tutorial Panel ==================== */

    public static Action<string> ACHIEVEMENT_03;

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
        ACHIEVEMENT_03?.Invoke("ACHIEVEMENT_03");
        yield return null;
    }


    //
    [SerializeField] Button FinishInfoBtn;
    private LevelManager levelManager;

    private void Start()
    {
        FinishButton.onClick.AddListener(OnClickFinishButton);
        FinishInfoBtn.onClick.AddListener(OnClickFinishInfoBtn);

        FinishInfoBtn.gameObject.SetActive(false);
        if (GameObject.Find("Level Manager"))
        {
            levelManager = GameObject.Find("Level Manager").GetComponent<LevelManager>();
        }
        
    }

    private void OnClickFinishButton()
    {
        FinishButton.interactable = false;
        AkSoundEngine.PostEvent("Play_cube_final_Click", gameObject);
        FinishButton.gameObject.SetActive(false);
        

        StartCoroutine(FinishImageShow());
    }

    private void SaveLevel(int currentLevel)
    {
        PlayerPrefs.SetInt("Level", currentLevel + 1);
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
        FinishInfoBtn.interactable = false;
        AkSoundEngine.PostEvent("Stop_cube_final_loop", gameObject);
        StartCoroutine(FinishImageHide());    
    }

    private IEnumerator FinishImageHide()
    {

        switch (PlayerPrefs.GetInt("Level"))
        {
            case 1:
                levelManager.LoadScene("NewtonLevel_GPP_Test");
                break;
            case 2:
                levelManager.LoadScene("ELE_GPP Temp");
                break;
            case 3:
                levelManager.LoadScene("Level_Fourier");
                break;
            case 4:
                SceneManager.LoadScene("EndingScene");
                break;
        }
        yield return null;
    }


}
