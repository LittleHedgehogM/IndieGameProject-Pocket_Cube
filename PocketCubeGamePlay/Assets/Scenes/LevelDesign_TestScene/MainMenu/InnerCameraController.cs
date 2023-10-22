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
    // 计算旋转
    private Quaternion rotation, targetRotation;
    // 计算距离
    private float distance, targetDistance;
    // 默认距离
    private const float default_distance = 5f;
    // y轴旋转范围
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
        // 初始位置是模型
        targetPosition = model.position;
        // 初始镜头拉伸
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
                // 获取摄像机欧拉角
                Vector3 angles = transform.rotation.eulerAngles;
                // 欧拉角表示按照坐标顺序旋转，比如angles.x=30，表示按x轴旋转30°，dy改变引起x轴的变化
                angles.x = Mathf.Repeat(angles.x + 180f, 360f) - 180f;
                angles.y += dx;
                angles.x -= dy;
                angles.x = ClampAngle(angles.x, min_angle_y, max_angle_y);
                // 计算摄像头旋转
                targetRotation.eulerAngles = new Vector3(angles.x, angles.y, 0);
                // 随着旋转，摄像头位置自动恢复
                Vector3 temp_position =
                        Vector3.Lerp(targetPosition, model.position, Time.deltaTime * moveLerp);
                targetPosition = Vector3.Lerp(targetPosition, temp_position, Time.deltaTime * moveLerp);
            }
        }
        targetDistance -= Input.GetAxis("Mouse ScrollWheel") * zoomSpeed;
    }
    float ClampAngle(float angle, float min, float max)
    {
        // 控制旋转角度不超过360
        if (angle < -360f) angle += 360f;
        if (angle > 360f) angle -= 360f;
        return Mathf.Clamp(angle, min, max);
    }
    private void FixedUpdate()
    {
        rotation = Quaternion.Slerp(rotation, targetRotation, Time.deltaTime * rotateLerp);
        position = Vector3.Lerp(position, targetPosition, Time.deltaTime * moveLerp);
        distance = Mathf.Lerp(distance, targetDistance, Time.deltaTime * zoomLerp);
        // 设置摄像头旋转
        transform.rotation = rotation;
        // 设置摄像头位置
        transform.position = position - rotation * new Vector3(0, 0, default_distance);
    }
}
