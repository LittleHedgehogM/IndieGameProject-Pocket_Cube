using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class Electro_VFX_ObjectPool : ObjectPool
{
    // callback

    public void PlayVFXAt(Transform aTransform)
    {
        GameObject aVFX = GetObjectFromPoolWithCallback(() =>
        {
            //Debug.Log("Special effect has finished playing.");
        });

        if (aVFX != null)
        {
            aVFX.SetActive(true);
            aVFX.transform.position = aTransform.position;
            aVFX.transform.rotation = aTransform.rotation;
            aVFX.GetComponent<ParticleSystem>().Play();

        }
    }

}
