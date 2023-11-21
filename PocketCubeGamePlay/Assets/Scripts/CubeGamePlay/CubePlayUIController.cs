using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using Button = UnityEngine.UI.Button;
//using static CubePlayManager;

public class CubePlayUIController : MonoBehaviour
{

    [SerializeField] private TextMeshProUGUI mySwipeCountText;
    [SerializeField] private TextMeshProUGUI myDiagonalCountText;
    [SerializeField] private TextMeshProUGUI myCommutationCountText;
    [SerializeField] private TextMeshProUGUI myIsCubeSolvedText;
    [SerializeField] private TextMeshProUGUI myTipsText;
    [SerializeField] private UnityEngine.UI.Image TutorialImage;
    [SerializeField][Range(3, 8)] private int sleepAfterSeconds;


    public Button DiagonalButton;
    public Button CommutationButton;
    public Button RestartButton;
    public Button FinishButton;
    public Button ResetCameraButton;
    private int totalSteps;

    [SerializeField] private bool isCommutationUnlocked;
    [SerializeField] private bool isDiagonalUnlocked;
    [SerializeField] private bool isShowTutorial;
    [SerializeField] private int maxCommutation;
    [SerializeField] private int maxDiagonal;
    [SerializeField] private int suggestedStepCount;


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
       FinishButton.gameObject.SetActive(true);
       DiagonalButton.transform.localScale = Vector3.zero;
       CommutationButton.transform.localScale = Vector3.zero;
       ResetCameraButton.transform.localScale = Vector3.zero;
       RestartButton.transform.localScale = Vector3.zero;
       myTipsText.transform.localScale = Vector3.zero;
       TutorialImage.transform.localScale = Vector3.zero;


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
        yield return new WaitForSeconds(sleepAfterSeconds);

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

}
