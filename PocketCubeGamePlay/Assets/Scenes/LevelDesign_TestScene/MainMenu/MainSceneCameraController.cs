using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainSceneCameraController : MonoBehaviour
{
    [SerializeField] private Camera mainCam;
    [SerializeField] private Object BigCube;
    Vector3 initialPosition, LookAtCentralPosition;
    Vector3 LookAtDleta = new Vector3(-1.22f, 0.17f, -0.31f);
    Vector3 Position_01 = new Vector3(0, -10, 0);
    Vector3 Position_02 = new Vector3(0, 0, 20);
    public float rotateLerp = 8;
    //Vector3 Position_03 = new Vector3(-10, 0, 10);
    private int holder_counter = 3;
    private float default_distance = 15f;
    private Quaternion rotation, targetRotation;
    //model central point
    private Vector3[] CameraPositions = new Vector3[]{
        new Vector3(1, 0, 0),
        new Vector3(0, 0, 1),
        new Vector3(-1, 0, 0),
        new Vector3(0, 0, -1)
    };

    private Vector3[] CameraRotationAngle = new Vector3[]{
        new Vector3(0, -90f, 0),
        new Vector3(0, -180f, 0),
        new Vector3(0, 90f, 0),
        new Vector3(0, 0, 0)
    };
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (keyboardControlDetect())
        {
            UpdateCameraPosition();
        }
    }

    bool keyboardControlDetect()
    {
        if (Input.GetKeyDown("left"))
        {
            holder_counter -= 1;
            return true;
        }
        if (Input.GetKeyDown("right"))
        {
            holder_counter += 1;
            return true;
        }
        if (Input.GetKeyDown("space"))
        {
            default_distance = 8;
            // Enter Scene; Scene Close
            return true;
        }
        if (Input.GetKeyDown("esc"))
        {
            default_distance = 16;
            return true;
        }
        return false;
    }
    void UpdateCameraPosition()
    {
        mainCam.transform.position = default_distance * CameraPositions[Mathf.Abs(holder_counter) % (CameraPositions.Length)];
        // mainCam.transform.rotation = CameraRotations[Mathf.Abs(holder_counter) % (CameraRotations.Length)];
        targetRotation.eulerAngles = (CameraRotationAngle[Mathf.Abs(holder_counter) % (CameraRotationAngle.Length)]);
        mainCam.transform.rotation = Quaternion.Euler(targetRotation.eulerAngles);
        rotation = mainCam.transform.rotation;

        Debug.Log(holder_counter % (CameraPositions.Length));
        Debug.Log(mainCam.transform.position);
        Debug.Log(mainCam.transform.rotation);
    }
}
