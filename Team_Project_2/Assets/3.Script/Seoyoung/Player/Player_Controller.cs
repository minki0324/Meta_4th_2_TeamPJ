using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Controller : MonoBehaviour
{
    public enum TeamColor
    {
        Red = 0,
        Blue,
        Green,
        Yellow,
    }
    public enum Mode
    {
        Follow = 1,      //���� ������~
        Attack,    //�����϶�~
        Assemble    //���ڸ��� ���߱� & ���� �ٰ����� �����
    }

    public TeamColor MyTeam;

    public Mode CurrentMode;

    //�ٸ� Ŭ���� ��ü=====================
    private Minion_Controller minionController;


    //�÷��̾� ����========================
    private int Max_MinionCount;
    private int Current_MinionCount;
    public int Gold { get; private set; } = 0; //���� ���� ���(��)
    public Transform CurrentTransform { get; private set; }

    public bool isDead { get; private set; }


    //�̴Ͼ� ������=========================
    [SerializeField]
    private GameObject SwordMan_Prefab;

    [SerializeField]
    private GameObject HeavyInfantry_Prefab;

    [SerializeField]
    private GameObject Archer_Prefab;

    public List<GameObject> Minions_List = new List<GameObject>();
    //public LinkedList<GameObject> Minions_List = new LinkedList<GameObject>();

   

    public int Human_num;

    [SerializeField]
    private GameObject Spawner;


    //���� & �ü� ���� �� �ִ°� �Ǵ��ϴ� ���� -> ���Ŀ� ���׷��̵� ��ɰ� �Ҷ� ������ּ���
    private bool isPossible_HeavyInfantry = false;
    private bool isPossible_Archer = false;


    private void Awake()
    {
      

        MyTeam = TeamColor.Red;
        CurrentMode = Mode.Follow;
    }

    private void Start()
    {
        CurrentTransform = this.transform;
        Max_MinionCount = 19;

        //GameObject Minion = Instantiate(SwordMan_Prefab, transform.position + Vector3.back, Quaternion.identity);
        //Minion.transform.SetParent(this.transform);
        //Minions_List.Add(Minion);
        //�̴Ͼ� ���� ��ġ�� ���߿� ������(Spawner)��ġ�� �ٲٱ� 
    }

    private void Update()
    {
        //���߿� �Լ�ȭ(switch case) �ϴ��� �ϱ�
        //�̴Ͼ� ��ȯ
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            Debug.Log("1����");
            Human_num = 1;
            GameObject Minion = Instantiate(SwordMan_Prefab, Spawner.transform.position, Quaternion.identity);
            Minion.transform.SetParent(this.transform);
            Minions_List.Add(Minion);
            //�̴Ͼ� ���� ��ġ�� ���߿� ������(Spawner)��ġ�� �ٲٱ� 

            Gold -= 15;
        }

        if (Input.GetKeyDown(KeyCode.Alpha2) && isPossible_HeavyInfantry)
        {
            Debug.Log("2����");
            Human_num = 2;
            GameObject Minion = Instantiate(SwordMan_Prefab, transform.position + Vector3.back, Quaternion.identity);
            Minion.transform.SetParent(this.transform);
            Minions_List.Add(Minion);
            Gold -= (int)minionController.Human_type;
        }

        if (Input.GetKeyDown(KeyCode.Alpha3) && isPossible_Archer)
        {
            Debug.Log("3����");
            Human_num = 3;
            GameObject Minion = Instantiate(SwordMan_Prefab, transform.position + Vector3.back, Quaternion.identity);
            Minion.transform.SetParent(this.transform);
            Minions_List.Add(Minion);
            Gold -= (int)minionController.Human_type;
        }


        //���� ����
        if (Input.GetKeyDown(KeyCode.Q))
        {
            Debug.Log("���� ������~~");
            CurrentMode = Mode.Follow;
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            Debug.Log("�����϶�~~");
            CurrentMode = Mode.Attack;
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            Debug.Log("�𿩶�~~");
            CurrentMode = Mode.Assemble;
        }


    }




}
