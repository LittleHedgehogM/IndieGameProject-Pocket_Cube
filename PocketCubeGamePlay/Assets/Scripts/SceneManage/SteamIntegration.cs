using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


public class SteamIntegration : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        try
        {
            Steamworks.SteamClient.Init(2682570);
            PrintYourName();

        }
        catch (System.Exception e)
        {
            Debug.Log(e);
        }
    }

    // Update is called once per frame
    private void PrintYourName()
    {
        Debug.Log(Steamworks.SteamClient.Name);
    }

    void OnApplicationQuit()
    {
        Steamworks.SteamClient.Shutdown();
    }

    private void OnEnable()
    {
        EndingVideo.ACHIEVEMENT_01 += AchievementTrigger; 
        CubePlayManager.OnlyUseSkillsToSolve += OnlyUseSkillsToSolveAction; //02
        CubePlayUIController.ACHIEVEMENT_03 += AchievementTrigger;
        CubePlayManager.UnsolveCubeAfterEightMinutes += UnsolveCubeAfterEightMinutesAction; //04
        CubePlayManager.SolveCubeWithinOneMins += SolveCubeWithinOneMinsAction; //05
        EndingVideo.ACHIEVEMENT_06 += AchievementTrigger; 
        CubePlayManager.RestartCubeGame += RestartCubeGameAction; //07
   
    }

    private void OnlyUseSkillsToSolveAction()
    {
        AchievementTrigger("ACHIEVEMENT_02");
    }

    private void UnsolveCubeAfterEightMinutesAction()
    {
        AchievementTrigger("ACHIEVEMENT_04");
    }

    private void SolveCubeWithinOneMinsAction()
    {
        AchievementTrigger("ACHIEVEMENT_05");
    }

    private void RestartCubeGameAction()
    {
        AchievementTrigger("ACHIEVEMENT_07");
    }

    private void AchievementTrigger(string api)
    {
        var ach = new Steamworks.Data.Achievement(api);
        ach.Trigger();
        List<bool> result = new List<bool>();
        for (int i = 1; i < 8; i++)
        {
            var cal = new Steamworks.Data.Achievement($"ACHIEVEMENT_0{i}");
            result.Add(cal.State);
            Debug.Log(cal.State);
        }
        if (!result.Contains(false))
        {
            var finalAch = new Steamworks.Data.Achievement("ACHIEVEMENT_08");
            finalAch.Trigger();
        }
        if (!ach.State)
        {
            StartCoroutine(AchivementsCheck(api));
        }
        
    }

    IEnumerator AchivementsCheck(string api)
    {
        var ach = new Steamworks.Data.Achievement(api);
        if (ach.State)
        {
            yield break;
        }
        else
        {
            AchievementTrigger(api);
        }
        yield return null;
    }


}
