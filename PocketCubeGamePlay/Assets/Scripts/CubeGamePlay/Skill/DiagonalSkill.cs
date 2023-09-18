using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiagonalSkill : SkillManager
{


    [SerializeField]
    float diagonalAnimationTime;

    [SerializeField] 
    private AnimationCurve positionTranslationCurve;    

    [SerializeField]
    private AnimationCurve scaleTranslationCurve;

    public static event Action onDiagonalFinished;

    private bool isDiagonalCube(GameObject firstCube, GameObject secondCube)
    {
        return Mathf.Abs(Vector3.Distance(firstCube.transform.position, secondCube.transform.position) - Mathf.Sqrt(2)) < 0.1f;
    }

    protected override bool CubesAreValid(GameObject FirstCube, GameObject SecondCube)
    {

        return isDiagonalCube(FirstCube, SecondCube);

    }

    protected override IEnumerator ApplySkill()
    {

        StartCoroutine(startAnimation());
        

        yield return null;
        //return true;
    }

    protected override void InvokeFinish()
    {
        onDiagonalFinished?.Invoke();
    }


    protected override IEnumerator startAnimation()
    {
        float currentUsedTime =0;
        //float currentRotationDegree = 0;
        float t = 0;

        startPos = FirstCubeHit.transform.position;
        endPos   = SecondCubeHit.transform.position;
        startScale = FirstCubeHit.transform.localScale;
        Vector3 midPos   = (startPos + endPos) / 2;

        //float distance = Vector3.Distance(startPos, midPos);

        while (t < 1)
        {
            currentUsedTime += Time.deltaTime;
            t = currentUsedTime / diagonalAnimationTime;
            //float speed = Time.deltaTime;
            FirstCubeHit.transform.position     = Vector3.Lerp(startPos, midPos, positionTranslationCurve.Evaluate(t));
            FirstCubeHit.transform.localScale   = Vector3.Lerp(startScale, Vector3.zero, scaleTranslationCurve.Evaluate(t));
            SecondCubeHit.transform.position    = Vector3.Lerp(endPos, midPos, positionTranslationCurve.Evaluate(t));
            SecondCubeHit.transform.localScale  = Vector3.Lerp(startScale, Vector3.zero, scaleTranslationCurve.Evaluate(t));

           

            yield return null;

        }

        FirstCubeHit.transform.RotateAround(FirstCubeHit.transform.position, commomFaceNormalAxis, 180);
        SecondCubeHit.transform.RotateAround(SecondFaceHit.transform.position, commomFaceNormalAxis, 180);

        currentUsedTime = 0;
        t = 0;



        while (t < 1)
        {
            currentUsedTime += Time.deltaTime;
            t = currentUsedTime / diagonalAnimationTime;
            FirstCubeHit.transform.position     = Vector3.Lerp(midPos, endPos, positionTranslationCurve.Evaluate(t));
            FirstCubeHit.transform.localScale   = Vector3.Lerp(Vector3.zero, startScale, scaleTranslationCurve.Evaluate(t));
            SecondCubeHit.transform.localScale  = Vector3.Lerp(Vector3.zero, startScale, scaleTranslationCurve.Evaluate(t));
            SecondCubeHit.transform.position    = Vector3.Lerp(midPos, startPos, positionTranslationCurve.Evaluate(t));

            yield return null;

        }


        yield return null;
    }
}
