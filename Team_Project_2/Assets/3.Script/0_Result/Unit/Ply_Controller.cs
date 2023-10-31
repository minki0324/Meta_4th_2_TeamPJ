using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ply_Controller : MonoBehaviour
{
    /*
        1. 숫자키 입력으로 병사 소환
        2. 단축키 입력으로 병사들 진영 배치
    */
    
    public enum Mode
    {
        Follow = 1,      //나를 따르라~
        Attack,    //돌격하라~
        Assemble,    //그자리에 멈추기 & 적이 다가오면 방어모드
        Stop    //멈춰라~~
    }

    public Mode CurrentMode;

    //다른 클래스 객체=====================
    private Minion_Controller[] minionController;

    private Following following;

    //플레이어 정보========================
    public int Max_MinionCount = 19;
    public int Current_MinionCount;

    //미니언 프리팹=========================
    [Header("인덱스 - 0 : 보병 / 1 : 기사 / 2 : 궁수")]
    [SerializeField] private GameObject[] Minion_Prefabs;

    public List<GameObject> Minions_List = new List<GameObject>();
    //public LinkedList<GameObject> Minions_List = new LinkedList<GameObject>();

    public int Human_num;

    [SerializeField]
    private Transform Spawner;

    //보병 & 궁수 뽑을 수 있는가 판단하는 변수 -> 추후에 업그레이드 기능과 할때 사용해주세여
    private bool isPossible_HeavyInfantry = false;
    private bool isPossible_Archer = false;


    public LayerMask TargetLayer;

    private UnityEngine.AI.NavMeshAgent[] agents;

    private List<GameObject> nearestMinion_List = new List<GameObject>();


    //변수추가 이서영

    public bool isOperateStop = false;

    public bool isOperateFollow = true;


    public bool isDead { get; private set; }

    private void Awake()
    {
        following = GetComponent<Following>();
        CurrentMode = Mode.Follow;
    }

    private void Update()
    {
        Spawn_Solider();
        //상태 변경
        if (Input.GetKeyDown(KeyCode.Q))
        {
            Debug.Log("나를 따르라~~");
            
            CurrentMode = Mode.Follow;
            isOperateFollow = true;
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

        if (Input.GetKeyDown(KeyCode.T))
        {
            Debug.Log("멈춰라~~");

            CurrentMode = Mode.Stop;
            isOperateStop = true;

            isOperateFollow = false;
     
          
            
        }



     

    }

    private void Spawn_Solider()
    {
        if(Current_MinionCount < Max_MinionCount)
        {
            if(Input.GetKeyDown(KeyCode.Alpha1) || Input.GetKeyDown(KeyCode.Alpha2) || Input.GetKeyDown(KeyCode.Alpha3))
            {
                int selectedNumber = -1; // 기본값 설정
                

                if (Input.GetKeyDown(KeyCode.Alpha1))
                {
                    selectedNumber = 1;
                }
                else if (Input.GetKeyDown(KeyCode.Alpha2))
                {
                    selectedNumber = 2;
                }
                else if (Input.GetKeyDown(KeyCode.Alpha3))
                {
                    selectedNumber = 3;
                }

                if(selectedNumber != -1)
                {
                    
                    switch (selectedNumber)
                    {
                        case 1:
                            Debug.Log("1눌림");
                            Human_num = 0;
                            if(GameManager.instance.Gold > 15)
                            {
                                Init_Solider(Human_num);
                            }
                            else
                            {
                                // 나중에 골드가 부족합니다 띄워줄 UI 제작 해야함
                                Debug.Log("1번 병사 골드가 부족합니다.");
                            }
                            break;
                        case 2:
                            Debug.Log("2눌림");
                            Human_num = 1;
                            // 나중에 if문에 앤드게이트로 isPossible_HeavyInfantry 업글 유무 확인
                            if (GameManager.instance.Gold > 20)
                            {
                                Init_Solider(Human_num);
                            }
                            else
                            {
                                // 나중에 골드가 부족합니다 띄워줄 UI 제작 해야함
                                Debug.Log("2번 병사 골드가 부족합니다.");
                            }
                            break;
                        case 3:
                            Debug.Log("3눌림");
                            Human_num = 2;
                            // 나중에 if문에 앤드게이트로 isPossible_Archer 업글 유무 확인
                            if (GameManager.instance.Gold > 25)
                            {
                                Init_Solider(Human_num);
                            }
                            else
                            {
                                // 나중에 골드가 부족합니다 띄워줄 UI 제작 해야함
                                Debug.Log("3번 병사 골드가 부족합니다.");
                            }
                            break;
                    }


                    StartCoroutine(following.Mode_Follow_co());
                }
            }
        }
    }

    private void Init_Solider(int Human_num)
    {
        // 나중에 인트로 씬에서 컬러셋 스크립트에서 컬러번호 넘겨 받은거로 소환할 때 컬러 적용 시켜야함

        GameObject Minion = null;
        Minion = Instantiate(Minion_Prefabs[Human_num], Spawner.position, Quaternion.identity);
        //미니언 생성 위치는 나중에 점령지(Spawner)위치로 바꾸기 

        Minion_Controller minionController = Minion.AddComponent<Minion_Controller>();

        GameManager.instance.Gold -= (15 + (Human_num * 5));
        Minion.transform.SetParent(transform);
        Minions_List.Add(Minion);
        Current_MinionCount++;
    }
}
