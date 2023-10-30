using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ply_Movement : MonoBehaviour
{
    /*
        1. 이동 구현
        2. 점프 구현
        3. 애니메이션 동작 구현

        변수

        카메라
        애니메이션
        리지드바디
        속도
        점프힘
        생사 유무 체크
        점프 상태인지 체크
    */
    Camera camera;
    [SerializeField] private Animator ani;
    [SerializeField] private Rigidbody rb;

    [Header("이동")]
    [SerializeField] private float MoveSpeed = 5f;

    [Header("점프")]
    [SerializeField] private float JumpForce = 10f;
    [SerializeField] private bool isGrounded = true;

    private void Start()
    {
        ani = GetComponent<Animator>();
        camera = Camera.main;
    }

    private void Update()
    {
        InputMovment();
        Jump();
        Check_Ground();

        if(Input.GetKeyDown(KeyCode.H))
        {
            ani.SetTrigger("Attack");
            // isLive = false;
        }
    }

    private void InputMovment()
    {
        if(!GameManager.instance.isLive)
        {
            return;
        }

        Vector3 forward = transform.TransformDirection(Vector3.forward);

        Vector3 right = transform.TransformDirection(Vector3.right);

        Vector3 playerRotate = Vector3.Scale(camera.transform.forward, new Vector3(1, 0, 1));

        Vector3 moveDirection = playerRotate * Input.GetAxis("Vertical") + camera.transform.right * Input.GetAxis("Horizontal");
        

        if (moveDirection != Vector3.zero)
        {
            // 회전
            transform.rotation = Quaternion.LookRotation(moveDirection);

            // 이동
            transform.position += (moveDirection.normalized * MoveSpeed * Time.deltaTime);

            ani.SetBool("Move", true);
            ani.SetBool("Idle", false);
        }
        else
        {
            ani.SetBool("Idle", true);
            ani.SetBool("Move", false);
        }
    }

    private void Jump()
    {
        if (!GameManager.instance.isLive)
        {
            return;
        }

        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            rb.AddForce(Vector3.up * JumpForce);
        }
    }

    public void Check_Ground()
    {
        if (!GameManager.instance.isLive)
        {
            return;
        }

        RaycastHit hit;
        Vector3 HitPos = new Vector3(transform.position.x, transform.position.y - 1, transform.position.z);

        if (Physics.Raycast(HitPos, Vector3.down, out hit, 1.1f))
        {
            Debug.DrawRay(HitPos, Vector3.down, Color.red, 1.1f);
            if (hit.transform.CompareTag("Ground"))
            {
                isGrounded = true;
                return;
            }
        }
        isGrounded = false;
    }
}
