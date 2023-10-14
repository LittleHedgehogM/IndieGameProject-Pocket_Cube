using System;
using UnityEngine;

public class Electro_CollideChecker : MonoBehaviour
{
    [System.Serializable]
    public enum RangeTag
    {
        Star,
        Moon,
        Sun
    }

    [SerializeField]
    public RangeTag myTag;

    public static event Action EnterStarRange;
    public static event Action LeaveStarRange;

    public static event Action EnterSunRange;
    public static event Action LeaveSunRange;

    public static event Action EnterMoonRange;
    public static event Action LeaveMoonRange;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            switch (myTag)
            {
                case RangeTag.Star:
                {
                    EnterStarRange?.Invoke();
                    break;
                }
                case RangeTag.Moon:
                {
                    EnterMoonRange?.Invoke();
                    break;
                }
                case RangeTag.Sun:
                {
                    EnterSunRange?.Invoke();
                    break;
                }
            }
            
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            switch (myTag)
            {
                case RangeTag.Star:
                    {
                        LeaveStarRange?.Invoke();
                        break;
                    }
                case RangeTag.Moon:
                    {
                        LeaveMoonRange?.Invoke();
                        break;
                    }
                case RangeTag.Sun:
                    {
                        LeaveSunRange?.Invoke();
                        break;
                    }
            }
        }
    }
   
}
