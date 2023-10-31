using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    // 1. 카메라가 마우스를 따라서 상하좌우로 움직임
    // 2. 카메라가 플레이어를 따라서 시점이 움직임

    [SerializeField] Transform Player;

    private float Yaixs; // Y축 중심선
    private float Xaixs; // X축 중심선

    public float RotSen = 3f; // 회전 감도  (나중에 옵션창에서 접근할 수 있도록 public으로 둘게용)
    private float Distance = 9f; // 플레이어와 카메라 거리
    private float RotMin = 10f;  // 회전각도 최소 
    private float RotMax = 45f;  // 회전각도 최대
    private float smoothTime = 0f; // 카메라 이동에 걸리는 시간

    private Vector3 targetRot; 
    private Vector3 currentVel; // 현재 Velocity

    private void Awake()
    {
        // Cursor.visible = false;
        // Cursor.lockState = CursorLockMode.Locked;
        
    }
    private void LateUpdate()
    {
        Yaixs += Input.GetAxis("Mouse X") * RotSen;  // 카메라 좌우
        Xaixs += -Input.GetAxis("Mouse Y") * RotSen; // 카메라 상하

        if (Input.GetKey(KeyCode.B)) // 옵션창에서로 재활용하는걸루,,,
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.Confined;
            Yaixs -= Input.GetAxis("Mouse X") * RotSen;  // 카메라 좌우 원래대로
            Xaixs -= -Input.GetAxis("Mouse Y") * RotSen; // 카메라 상하 원래대로
        }
        else
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }

        Xaixs = Mathf.Clamp(Xaixs, RotMin, RotMax);

        targetRot = Vector3.SmoothDamp(targetRot, new Vector3(Xaixs, Yaixs, 0), ref currentVel, smoothTime);
        transform.eulerAngles = targetRot;

        transform.position = Player.position - transform.forward * Distance;

    }









}
