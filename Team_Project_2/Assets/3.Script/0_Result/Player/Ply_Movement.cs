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
    Camera camera_;
    [SerializeField] private Animator ani;
    [SerializeField] private Rigidbody rb;

    [Header("이동")]
    public float MoveSpeed = 10f ;
    public float speed;
    public float realSpeed;
    [Header("점프")]
    [SerializeField] private float JumpForce = 10f;
    [SerializeField] private bool isGrounded = true;
    public float rotationSpeed = 3.0f;
    public bool isPlayerMove { get; private set; }
    public Vector3 MoveDir = Vector3.zero;
    public Vector3 CurrentPos { get; private set; }

    public bool isAttacking_1 = false;  //공격모션 1이 실행중인가 판단
    public bool isPossible_Attack_2 = false;    //공격모션 2가 실행가능한 상태인가 (모션 1이 중간이상 실행되었는가) 판단

    public bool isAttacking_2 = false;   //공격모션 2이 실행중인가 판단
    public bool isPossible_Attack_1 = true;     //공격모션 1가 실행가능한 상태인가 (모션 1이 중간이상 실행되었는가) 판단

    public float groundCheckRadius = 0.2f;  // OverlapSphere 반지름
    public string groundTag = "Ground";  // 땅의 태그


    public bool holdingShield; //우클릭 실드들때
    private bool isMove; 

    private Vector3 playerPosition;

    private float Min = -210f;
    private float Max = 210f;
    Vector3 playerRotate;


    public Quaternion playerRotation { get; private set; }   //added


    private void Start()
    {
        camera_ = Camera.main;
        isPossible_Attack_1 = true;
        playerPosition = gameObject.transform.position;

    }

    private void Update()
    {
        CurrentPos = transform.position;
     

        if (holdingShield)
        {
            speed -= 1f * Time.deltaTime;
            speed = Mathf.Clamp(speed, 0.3f, 1f);
            
        }
        else
        {
            if (isMove)
            {
                speed += 1f * Time.deltaTime;
            }
            else
            {
                speed -= 1f * Time.deltaTime;
            }
            speed = Mathf.Clamp01(speed);
        }
        ani.SetBool("Shield", holdingShield);
        ani.SetFloat("Speed", speed);
        ani.SetBool("Move", isMove);
        realSpeed = MoveSpeed * speed *Time.deltaTime;
        InputMovment(MoveSpeed);
        Jump();
        Ground_Check();

        if (Input.GetMouseButton(0))
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

            holdingShield = true;

            Vector3 cameraForward = Camera.main.transform.forward;
            cameraForward.y = 0f; // 회전을 수평 평면에만 유지
            Quaternion targetRotation = Quaternion.LookRotation(cameraForward);
            transform.rotation = targetRotation;

        }
        else
        {
            holdingShield = false;
        }


    }





    private void InputMovment(float MoveSpeed)
    {
        if (!GameManager.instance.isLive)
        {
            return;
        }



        // 목표 플레이어 회전 계산
        Vector3 cameraForward = Camera.main.transform.forward;
        cameraForward.y = 0f; // 수평 평면에만 유지
        Vector3 targetPlayerRotate = Vector3.Scale(cameraForward, new Vector3(1, 0, 1));

        // Vector3.Slerp를 사용하여 자연스럽게 회전 보간

        // 수평 및 수직 입력 값
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");
        //if (horizontalInput == 1)
        //{
        //    RotateToDirection(Camera.main.transform.right);
        //}
        //else if (horizontalInput == -1)
        //{
        //    RotateToDirection(-Camera.main.transform.right);
        //}
        //else if (verticalInput == 1)
        //{
        //    RotateToDirection(Camera.main.transform.forward);
        //}
        //else if (verticalInput == -1)
        //{
        //    RotateToDirection(-Camera.main.transform.forward);
        //}
        // 현재 회전에서 목표 회전까지 특정 각속도로 즉시 회전

        //Vector3 rightDir = Camera.main.transform.right * horizontalInput;
        //Vector3 forwardDir = Camera.main.transform.forward * verticalInput;
        //Vector3 diagonalDir = rightDir + forwardDir;
        //Quaternion rotation = Quaternion.LookRotation(diagonalDir);
        //transform.rotation = Quaternion.Slerp(transform.rotation, rotation ,rotationSpeed *Time.deltaTime);
        //Quaternion targetRotation = Quaternion.LookRotation(diagonalDir);
        //Quaternion currentRotation = transform.rotation;
        //transform.rotation = Quaternion.Slerp(currentRotation, targetRotation, rotationSpeed * Time.deltaTime);
        //// 움직임 방향 계산
        ///
        MoveDir = targetPlayerRotate * verticalInput + Camera.main.transform.right * horizontalInput;

        // 목표 회전 방향 계산

        // Slerp를 사용하여 부드러운 회전 적용

        //MoveDir = playerRotate * Input.GetAxis("Vertical") + camera_.transform.right * Input.GetAxis("Horizontal");
        //playerRotate = Vector3.Slerp(targetPlayerRotate, MoveDir, rotationSpeed * Time.deltaTime);



        if (MoveDir != Vector3.zero)
        {
            if (Input.GetMouseButton(1))
            {
                transform.rotation = Quaternion.LookRotation(playerRotate);
            }
            else
            {
               transform.rotation = Quaternion.LookRotation(MoveDir);

            }
            // 회전
            playerRotation = transform.rotation;    //added

            // 이동
            transform.position += (MoveDir.normalized * realSpeed /** Time.deltaTime*/);
            isMove = true;
            isPlayerMove = true;
        }
        else
        {
            isMove = false;
            isPlayerMove = false;
        }
    }
    private void RotateToDirection(Vector3 targetDirection)
    {
        Quaternion targetRotation = Quaternion.LookRotation(targetDirection, Vector3.up);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
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




    public Vector3 GetDirection(Vector3 standard, Vector3 forward, string Direction)
    {
        Vector3 Forward = new Vector3();
        Vector3 Backward = new Vector3(); 
        Vector3 Right = new Vector3();
        Vector3 Left = new Vector3();


        if (standard.z < forward.z)
        {
            Forward = standard + new Vector3(0f, 0f, 1f);         
            Backward = standard - new Vector3(0f, 0f, 1f);
            Left = standard - new Vector3(1f, 0f, 0f);
            Right = standard + new Vector3(1f, 0f, 0f);
        }
        if (standard.x < forward.x)
        {
            Forward = standard + new Vector3(1f, 0f, 0f);   
            Backward = standard - new Vector3(1f, 0f, 0f);
            Left = standard + new Vector3(0f, 0f, 1f);
            Right = standard - new Vector3(0f, 0f, 1f);

        }
        if(standard.z > forward.z)
        {
            Forward = standard - new Vector3(0f, 0f, 1f);
            Backward = standard + new Vector3(0f, 0f, 1f);
            Left = standard + new Vector3(1f, 0f, 0f);
            Right = standard - new Vector3(1f, 0f, 0f);
        }
        if(standard.x > forward.x)
        {
            Forward = standard - new Vector3(1f, 0f, 0f);
            Backward = standard + new Vector3(1f, 0f, 0f);
            Left = standard - new Vector3(0f, 0f, 1f);
            Right = standard + new Vector3(0f, 0f, 1f);
        }
        


        switch(Direction)
        {
            case "Forward":
                return Forward;
                

            case "Backward":
                return Backward;

            case "Left":
                return Left;
               

            case "Right":
                return Right;

            default:
                return new Vector3(0, 0, 0);
        }

    }



}