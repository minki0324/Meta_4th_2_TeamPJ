using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI    ;

public class Player_Controller : MonoBehaviour
{
    /*
      플레이어 컨트롤러
      */
    public enum TeamColor
    {
        Red = 0,
        Blue,
        Green,
        Yellow,
    }
    public enum Mode
    {
        Follow = 1,      //나를 따르라~
        Attack,    //돌격하라~
        Assemble    //그자리에 멈추기 & 적이 다가오면 방어모드
    }

    public TeamColor MyTeam;

    public Mode CurrentMode;

    //다른 클래스 객체=====================
    private Minion_Controller[] minionController;

    private Following following;

    //플레이어 정보========================
    private int Max_MinionCount;
    private int Current_MinionCount;
    public int Gold { get; private set; } = 0; //현재 보유 골드(돈)
    public Transform CurrentTransform { get; private set; }

    public bool isDead { get; private set; }


    //미니언 프리팹=========================
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
    private Transform Spawner;


    //보병 & 궁수 뽑을 수 있는가 판단하는 변수 -> 추후에 업그레이드 기능과 할때 사용해주세여
    private bool isPossible_HeavyInfantry = false;
    private bool isPossible_Archer = false;


    //추가된 변수 - 이서영
    private List<GameObject> nearestMinion_List = new List<GameObject>();

    public LayerMask TargetLayer;

    private UnityEngine.AI.NavMeshAgent[] agents;

    private void Awake()
    {
        following = GetComponent<Following>();

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
        //미니언 생성 위치는 나중에 점령지(Spawner)위치로 바꾸기 
    }

    private void Update()
    {


        //나중에 함수화(switch case) 하던가 하깅
        //미니언 소환
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            Debug.Log("1눌림");
            Human_num = 1;
            GameObject Minion = Instantiate(SwordMan_Prefab, Spawner.transform.position, Quaternion.identity);
            Minion.transform.SetParent(this.transform);
            Minions_List.Add(Minion);
            //미니언 생성 위치는 나중에 점령지(Spawner)위치로 바꾸기 


            Gold -= 15;
        }

        if (Input.GetKeyDown(KeyCode.Alpha2) && isPossible_HeavyInfantry)
        {
            Debug.Log("2눌림");
            Human_num = 2;
            GameObject Minion = Instantiate(SwordMan_Prefab, transform.position + Vector3.back, Quaternion.identity);
            Minion.transform.SetParent(this.transform);
            Minions_List.Add(Minion);
            Gold -= 10;
        }

        if (Input.GetKeyDown(KeyCode.Alpha3) && isPossible_Archer)
        {
            Debug.Log("3눌림");
            Human_num = 3;
            GameObject Minion = Instantiate(SwordMan_Prefab, transform.position + Vector3.back, Quaternion.identity);
            Minion.transform.SetParent(this.transform);
            Minions_List.Add(Minion);
            Gold -= 15;
        }


        //상태 변경
        if (Input.GetKeyDown(KeyCode.Q))
        {
            Debug.Log("나를 따르라~~");
            StopCoroutine(following.Mode_Follow_co());
            StartCoroutine(following.Mode_Follow_co());
            CurrentMode = Mode.Follow;
           


        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            Debug.Log("돌격하라~~");
            CurrentMode = Mode.Attack;

        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            Debug.Log("모여라~~");
            CurrentMode = Mode.Assemble;
        }





    }





  

}
