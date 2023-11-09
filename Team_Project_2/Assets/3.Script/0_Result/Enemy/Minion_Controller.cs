using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Minion_Controller : MonoBehaviour
{
    /*
    �̴Ͼ� ��Ʈ�ѷ�
      1. �÷��̾��� Ű �Է¿� ���� ��庯��
      2. ���¿� ���� �ִϸ��̼� ����
    */

    [SerializeField]
    GameObject Player;

    public enum Type
    {
        //�̴Ͼ� ����
        //���ڴ� ���� ���� :)
        SwordMan = 15,
        HeavyInfantry = 20,
        Archer = 25,
    }

    private Ply_Controller playerController;
    private Ply_Movement playerMovement;    //added
    private Animator ani;
    private NavMeshAgent agent;

    private CapsuleCollider MeshCollider;  //������ �Ǳ��̴� ������ �ݶ��̴�
    private CapsuleCollider DetectCollider; //���� ������ �ݶ��̴�
    [SerializeField] private UnitAttack1 UnitAtk;

    private float MaxHP;
    public float CurrentHP { get; private set; }

    private float Damage;
    public bool isDead { get; private set; } = false;

    public Type Human_type;

    public bool isDetect = false;
    public bool isClose = false;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        UnitAtk = GetComponent<UnitAttack1>();
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
        transform.forward = playerController.transform.forward;
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



                // �÷��̾�or�뿭�� �� ����� ��������� �� üũ�ϴ� �޼ҵ� �־ ��������� Bool�� false�� ����, �־����� �ٽ� true�� �����ؼ� ���󰡱�
                break;


            case Ply_Controller.Mode.Attack:
                // ���¿� ���� �ִϸ��̼� �����ؾ���.
                /*
                    1. �÷��̾��� ������ �̵��Ҷ��� Move�� true�ٲ㼭 �޷����� ���
                    2. ������ �����Ÿ� ���� ������ ���� Move�� false�� �ٲٰ� ��ü�� Idle���� ��ü�� ��Ÿ�ӿ����� Attack Trigger�� �Ѽ� ���� ��� ���ϵ���
                */
                if(UnitAtk.nearestTarget != null)
                {
                    UnitAtk.MinionAttack();
                }
                else
                {
                    return;
                }
                break;
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            agent.obstacleAvoidanceType = ObstacleAvoidanceType.HighQualityObstacleAvoidance;
        }
        if (collision.gameObject.CompareTag("Door"))
        {
            agent.obstacleAvoidanceType = ObstacleAvoidanceType.NoObstacleAvoidance;
        }
    }

}