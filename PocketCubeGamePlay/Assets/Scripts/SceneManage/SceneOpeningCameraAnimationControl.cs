using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneOpeningCameraAnimationControl : MonoBehaviour
{
    [SerializeField] Animator ani;
    AnimatorStateInfo stateinfo;
   

    private void Update()
    {
        stateinfo = ani.GetCurrentAnimatorStateInfo(0);
        if (!stateinfo.IsName("SceneOpeningCamera"))
        {
            gameObject.SetActive(false);
        }
    }
}
