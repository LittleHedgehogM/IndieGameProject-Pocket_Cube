using System;
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

    public Button DiagonalButton;
    public Button CommutationButton;
    public Button RestartButton;
    public Button FinishButton;
    public Button ResetCameraButton;
    private int totalSteps;

    [SerializeField] private bool isCommutationUnlocked;
    [SerializeField] private bool isDiagonalUnlocked;
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
            UpdateSwipeCountText(_swipeCount);
            UpdateTotalStepCount();
        }
    }


    private int _diagonalCount;
    public int DiagonalCount
    {
        get { return _diagonalCount; }
        set
        {
            _diagonalCount = value;
            UpdateDiagonalCountText(_diagonalCount);
            UpdateTotalStepCount();
        }
    }
    private int _commutationCount;
    public int CommutationCount
    {
        get { return _commutationCount; }
        set 
        {
            _commutationCount = value;
            UpdateCommutationCountText(_commutationCount);
            UpdateTotalStepCount();
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


    }

    private void OnEnable()
    {
        //CubePlayManager.onCubeSolved += CubeSolved;
        SwipeFaceManager.onSwipeFinished += onSwipeFinished;
        DiagonalSkill.onDiagonalFinished += onDiagonalFinished;
        CommutationSkill.onCommutataionFinished += onCommutationFinished;
    }

    private void OnDisable()
    {
        //CubePlayManager.onCubeSolved -= CubeSolved;
        SwipeFaceManager.onSwipeFinished += onSwipeFinished;
        DiagonalSkill.onDiagonalFinished += onDiagonalFinished;
        CommutationSkill.onCommutataionFinished += onCommutationFinished;
    }

    
    public void clickDiagonalButton()
    {
       isDiagonalApplied = !isDiagonalApplied;
       if (isDiagonalApplied)
       {
            onEnterDiagonalState?.Invoke();
            DiagonalButton.image.color = Color.grey;
        }
       else
       {
            onRestoreDiagonalCheckPoint?.Invoke();
            DiagonalButton.image.color = Color.white;

        }
    }

    public void clickCommutationButton()
    {
        isCommutationApplied = !isCommutationApplied;
        if (isCommutationApplied)
        {
            onEnterCommutationState?.Invoke();
            CommutationButton.image.color = Color.grey;

        }
        else
        {
            onRestoreCommutationCheckPoint?.Invoke();
            CommutationButton.image.color = Color.white;

        }
    }

    public void onSelectDiagonal()
    {
        ColorBlock buttonColorBlocks = DiagonalButton.colors;
        buttonColorBlocks.normalColor = Color.red;

    }

    void onCommutationFinished()
    {
        //CommutationCount++;
        //if (CommutationCount >= maxCommutation)
        //{
        //    isCommutationApplied = true;
        //}
    }

    void onDiagonalFinished()
    {
        //DiagonalCount++;
        //if (DiagonalCount >= maxDiagonal)
        //{
        //    isDiagonalApplied = true;
        //}
    }

    void onSwipeFinished()
    {
        SwipeCount++;
    }

    private void CubeSolved()
    {
        if (totalSteps > 0)
        {
            FinishButton.gameObject.SetActive(true);
        }   
    }

    private void UpdateTotalStepCount()
    {
        totalSteps = SwipeCount + DiagonalCount + CommutationCount;
        if (totalSteps > suggestedStepCount)
        {
            //RestartButton.gameObject.SetActive(true);
        }
    }

    void UpdateSwipeCountText(int swipeCount)
    {
        mySwipeCountText.text = "Swipe Count = " + swipeCount;
    }

    void UpdateDiagonalCountText(int diagonalCount)
    {
        myDiagonalCountText.text = "Diagonal Count = " + diagonalCount;
    }

    void UpdateCommutationCountText(int commutationCount)
    {
        myCommutationCountText.text = "Commutation Count = " + commutationCount;
    }
    
    void UpdateSolveStatusText(bool isCubeSolved)
    {
        myIsCubeSolvedText.text = "Is Cube Solved? " + (isCubeSolved? "Yes":"False");
    }


}
