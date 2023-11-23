using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class CubeVFXManager : MonoBehaviour
{

    [SerializeField] private GameObject finish_VFX;
    [SerializeField] private GameObject skill_VFX;

    private void Start()
    {
        finish_VFX.SetActive(false);    
        skill_VFX.SetActive(false);
    }

    public void PlayFinishVFX()
    {
        finish_VFX.SetActive(true);
        // finish cube audio
        AkSoundEngine.PostEvent("Play_cube_final", gameObject);

        finish_VFX.transform.parent = null;
        finish_VFX.transform.position = Vector3.zero;
        finish_VFX.GetComponent<ParticleSystem>().Play();
        
    }

    public void PlaySkillVFX()
    {
        skill_VFX.SetActive(true);
        skill_VFX.transform.parent = null;
        skill_VFX.transform.position = Vector3.zero;
        skill_VFX.GetComponent<ParticleSystem>().Play();
       // var main = skill_VFX.GetComponent<ParticleSystem>().main;
        //main.loop = false;
    }

    public void StopSkillVFX()
    {

    }
}
