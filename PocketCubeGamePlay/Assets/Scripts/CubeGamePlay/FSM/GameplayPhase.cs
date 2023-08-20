using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class GameplayPhase : MonoBehaviour
{

    public abstract void onStart();

    // return true if is finished
    public abstract bool onUpdate();

    public abstract void onEnd();

    public virtual void Pause()
    {

    }

    public virtual void onResume()
    {

    }

    public virtual void onRestart()
    {

    }

}