using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Origin_VFXController : MonoBehaviour
{
    [SerializeField] GameObject rotationVFX;
    [SerializeField] GameObject cubeVFX;


    private void Start()
    {
        rotationVFX.GetComponent<ParticleSystem>().Stop();
        cubeVFX.GetComponent<ParticleSystem>().Stop();
    }

    public void playRotationVFXAt(Transform transform)
    {
        playVFX(rotationVFX, transform);
        
    }

    public void playCubeFinishVFXAt(Transform transform)
    {
        GameObject finishVFX =  Instantiate(cubeVFX, transform.position, transform.rotation);
        finishVFX.GetComponent<ParticleSystem>().Play();
    }

    private void playVFX(GameObject vfx, Transform transform)
    {
        vfx.SetActive(true);
        vfx.transform.parent = transform;
        vfx.transform.localPosition = Vector3.zero;
        vfx.GetComponent<ParticleSystem>().Play();
    }

}
