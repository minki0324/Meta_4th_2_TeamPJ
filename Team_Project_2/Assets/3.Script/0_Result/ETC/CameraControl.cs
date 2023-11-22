using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    // 1. ī�޶� ���콺�� ���� �����¿�� ������
    // 2. ī�޶� �÷��̾ ���� ������ ������

    [SerializeField] Transform Player;

    private float Yaixs; // Y�� �߽ɼ�
    private float Xaixs; // X�� �߽ɼ�

    public float RotSen = 3f; // ȸ�� ����  (���߿� �ɼ�â���� ������ �� �ֵ��� public���� �ѰԿ�)
    private float Distance = 9f; // �÷��̾�� ī�޶� �Ÿ�
    private float RotMin = 10f;  // ȸ������ �ּ� 
    private float RotMax = 45f;  // ȸ������ �ִ�
    private float smoothTime = 0f; // ī�޶� �̵��� �ɸ��� �ð�
    
    
    private Vector3 targetRot; 
    private Vector3 currentVel; // ���� Velocity

    private void Awake()
    {
        // Cursor.visible = false;
        // Cursor.lockState = CursorLockMode.Locked;
        
    }

    private void Update()
    {
        if (!GameManager.instance.isLive)
        {
            return;
        }

        if(Player == null)
        {
            Player = GameObject.FindGameObjectWithTag("Player").GetComponent<Ply_Controller>().transform;
        }
    }
    private void LateUpdate()
    {
        if(!GameManager.instance.isLive)
        {
            return;
        }

        Yaixs += Input.GetAxis("Mouse X") * RotSen;  // ī�޶� �¿�
        Xaixs += -Input.GetAxis("Mouse Y") * RotSen; // ī�޶� ����

        if (Input.GetKey(KeyCode.B)) // �ɼ�â������ ��Ȱ���ϴ°ɷ�,,,
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.Confined;
            Yaixs -= Input.GetAxis("Mouse X") * RotSen;  // ī�޶� �¿� �������
            Xaixs -= -Input.GetAxis("Mouse Y") * RotSen; // ī�޶� ���� �������
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
