using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class CubeVFXManager : MonoBehaviour
{

    [SerializeField] private GameObject finish_VFX;
    [SerializeField] private GameObject skill_VFX;
    private GameObject finish_VFX_Instance = null;
    private GameObject skill_VFX_Instance = null;

    public void PlayFinishVFX()
    {
        if (finish_VFX_Instance == null)
        {
            finish_VFX_Instance = Instantiate(finish_VFX, Vector3.zero, finish_VFX.transform.rotation); 
            finish_VFX_Instance.transform.SetParent(null);
            finish_VFX_Instance.GetComponent<ParticleSystem>().Play();
        }
        
    }

    public void PlaySkillVFX()
    {
        if (skill_VFX_Instance == null)
        {
            skill_VFX_Instance = Instantiate(skill_VFX, Vector3.zero, skill_VFX.transform.rotation);
            skill_VFX_Instance.transform.SetParent(null);
            skill_VFX_Instance.GetComponent<ParticleSystem>().Play();
            
        }

        var main = skill_VFX_Instance.GetComponent<ParticleSystem>().main;
        main.loop = false;
    }

    public void StopSkillVFX()
    {
        if (skill_VFX_Instance != null)
        {
            skill_VFX_Instance.GetComponent<ParticleSystem>().Stop();
            skill_VFX_Instance = null;
        }

    }
}
