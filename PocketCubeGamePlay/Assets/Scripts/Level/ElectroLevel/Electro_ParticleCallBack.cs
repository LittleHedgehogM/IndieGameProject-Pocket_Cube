using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Electro_ParticleCallBack : MonoBehaviour
{
    private void OnParticleSystemStopped()
    {
        FindObjectOfType<Electro_VFX_ObjectPool>().ReturnObjectToPool(this.gameObject);
    }
}
