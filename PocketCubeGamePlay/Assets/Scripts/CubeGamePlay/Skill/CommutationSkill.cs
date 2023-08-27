using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommutationSkill : SkillManager
{
    [SerializeField]
    float commutationAnimationTime;

    [SerializeField]
    private AnimationCurve positionTranslationCurve;

    [SerializeField]
    private AnimationCurve scaleTranslationCurve;

    public static event Action onCommutataionFinished;
    private bool isAdjacentCube(GameObject firstCube, GameObject secondCube)
    {
        return Mathf.Abs(Vector3.Distance(firstCube.transform.position, secondCube.transform.position) - 1) < 0.1f ;
    }
 
    protected override  bool CubesAreValid(GameObject FirstCube, GameObject SecondCube)
    {  
        return isAdjacentCube(FirstCubeHit, SecondCube);

    }

    protected override IEnumerator ApplySkill()
    {
        //Vector3 FirstDestinationVec = SecondCubeHit.transform.position - FirstCubeHit.transform.position;

        //// get face normal of First and Second Cube
        //commomFaceNormalAxis = FindFaceNormal(FirstFaceHit);
        //Vector3 rotationalAxis = Vector3.Cross(FirstDestinationVec.normalized, commomFaceNormalAxis);
        //FirstCubeHit.transform.RotateAround(Vector3.zero, rotationalAxis, -90);
        //SecondCubeHit.transform.RotateAround(Vector3.zero, rotationalAxis, 90);

        StartCoroutine(startAnimation());
        //return true;
        yield return null;
    }

    protected override void InvokeFinish()
    {
        onCommutataionFinished?.Invoke();
    }


    protected override IEnumerator startAnimation()
    {
        float currentUsedTime = 0;
        //float currentRotationDegree = 0;
        float t = 0;
        startPos = FirstCubeHit.transform.position;
        endPos = SecondCubeHit.transform.position;
        startScale = FirstCubeHit.transform.localScale;
        Vector3 midPos = Vector3.zero;
        Vector3 FirstDestinationVec = endPos - startPos;
        commomFaceNormalAxis = FindFaceNormal(FirstFaceHit);
        Vector3 rotationalAxis = Vector3.Cross(FirstDestinationVec.normalized, commomFaceNormalAxis);

        while (t < 1)
        {
            currentUsedTime += Time.deltaTime;
            t = currentUsedTime / commutationAnimationTime;
            //float speed = Time.deltaTime;
            FirstCubeHit.transform.position    = Vector3.Lerp(startPos, midPos, positionTranslationCurve.Evaluate(t));
            FirstCubeHit.transform.localScale  = Vector3.Lerp(startScale, Vector3.zero, scaleTranslationCurve.Evaluate(t));
            SecondCubeHit.transform.position   = Vector3.Lerp(endPos, midPos, positionTranslationCurve.Evaluate(t));
            SecondCubeHit.transform.localScale = Vector3.Lerp(startScale, Vector3.zero, scaleTranslationCurve.Evaluate(t));

            yield return null;

        }

        FirstCubeHit.transform.RotateAround(FirstCubeHit.transform.position, rotationalAxis, -90);
        SecondCubeHit.transform.RotateAround(SecondFaceHit.transform.position, rotationalAxis, 90);

        currentUsedTime = 0;
        t = 0;
        while (t < 1)
        {
            currentUsedTime += Time.deltaTime;
            t = currentUsedTime / commutationAnimationTime;
            FirstCubeHit.transform.position = Vector3.Lerp(midPos, endPos, positionTranslationCurve.Evaluate(t));
            FirstCubeHit.transform.localScale = Vector3.Lerp(Vector3.zero, startScale, scaleTranslationCurve.Evaluate(t));
            SecondCubeHit.transform.localScale = Vector3.Lerp(Vector3.zero, startScale, scaleTranslationCurve.Evaluate(t));
            SecondCubeHit.transform.position = Vector3.Lerp(midPos, startPos, positionTranslationCurve.Evaluate(t));

            yield return null;
        }

        yield return new WaitForSeconds(commutationAnimationTime*2);
    }
}
