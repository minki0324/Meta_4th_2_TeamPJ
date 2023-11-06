using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ply_Movement_Fix : MonoBehaviour
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
    Camera camera_;
    [SerializeField] private Animator ani;
    [SerializeField] private Rigidbody rb;

    [Header("이동")]
    [SerializeField] private float MoveSpeed = 5f;

    [Header("점프")]
    [SerializeField] private float JumpForce = 10f;
    [SerializeField] private bool isGrounded = true;

    public bool isPlayerMove { get; private set; }

    public Vector3 CurrentPos { get; private set; }

    //추가된 변수-이서영

    public bool isAttacking_1 = false;  //공격모션 1이 실행중인가 판단
    public bool isPossible_Attack_2 = false;    //공격모션 2가 실행가능한 상태인가 (모션 1이 중간이상 실행되었는가) 판단

    public bool isAttacking_2 = false;   //공격모션 2이 실행중인가 판단
    public bool isPossible_Attack_1 = true;     //공격모션 1가 실행가능한 상태인가 (모션 1이 중간이상 실행되었는가) 판단

    public float groundCheckRadius = 0.2f;  // OverlapSphere 반지름
    public string groundTag = "Ground";  // 땅의 태그

    private Vector3 playerPosition;

    private float Min = -210f;
    private float Max = 210f;

    private void Start()
    {
        camera_ = Camera.main;
        isPossible_Attack_1 = true;
        playerPosition = gameObject.transform.position;
    }

    private void Update()
    {

   
        CurrentPos = transform.position;
        InputMovment();
        Jump();
        Ground_Check();

        if (Input.GetKeyDown(KeyCode.H))
        {

            if (isPossible_Attack_1)
            {
                //모션 1 실행 시작

                ani.SetTrigger("Attack");
                //ani.SetBool("Attack1", true);

                isAttacking_1 = true;   //실행중

                isAttacking_2 = false;
                isPossible_Attack_1 = false;
                isPossible_Attack_2 = false;

            }

            if (isPossible_Attack_2)
            {

                //ani.SetBool("ContinualAttack", true);
                ani.SetTrigger("Continual_Attack");

                isAttacking_2 = true;
                isPossible_Attack_2 = false;

                isAttacking_1 = false;
                isPossible_Attack_1 = false;

            }
            // isLive = false;
        }


        if (Input.GetMouseButton(1))
        {
            ani.SetBool("Shield", true);
            ani.SetFloat("MoveSpeed", 0.5f);
            MoveSpeed = 2f;
        }
        else
        {
            ani.SetBool("Shield", false);
            ani.SetFloat("MoveSpeed", 1f);
            MoveSpeed = 5f;
        }
    

    }





    private void InputMovment()
    {
        if (!GameManager.instance.isLive)
        {
            return;
        }

        Vector3 forward = transform.TransformDirection(Vector3.forward);

        Vector3 right = transform.TransformDirection(Vector3.right);

        Vector3 playerRotate = Vector3.Scale(camera_.transform.forward, new Vector3(1, 0, 1));

        Vector3 moveDirection = playerRotate * Input.GetAxis("Vertical") + camera_.transform.right * Input.GetAxis("Horizontal");


        if (moveDirection != Vector3.zero)
        {
            // 회전
            transform.rotation = Quaternion.LookRotation(moveDirection);

            // 이동
            transform.position += (moveDirection.normalized * MoveSpeed * Time.deltaTime);
            transform.position = new Vector3(
Mathf.Clamp(transform.position.x, Min, Max),
transform.position.y,
Mathf.Clamp(transform.position.z, Min, Max));
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
        if (!GameManager.instance.isLive)
        {
            return;
        }

        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            rb.AddForce(Vector3.up * JumpForce);
            // Ply_rb.AddForce(Vector3.up * JumpForce);
        }
    }

    public void Check_Ground()
    {
        if (!GameManager.instance.isLive)
        {
            return;
        }

        RaycastHit hit;
        Vector3 HitPos = new Vector3(transform.position.x, transform.position.y, transform.position.z);

        Debug.DrawRay(HitPos, Vector3.down, Color.red, 1.1f);
        if (Physics.Raycast(HitPos, Vector3.down, out hit, 1.1f))
        {
            if (hit.transform.CompareTag("Ground"))
            {
                isGrounded = true;
                return;
            }
        }
        isGrounded = false;
    }
    private void Ground_Check()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, groundCheckRadius);

        foreach (Collider col in colliders)
        {
            if (col.CompareTag(groundTag))
            {
                // 땅 태그를 가진 오브젝트와 충돌한 경우
                isGrounded = true;
                break;
            }
            else
            {
                isGrounded = false;
            }
        }

        if (isGrounded)
        {
            // 캐릭터는 땅에 닿아 있음
            // 여기에서 점프를 허용하거나 다른 동작을 수행할 수 있음
        }
    }
}