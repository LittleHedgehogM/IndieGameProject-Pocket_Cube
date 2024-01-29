using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetAchivement : MonoBehaviour
{
    public void ResetAch()
    {
        List<bool> r = new List<bool>();
        for (int i = 1; i < 8; i++)
        {
            var cal = new Steamworks.Data.Achievement($"ACHIEVEMENT_0{i}");
            cal.Clear();
        }
    }
}
