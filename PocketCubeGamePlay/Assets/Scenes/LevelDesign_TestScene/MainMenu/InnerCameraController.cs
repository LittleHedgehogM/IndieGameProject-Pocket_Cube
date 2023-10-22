using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InnerCameraController : MonoBehaviour
{
    public Transform model;
    public float rotateSpeed = 32f;
    public float rotateLerp = 8;
    public float moveSpeed = 1f;
    public float moveLerp = 10f;
    public float zoomSpeed = 10f;
    public float zoomLerp = 4f;
    // calculate move
    private Vector3 position, targetPosition;
    // ������ת
    private Quaternion rotation, targetRotation;
    // �������
    private float distance, targetDistance;
    // Ĭ�Ͼ���
    private const float default_distance = 5f;
    // y����ת��Χ
    private const float min_angle_y = -89f;
    private const float max_angle_y = 89f;

    // Camera transpose positions
    Vector3 Position_01 = new Vector3(0, -10, 0);
    Vector3 Position_02 = new Vector3(0, 0, 20);
    //Vector3 Position_03 = new Vector3(-10, 0, 10);

    Quaternion Rotation_01 = new Quaternion(-90, 0, 0, 1);


    // Start is called before the first frame update
    void Start()
    {
        //init at zero
        targetRotation = Quaternion.identity;
        // ��ʼλ����ģ��
        targetPosition = model.position;
        // ��ʼ��ͷ����
        targetDistance = default_distance;

    }

    // Update is called once per frame
    void Update()
    {
        float dx = Input.GetAxis("Mouse X");
        float dy = Input.GetAxis("Mouse Y");
        // mouse right input
        float d_target_distance = targetDistance;
        if (d_target_distance < 2f)
        {
            d_target_distance = 2f;
        }
        //if (Input.GetKeyDown("space"))
        //{
            //mainCam.transform.rotation = Rotation_01;
            //this.position = Vector3.Lerp(Position_01, model.position, Time.deltaTime * moveLerp);  
            //this.position = Position_01;
            //Debug.Log(model.position);
        //}
        if (Input.GetMouseButton(0))
        {
            dx *= moveSpeed * d_target_distance / default_distance;
            dy *= moveSpeed * d_target_distance / default_distance;
            targetPosition -= transform.up * dy + transform.right * dx;
        }
        if (Input.GetMouseButton(1))
        {
            dx *= rotateSpeed;
            dy *= rotateSpeed;
            if (Mathf.Abs(dx) > 0 || Mathf.Abs(dy) > 0)
            {
                // ��ȡ�����ŷ����
                Vector3 angles = transform.rotation.eulerAngles;
                // ŷ���Ǳ�ʾ��������˳����ת������angles.x=30����ʾ��x����ת30�㣬dy�ı�����x��ı仯
                angles.x = Mathf.Repeat(angles.x + 180f, 360f) - 180f;
                angles.y += dx;
                angles.x -= dy;
                angles.x = ClampAngle(angles.x, min_angle_y, max_angle_y);
                // ��������ͷ��ת
                targetRotation.eulerAngles = new Vector3(angles.x, angles.y, 0);
                // ������ת������ͷλ���Զ��ָ�
                Vector3 temp_position =
                        Vector3.Lerp(targetPosition, model.position, Time.deltaTime * moveLerp);
                targetPosition = Vector3.Lerp(targetPosition, temp_position, Time.deltaTime * moveLerp);
            }
        }
        targetDistance -= Input.GetAxis("Mouse ScrollWheel") * zoomSpeed;
    }
    float ClampAngle(float angle, float min, float max)
    {
        // ������ת�ǶȲ�����360
        if (angle < -360f) angle += 360f;
        if (angle > 360f) angle -= 360f;
        return Mathf.Clamp(angle, min, max);
    }
    private void FixedUpdate()
    {
        rotation = Quaternion.Slerp(rotation, targetRotation, Time.deltaTime * rotateLerp);
        position = Vector3.Lerp(position, targetPosition, Time.deltaTime * moveLerp);
        distance = Mathf.Lerp(distance, targetDistance, Time.deltaTime * zoomLerp);
        // ��������ͷ��ת
        transform.rotation = rotation;
        // ��������ͷλ��
        transform.position = position - rotation * new Vector3(0, 0, default_distance);
    }
}
