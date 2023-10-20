using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrownRotate : MonoBehaviour
{
    [SerializeField] private Vector3 _rotation;
    [SerializeField] private float _speed;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        CubeAutoRotate();
    }

    private void CubeAutoRotate()
    {
        //transform.Rotate(_rotation * _speed * Time.deltaTime);
        transform.Rotate(_rotation*_speed*Time.deltaTime,Space.World);
    }
}
