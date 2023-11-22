using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FourierSceneTutorialController : SceneTutorialController
{

    //public static Action FourierTutorialEnds;

    private void OnEnable()
    {
        FourierAniCtl.PerformCameraFinished += TutorialStarts;
    }


    private void OnDisable()
    {
        FourierAniCtl.PerformCameraFinished -= TutorialStarts;
    }

   

}
