using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FourierKeyCube : MonoBehaviour
{
    [SerializeField] private FourierColorChanger Level1;
    [SerializeField] private FourierColorChanger Level2;
    [SerializeField] private FourierColorChanger Level3;
    [SerializeField] private FourierColorChanger Level4;
    [SerializeField] private FourierColorChanger Level5;

    [SerializeField] private Vector3 _rotation;
    [SerializeField] private float _speed;

    [SerializeField] private bool levelPass = false;

    [SerializeField] private ParticleSystem cubeFx;
    //[SerializeField] private ParticleSystem cubeLevelPassFx;
    //private bool cubeLevelPassFxReady = true;
    private bool played = false;
    // Update is called once per frame
    void Update()
    {
        
        if (Level4.isLevelPass & Level5.isLevelPass || levelPass)
        {
            CubeAutoRotate();
            
        }
    }

    private void CubeAutoRotate()
    {
        //transform.Rotate(_rotation * _speed * Time.deltaTime);
        transform.Rotate(_rotation * _speed * Time.deltaTime, Space.World);
        if (!played)
        {
            cubeFx.Play();
            played = true;
        }

        
    }

    private void OnMouseUp()
    {
        if (Level4.isLevelPass & Level5.isLevelPass || levelPass)
        {
            FindObjectOfType<LevelLoaderScript>().LoadNextLevel();
        }
        
    }

    public void easyPass()
    {
        levelPass = true;
    }
}