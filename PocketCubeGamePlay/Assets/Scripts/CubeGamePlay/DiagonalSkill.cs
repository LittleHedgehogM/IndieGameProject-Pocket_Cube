using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiagonalSkill : SkillManager
{
    public static event Action onDiagonalFinished;

    private bool isDiagonalCube(GameObject firstCube, GameObject secondCube)
    {
        return Mathf.Abs(Vector3.Distance(firstCube.transform.position, secondCube.transform.position) - Mathf.Sqrt(2)) < 0.1f;
    }

    protected override bool CubesAreValid(GameObject FirstCube, GameObject SecondCube)
    {

        return isDiagonalCube(FirstCube, SecondCube);

    }

    protected override bool ApplySkill()
    {
        FirstCubeHit.transform.RotateAround(Vector3.zero, commomFaceNormalAxis, 180);
        SecondCubeHit.transform.RotateAround(Vector3.zero, commomFaceNormalAxis, 180);

        return true;
    }

    protected override void InvokeFinish()
    {
        onDiagonalFinished?.Invoke();
    }
}
