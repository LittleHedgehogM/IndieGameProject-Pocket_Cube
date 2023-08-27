using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CubePlayCheckPoint : MonoBehaviour
{
    
    public static CubePlayCheckPoint instance;

    [SerializeField]
    private GameObject FrontLeftUp;

    [SerializeField]
    private GameObject FrontLeftDown;

    [SerializeField]
    private GameObject FrontRightUp;

    [SerializeField]
    private GameObject FrontRightDown;

    [SerializeField]
    private GameObject BackLeftUp;

    [SerializeField]
    private GameObject BackLeftDown;

    [SerializeField]
    private GameObject BackRightUp;

    [SerializeField]
    private GameObject BackRightDown;

    CubeState myCubeState;
    ReadCube readCube;

    string CM_StateString;
    Vector3 CM_FLU_position;
    Quaternion CM_FLU_rotation;
    Vector3 CM_FLD_position;
    Quaternion CM_FLD_rotation;
    Vector3 CM_FRU_position;
    Quaternion CM_FRU_rotation;
    Vector3 CM_FRD_position;
    Quaternion CM_FRD_rotation;
    Vector3 CM_BLU_position;
    Quaternion CM_BLU_rotation;
    Vector3 CM_BLD_position;
    Quaternion CM_BLD_rotation;
    Vector3 CM_BRU_position;
    Quaternion CM_BRU_rotation;
    Vector3 CM_BRD_position;
    Quaternion CM_BRD_rotation;


    string DG_StateString;
    Vector3 DG_FLU_position;
    Quaternion DG_FLU_rotation;
    Vector3 DG_FLD_position;
    Quaternion DG_FLD_rotation;
    Vector3 DG_FRU_position;
    Quaternion DG_FRU_rotation;
    Vector3 DG_FRD_position;
    Quaternion DG_FRD_rotation;
    Vector3 DG_BLU_position;
    Quaternion DG_BLU_rotation;
    Vector3 DG_BLD_position;
    Quaternion DG_BLD_rotation;
    Vector3 DG_BRU_position;
    Quaternion DG_BRU_rotation;
    Vector3 DG_BRD_position;
    Quaternion DG_BRD_rotation;

    private void Awake()
    {
       myCubeState = FindObjectOfType<CubeState>();
       readCube = FindObjectOfType<ReadCube>();
        if (instance == null)
            instance = this;
    }

    public void saveCurrentStateCommutation()
    {
        CM_StateString = myCubeState.GetStateString();

        CM_FLU_position = FrontLeftUp.transform.position;
        CM_FLU_rotation = FrontLeftUp.transform.rotation;

        CM_FLD_position = FrontLeftDown.transform.position;
        CM_FLD_rotation = FrontLeftDown.transform.rotation;


        CM_FRU_position = FrontRightUp.transform.position;
        CM_FRU_rotation = FrontRightUp.transform.rotation;

        CM_FRD_position = FrontRightDown.transform.position;
        CM_FRD_rotation = FrontRightDown.transform.rotation;


        CM_BLU_position = BackLeftUp.transform.position;
        CM_BLU_rotation = BackLeftUp.transform.rotation;

        CM_BLD_position = BackLeftDown.transform.position;
        CM_BLD_rotation = BackLeftDown.transform.rotation;


        CM_BRU_position = BackRightUp.transform.position;
        CM_BRU_rotation = BackRightUp.transform.rotation;

        CM_BRD_position = BackRightDown.transform.position;
        CM_BRD_rotation = BackRightDown.transform.rotation;
    }



    public void saveCurrentStateDiagonal()
    {
        DG_StateString = myCubeState.GetStateString();

        DG_FLU_position = FrontLeftUp.transform.position;
        DG_FLU_rotation = FrontLeftUp.transform.rotation;

        DG_FLD_position = FrontLeftDown.transform.position;
        DG_FLD_rotation = FrontLeftDown.transform.rotation;


        DG_FRU_position = FrontRightUp.transform.position;
        DG_FRU_rotation = FrontRightUp.transform.rotation;

        DG_FRD_position = FrontRightDown.transform.position;
        DG_FRD_rotation = FrontRightDown.transform.rotation;


        DG_BLU_position = BackLeftUp.transform.position;
        DG_BLU_rotation = BackLeftUp.transform.rotation;

        DG_BLD_position = BackLeftDown.transform.position;
        DG_BLD_rotation = BackLeftDown.transform.rotation;


        DG_BRU_position = BackRightUp.transform.position;
        DG_BRU_rotation = BackRightUp.transform.rotation;

        DG_BRD_position = BackRightDown.transform.position;
        DG_BRD_rotation = BackRightDown.transform.rotation;
    }

    public void loadCurrentStateCommutation()
    {
        FrontLeftUp.transform.position  = CM_FLU_position;
        FrontLeftUp.transform.rotation  = CM_FLU_rotation;

        FrontLeftDown.transform.position = CM_FLD_position;
        FrontLeftDown.transform.rotation = CM_FLD_rotation;

        FrontRightUp.transform.position = CM_FRU_position;
        FrontRightUp.transform.rotation = CM_FRU_rotation;

        FrontRightDown.transform.position = CM_FRD_position;
        FrontRightDown.transform.rotation = CM_FRD_rotation;

        BackLeftUp.transform.position = CM_BLU_position;
        BackLeftUp.transform.rotation = CM_BLU_rotation;

        BackLeftDown.transform.position = CM_BLD_position;
        BackLeftDown.transform.rotation = CM_BLD_rotation;

        BackRightUp.transform.position = CM_BRU_position;
        BackRightUp.transform.rotation = CM_BRU_rotation;

        BackRightDown.transform.position = CM_BRD_position;
        BackRightDown.transform.rotation = CM_BRD_rotation;

        
    }

    public void loadCurrentStateDiagonal()
    {
        FrontLeftUp.transform.position = DG_FLU_position;
        FrontLeftUp.transform.rotation = DG_FLU_rotation;

        FrontLeftDown.transform.position = DG_FLD_position;
        FrontLeftDown.transform.rotation = DG_FLD_rotation;

        FrontRightUp.transform.position = DG_FRU_position;
        FrontRightUp.transform.rotation = DG_FRU_rotation;

        FrontRightDown.transform.position = DG_FRD_position;
        FrontRightDown.transform.rotation = DG_FRD_rotation;

        BackLeftUp.transform.position = DG_BLU_position;
        BackLeftUp.transform.rotation = DG_BLU_rotation;

        BackLeftDown.transform.position = DG_BLD_position;
        BackLeftDown.transform.rotation = DG_BLD_rotation;

        BackRightUp.transform.position = DG_BRU_position;
        BackRightUp.transform.rotation = DG_BRU_rotation;

        BackRightDown.transform.position = DG_BRD_position;
        BackRightDown.transform.rotation = DG_BRD_rotation;
    }



}
