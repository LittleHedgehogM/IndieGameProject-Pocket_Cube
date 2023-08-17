using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class CubePlayUIController : MonoBehaviour
{

    [SerializeField]  TextMeshProUGUI mySwipeCountText;
    [SerializeField]  TextMeshProUGUI myDiagonalCountText;
    [SerializeField]  TextMeshProUGUI myCommutationCountText;
    [SerializeField]  TextMeshProUGUI myIsCubeSolvedText;

    public Button DiagonalButton;
    public Button CommutationButton;
    public Button RestartButton;

    private int _swipeCnt;
    public int SwipeCount
    {
        get { return _swipeCnt; }
        set 
        { 
            _swipeCnt = value;
            UpdateSwipeCountText(_swipeCnt);
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

    //// Start is called before the first frame update
    
    void Start()
    {
        SwipeCount = 0;
        CommutationCount = 0;
        DiagonalCount = 0;
        SolveResult = false;
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
