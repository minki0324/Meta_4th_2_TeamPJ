/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Minion_Controller_S : MonoBehaviour
{
    *//*
    미니언 컨트롤러
      1. 플레이어의 키 입력에 따른 모드변경
      2. 상태에 따라서 애니메이션 변경
  *//*

    public enum Type
    {
        //미니언 종류
        //숫자는 구매 가격 :)
        SwordMan = 15,
        HeavyInfantry = 20,
        Archer = 25,
    }


    private Ply_Controller playerController;
    private Animator ani;

    private CapsuleCollider MeshCollider;  //맞으면 피까이는 판정용 콜라이더
    private CapsuleCollider DetectCollider; //공격 판정용 콜라이더
    [SerializeField] private UnitAttack UnitAtk;

    private float MaxHP;
    public float CurrentHP { get; private set; }

    private float Damage;
    public bool isDead { get; private set; } = false;

    public Type Human_type;





    public bool isClose = false;







    private void Awake()
    {
        UnitAtk = GetComponent<UnitAttack>();
        playerController = FindObjectOfType<Ply_Controller>();
        MeshCollider = GetComponent<CapsuleCollider>();
        DetectCollider = transform.GetChild(0).GetComponent<CapsuleCollider>();
        TryGetComponent(out ani);
    }

    private void Start()
    {
        Get_HumanType();
    }

    private void Update()
    {
        Behavior_Mode();
        if (isClose == true)
        {
            ani.SetBool("Move", false);

        }
        else
        {

            ani.SetBool("Move", true);
        }
    }

    public void Get_HumanType()
    {
        int num = playerController.Human_num;
        switch (num)
        {
            case 1:
                Human_type = Type.SwordMan;
                MaxHP = 80f;
                Damage = 10f;
                Debug.Log("SwordMan");
                break;

            case 2:
                Human_type = Type.HeavyInfantry;
                MaxHP = 90f;
                Damage = 15f;
                Debug.Log("Infantry");
                break;

            case 3:
                Human_type = Type.Archer;
                MaxHP = 80f;
                Damage = 20f;
                Debug.Log("Archer");
                break;
        }


    }

    private void Behavior_Mode()
    {
        switch (playerController.CurrentMode)
        {
            case Ply_Controller.Mode.Follow:



                // 플레이어or대열의 앞 병사와 가까워졌을 때 체크하는 메소드 넣어서 가까워지면 Bool값 false로 변경, 멀어지면 다시 true로 변경해서 따라가기
                break;


            case Ply_Controller.Mode.Attack:
                // 상태에 따라서 애니메이션 구현해야함.
                *//*
                    1. 플레이어의 앞으로 이동할때는 Move를 true바꿔서 달려가는 모션
                    2. 적군이 사정거리 내에 들어왔을 때는 Move를 false로 바꾸고 하체는 Idle상태 상체는 쿨타임에따라 Attack Trigger를 켜서 공격 모션 취하도록
                *//*
                UnitAtk.MinionAttack();
                break;
        }
    }




}
*/