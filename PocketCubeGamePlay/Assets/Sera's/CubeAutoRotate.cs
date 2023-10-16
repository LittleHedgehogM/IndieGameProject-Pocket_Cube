using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeAutoRotate : MonoBehaviour
{
    [SerializeField] private Vector3 _rotation;
    [SerializeField] private float _speed;


    // Update is called once per frame
     
    private void Update()
    {
        StartMenuCubeAutoRotate();
    }

    public void StartMenuCubeAutoRotate()
    {
        transform.Rotate(_rotation * _speed * Time.deltaTime);
    }
}
