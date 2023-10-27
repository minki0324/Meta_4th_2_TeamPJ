using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Minion_Controller : MonoBehaviour
{
    public enum Type
    {
       //숫자는 구매 가격 :)
        SwordMan = 15,
        HeavyInfantry = 20,
        Archer = 25,
    }

    private Player_Controller playerController;
    private Follow_Player followPlayer;


    private CapsuleCollider MeshCollider;  //맞으면 피까이는 판정용 콜라이더
    private CapsuleCollider DetectCollider; //공격 판정용 콜라이더

    private float MaxHP;
    public float CurrentHP { get; private set; }

    private float Damage;
    public bool isDead { get; private set; } = false;


    public Type Human_type;
    private int HireKey;     //뽑을 때 누르는 숫자
    private int Cost;      //뽑을 때 드는 비용

    
   // private GameObject Attack_Target;




    private void Awake()
    {
        playerController = FindObjectOfType<Player_Controller>();
        MeshCollider = GetComponent<CapsuleCollider>();
        DetectCollider = transform.GetChild(0).GetComponent<CapsuleCollider>();
        followPlayer = GetComponent<Follow_Player>();
        
    }

    private void Start()
    {
        Get_HumanType();
       
    }




    private void Update()
    {
        Behavior_Mode();
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
            case Player_Controller.Mode.Follow:
              
                break;


            case Player_Controller.Mode.Attack:
                //돌진하라
                break;


            case Player_Controller.Mode.Assemble:
                //그자리에 멈추기 & 적이 다가오면 방어모드
                break;

        }
    }

   

    public void Mode_Attack()
    {
        //�����϶�


    }





    private void OnTriggerEnter(Collider other)
    {

        if (other.CompareTag("Enemy") && playerController.CurrentMode == Player_Controller.Mode.Attack)
        {
         


        }
    }
}
