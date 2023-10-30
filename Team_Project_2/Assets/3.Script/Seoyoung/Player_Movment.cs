using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Movment : MonoBehaviour
{

    /*
        1. �̵� ����
        2. ���� ����
        3. �ִϸ��̼� ���� ����

        ����

        ī�޶�
        �ִϸ��̼�
        ������ٵ�
        �ӵ�
        ������
        ���� ���� üũ
        ���� �������� üũ
    */
    Camera camera;
    private Animator ani;
    [SerializeField] private Rigidbody rb;

    [Header("�̵�")]
    [SerializeField] private float MoveSpeed = 5f;

    [Header("����")]
    [SerializeField] private float JumpForce = 10f;
    [SerializeField] private bool isGrounded = true;
    [SerializeField] private bool isLive = true;

    //�߰��� ����
    public bool isPlayerMove { get; private set; }

    public Vector3 CurrentPos { get; private set; }


    private void Start()
    {
        ani = GetComponent<Animator>();
        camera = FindObjectOfType<Camera>();
    }

    private void Update()
    {
        CurrentPos = transform.position;
        InputMovment();
        Jump();
        Check_Ground();

        if (Input.GetKeyDown(KeyCode.H))
        {
            ani.SetTrigger("Hit");
            // isLive = false;
        }
    }

    private void InputMovment()
    {
        if (!isLive)
        {
            return;
        }

        Vector3 forward = transform.TransformDirection(Vector3.forward);

        Vector3 right = transform.TransformDirection(Vector3.right);

        Vector3 playerRotate = Vector3.Scale(camera.transform.forward, new Vector3(1, 0, 1));

        Vector3 moveDirection = playerRotate * Input.GetAxis("Vertical") + camera.transform.right * Input.GetAxis("Horizontal");


        if (moveDirection != Vector3.zero)
        {
            // ȸ��
            transform.rotation = Quaternion.LookRotation(moveDirection);

            // �̵�
            transform.position += (moveDirection.normalized * MoveSpeed * Time.deltaTime);

            ani.SetBool("Move", true);
            ani.SetBool("Idle", false);
            isPlayerMove = true;
        }
        else
        {
            ani.SetBool("Idle", true);
            ani.SetBool("Move", false);
            isPlayerMove = false;
        }
    }

    private void Jump()
    {
        if (!isLive)
        {
            return;
        }

        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            rb.AddForce(Vector3.up * JumpForce);
        }
    }

    private void Check_Ground()
    {
        if (!isLive)
        {
            return;
        }

        RaycastHit hit;

        if (Physics.Raycast(transform.position, Vector3.down, out hit, 0.1f))
        {
            Debug.DrawRay(transform.position, Vector3.down, Color.red, 0.1f);
            if (hit.transform.CompareTag("Ground"))
            {
                isGrounded = true;
                return;
            }
        }
        isGrounded = false;
    }
}