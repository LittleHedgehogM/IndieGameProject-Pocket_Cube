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
    [SerializeField] private TextMeshProUGUI myTipsText;
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

            //UpdateDiagonalCountText(_diagonalCount);
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
            //UpdateCommutationCountText(_commutationCount);
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
        CommutationCount = 1;
    }

    void onDiagonalFinished()
    {
        //DiagonalCount++;
        //if (DiagonalCount >= maxDiagonal)
        //{
        //    isDiagonalApplied = true;
        //}
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
  
    }

    private void UpdateTotalStepCount()
    {
        totalSteps = SwipeCount + DiagonalCount + CommutationCount;
        //if (totalSteps > suggestedStepCount)
        //{
        //    RestartButton.gameObject.SetActive(true);
        //}
    }

    void UpdateTotalStepsCountText()
    {
        totalSteps = SwipeCount + DiagonalCount + CommutationCount;
        mySwipeCountText.text = totalSteps + " step" + ((totalSteps>1)? "s": "");
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
