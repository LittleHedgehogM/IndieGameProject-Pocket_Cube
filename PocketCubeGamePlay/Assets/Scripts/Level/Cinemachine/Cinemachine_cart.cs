using System;
using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class Cinemachine_cart : MonoBehaviour
{
    /// <summary>The path to follow</summary>
    [Tooltip("The path to follow")]
    public CinemachinePathBase m_Path;

    /// <summary>This enum defines the options available for the update method.</summary>
    //public enum UpdateMethod
    //{
    //    /// <summary>Updated in normal MonoBehaviour Update.</summary>
    //    Update,
    //    /// <summary>Updated in sync with the Physics module, in FixedUpdate</summary>
    //    FixedUpdate,
    //    /// <summary>Updated in normal MonoBehaviour LateUpdate</summary>
    //    LateUpdate
    //};

    /// <summary>When to move the cart, if Velocity is non-zero</summary>
    //[Tooltip("When to move the cart, if Velocity is non-zero")]
    //public UpdateMethod m_UpdateMethod = UpdateMethod.Update;

    /// <summary>How to interpret the Path Position</summary>
    [Tooltip("How to interpret the Path Position.  If set to Path Units, values are as follows: 0 represents the first waypoint on the path, 1 is the second, and so on.  Values in-between are points on the path in between the waypoints.  If set to Distance, then Path Position represents distance along the path.")]
    public CinemachinePathBase.PositionUnits m_PositionUnits = CinemachinePathBase.PositionUnits.Distance;

    /// <summary>Move the cart with this speed</summary>
    [Tooltip("Move the cart with this speed along the path.  The value is interpreted according to the Position Units setting.")]
    [FormerlySerializedAs("m_Velocity")]
    public float m_Speed;

    /// <summary>The cart's current position on the path, in distance units</summary>
    [Tooltip("The position along the path at which the cart will be placed.  This can be animated directly or, if the velocity is non-zero, will be updated automatically.  The value is interpreted according to the Position Units setting.")]
    [FormerlySerializedAs("m_CurrentDistance")]
    public float m_Position;
    private bool canPlayCinemachine = false;
    public static Action CinemachineFinished;

    private void OnEnable()
    {
        VideoScript.VideoFinished += StartPlayingCinemachine;

    }

    private void OnDisable()
    {
        VideoScript.VideoFinished -= StartPlayingCinemachine;

    }

    public void StartPlayingCinemachine()
    {
        canPlayCinemachine = true;
    }


    void Update()
    {
        float speed = Application.isPlaying ? m_Speed : 0;
        if (canPlayCinemachine)
        {
            SetCartPosition(m_Position + speed * Time.deltaTime);          
        }            
    }

    void SetCartPosition(float distanceAlongPath)
    {
        if (m_Path != null)
        {
            m_Position = m_Path.StandardizeUnit(distanceAlongPath, m_PositionUnits);
            float dist = Vector3.Distance(transform.position, m_Path.EvaluatePositionAtUnit(m_Position, m_PositionUnits));
            if (dist < 0.01f)
            {
                CinemachineFinished?.Invoke();
                canPlayCinemachine = false;
            }       
            else 
            {
                transform.position = m_Path.EvaluatePositionAtUnit(m_Position, m_PositionUnits);
                transform.rotation = m_Path.EvaluateOrientationAtUnit(m_Position, m_PositionUnits);
                
            }
        }
    }
}
