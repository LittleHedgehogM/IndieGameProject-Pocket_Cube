using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static System.TimeZoneInfo;

public class Origin_Controller : MonoBehaviour
{
    [SerializeField] private GameObject Sphere;
    [SerializeField] private Camera mainCam;
    [SerializeField] private Origin_Axis leftAxis;
    [SerializeField] private Origin_Axis rightAxis;  

    [SerializeField] private Origin_Axis upAxis;
    [SerializeField] private Origin_Axis downAxis;
    [SerializeField] private Origin_Cube cubeController;
   
    Origin_RotationTarget myRotationTarget;
    public static Action DisableAllAxis;
    Origin_VFXController myVFXController;
    [SerializeField][Range(0.5f, 3f)]  private float translationTime;
    [SerializeField] AnimationCurve translationCurve;

    public static Action rotateFinish;

    bool enableInteraction = true;

    int leftAxisClickCount  = 0;
    int upAxisClickCount = 0;
    Quaternion originAngle;

    enum PuzzleState
    {
        Phase_One,
        Phase_Two,
        Phase_Three,
        Solved,
    };
    private PuzzleState currentState;
    private void Start()
    {
        myRotationTarget = FindObjectOfType<Origin_RotationTarget>();
        currentState = PuzzleState.Phase_One;
        InitPhaseOne();
        enableInteraction = true;
        originAngle = Sphere.transform.rotation;
        myVFXController = FindObjectOfType<Origin_VFXController>();
        Application.targetFrameRate = 60;
    }

    private void OnEnable()
    {
        //Cinemachine_cart.CinemachineFinished += EnableSceneInteraction;
        Origin_Axis.LeftAxisClicked     +=isLeftAxisClicked;
        Origin_Axis.RightAxisClicked    +=isRightAxisClicked;
        Origin_Axis.UpAxisClicked       +=isUpAxisClicked;
        Origin_Axis.DownAxisClicked     +=isDownAxisClicked;
        Origin_RotationTarget.TargetVanished += PlayCubeAnim;
    }

    private void OnDisable()
    {
        //Cinemachine_cart.CinemachineFinished -= EnableSceneInteraction;
        Origin_Axis.LeftAxisClicked     -= isLeftAxisClicked;
        Origin_Axis.RightAxisClicked    -= isRightAxisClicked;
        Origin_Axis.UpAxisClicked       -= isUpAxisClicked;
        Origin_Axis.DownAxisClicked     -= isDownAxisClicked;
        Origin_RotationTarget.TargetVanished -= PlayCubeAnim;

    }

    private void isLeftAxisClicked()
    {
        leftAxisClickCount = (leftAxisClickCount + 1) % 5 ;
        StartCoroutine(Rotate(Vector3.up, true, currentUpGap == 108.0f ? 72.0f : 360.0f));
        enableInteraction = false;
    }
    private void isRightAxisClicked()
    {
        leftAxisClickCount = (leftAxisClickCount - 1) % 5;
        StartCoroutine(Rotate(Vector3.up, false, currentUpGap == 108.0f ? 72.0f : 360.0f));
        enableInteraction = false;
    }
    private void isUpAxisClicked()
    {
        upAxisClickCount = (upAxisClickCount + 1) % 4 ;
        StartCoroutine(Rotate(Vector3.right, true, currentUpGap));
        currentUpGap = getNextAngleGap(currentUpGap);
        enableInteraction = false;
    }
    private void isDownAxisClicked()
    {
        upAxisClickCount = (upAxisClickCount - 1) % 4;
        StartCoroutine(Rotate(Vector3.right, false, 180.0f - currentUpGap));
        currentUpGap = getNextAngleGap(currentUpGap);
        enableInteraction = false;
    }

    private IEnumerator Rotate(Vector3 Axis, bool isClockwise, float angleGap)
    {

        // Add rotate sphere audio
        float currentUsedTime = 0;
        float t = 0;
        float currentAngle = 0f;

        translationTime = 0.5f * ( (angleGap) / 72.0f);
        while (t < 1)
        {
            currentUsedTime += Time.deltaTime;
            t = currentUsedTime / translationTime;

            float targetAngle = Mathf.Lerp(0, angleGap, translationCurve.Evaluate(t));
            float deltaAngle = targetAngle - currentAngle ;
            currentAngle = targetAngle ;
            Sphere.transform.RotateAround(Sphere.transform.position, Axis, isClockwise ? deltaAngle : -deltaAngle) ;

            yield return null;
        }
        enableInteraction = true;
        rotateFinish?.Invoke();
    }

    float currentUpGap = 108.0f;
    private float getNextAngleGap(float currentGap)
    {
        if (currentGap == 72.0f)
        {
            currentGap = 108.0f;
        }
        else 
        {
            currentGap = 72.0f;
        }

        return currentGap;
    }

    private void InitPhaseOne()
    {
        myRotationTarget.InitPhaseOne();
        leftAxis.setActive(true);
        rightAxis.setActive(true);
        upAxis.setActive(false);
        downAxis. setActive(false);
    }

    private IEnumerator InitPhaseTwo()
    {
        myVFXController.playRotationVFXAt(Sphere.transform);
        leftAxis.setActive(false);
        rightAxis.setActive(false);
        yield return new WaitForSeconds(1);
        myRotationTarget.InitPhaseTwo();      
        upAxis.setActive(true);
        downAxis.setActive(true);
    }
    
    private IEnumerator InitPhaseThree()
    {
        myVFXController.playRotationVFXAt(Sphere.transform);
        yield return new WaitForSeconds(1);

        myRotationTarget.InitPhaseThree();
        leftAxis.setActive(true);
        rightAxis.setActive(true);
        upAxis.setActive(true);
        downAxis.setActive(true);
    }

    private IEnumerator InitSolved()
    {
        yield return new WaitForSeconds(1);
        myRotationTarget.minimizeTargetAndshowCube();
    }

    private void PlayCubeAnim()
    {
        // Cube Animation Start
        myVFXController.playCubeFinishVFXAt(cubeController.transform);
        cubeController.PlayAnim();
    }

    private void EnableSceneInteraction()
    {
        enableInteraction = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (!enableInteraction)
        {
            return;
        }
        switch (currentState)
        {
            case PuzzleState.Phase_One:
            {
                 if (leftAxisClickCount == 4 || leftAxisClickCount == -1)
                 {
                      Debug.Log("FirstPhaseSolved");
                      myRotationTarget.FinishPhaseOne();
                      currentState = PuzzleState.Phase_Two;
                      StartCoroutine(InitPhaseTwo());                      
                 }
                 break;
            }
            case PuzzleState.Phase_Two:
            {
                if (upAxisClickCount == 2 || upAxisClickCount == -2)
                 {
                      Debug.Log("SecondPhaseSolved");
                      myRotationTarget.FinishPhaseTwo();
                      currentState = PuzzleState.Phase_Three;
                      StartCoroutine(InitPhaseThree());

                 }
                break;
            }
            case PuzzleState.Phase_Three:
            {
                if (Quaternion.Angle(Sphere.transform.rotation, originAngle) <0.1f)
                {
                      Debug.Log("ThirdPhaseSolved");
                      myRotationTarget.FinishPhaseThree();
                      currentState = PuzzleState.Solved;
                      StartCoroutine(InitSolved());
                      DisableAllAxis?.Invoke();
                }
                break;
            }
            case PuzzleState.Solved:
            {                               
                break;
            }
            default:
                { break; }

        }
                 
    }
}
