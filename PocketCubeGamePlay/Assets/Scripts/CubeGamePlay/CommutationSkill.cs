using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommutationSkill : SkillManager
{
    public static event Action onCommutataionFinished;
    private bool isAdjacentCube(GameObject firstCube, GameObject secondCube)
    {
        return Mathf.Abs(Vector3.Distance(firstCube.transform.position, secondCube.transform.position) - 1) < 0.001f; ;
    }
 
    protected override  bool CubesAreValid(GameObject FirstCube, GameObject SecondCube)
    {  
        return isAdjacentCube(FirstCubeHit, SecondCube);

    }

    protected override bool ApplySkill()
    {
        Vector3 FirstDestinationVec = SecondCubeHit.transform.position - FirstCubeHit.transform.position;

        // get face normal of First and Second Cube
        commomFaceNormalAxis = FindFaceNormal(FirstFaceHit);
        Vector3 rotationalAxis = Vector3.Cross(FirstDestinationVec.normalized, commomFaceNormalAxis);
        FirstCubeHit.transform.RotateAround(Vector3.zero, rotationalAxis, -90);
        SecondCubeHit.transform.RotateAround(Vector3.zero, rotationalAxis, 90);
        return true;
    }

    protected override void InvokeFinish()
    {
        onCommutataionFinished?.Invoke();
    }
}
