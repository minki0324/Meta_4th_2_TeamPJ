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
        Follow = 1,      //나를 따르라~ (플레이어가 멈추면 대열갖추기)
        Attack,    //돌격하라~
        Stop    //멈춰라~~
    }

    public Mode CurrentMode;

    //다른 클래스 객체=====================
    private Minion_Controller[] minionController;

    private Following following;

    //플레이어 정보========================
   

    //미니언 프리팹=========================
    //[Header("인덱스 - 0 : 보병 / 1 : 기사 / 2 : 궁수 / 3 : 힐러 / 4 : 창병 / 5 : 폴암병" )]
    //[SerializeField] private GameObject[] Minion_Prefabs;

    public List<GameObject> UnitList_List = new List<GameObject>();
    //public LinkedList<GameObject> Minions_List = new LinkedList<GameObject>();
    
    public int Human_num;

    [SerializeField]
    private Transform Spawner;
    public EnemySpawn spawnPoint;
    //보병 & 궁수 뽑을 수 있는가 판단하는 변수 -> 추후에 업그레이드 기능과 할때 사용해주세여

    private int spawnIndex;

    public LayerMask TargetLayer;

    private UnityEngine.AI.NavMeshAgent[] agents;



    //변수추가 이서영

    public bool isOperateStop = false;

    public bool isOperateFollow = true;


    public bool isDead { get; private set; }


    [SerializeField]
    private Animator animator;

    public bool isPlay_AttackOrder = false;
    public bool isPlay_FollowOrder = false;
    public bool isPlay_StopOrder = false;



    //추가
    public bool ischeckPosition = false;
    public Vector3 StopPos;


    private void Awake()
    {
        following = GetComponent<Following>();
        CurrentMode = Mode.Follow;

    }

    private void Update()
    {


        if (GameManager.instance.inRange)
        {
            Spawn_Solider();
        }
        //상태 변경
        if (Input.GetKeyDown(KeyCode.Q))
        {
            Debug.Log("나를 따르라~~");
            CurrentMode = Mode.Follow;
            if (!isPlay_FollowOrder)
            {
              
                animator.SetTrigger("FollowOrder");
                isPlay_FollowOrder = true;
            }


            isOperateFollow = true;
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            Debug.Log("돌격하라~~");
            CurrentMode = Mode.Attack;
            if (!isPlay_AttackOrder)
            {
                animator.SetTrigger("AttackOrder");
                isPlay_AttackOrder = true;
            }

        }



        if (Input.GetKeyDown(KeyCode.R))
        {
            Debug.Log("멈춰라~~");
            CurrentMode = Mode.Stop;
            if (!isPlay_StopOrder)
            {
                animator.SetTrigger("StopOrder");
                isPlay_StopOrder = true;
            }

            isOperateStop = true;
            isOperateFollow = false;


            ischeckPosition = true;

        }





    }

    private void Spawn_Solider()
    {
        if ( GameManager.instance.Current_MinionCount < GameManager.instance.Max_MinionCount)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1) || Input.GetKeyDown(KeyCode.Alpha2) || Input.GetKeyDown(KeyCode.Alpha3))
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

                if (selectedNumber != -1)
                {
                    // following.nearestMinion_List.Clear();
                    switch (selectedNumber)
                    {
                        case 1:
                            Human_num = 0;
                            if (GameManager.instance.Gold > 15)
                            {
                                Init_Solider(GameManager.instance.unit0);
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
                                Init_Solider(GameManager.instance.unit1);
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
                                Init_Solider(GameManager.instance.unit2);
                            }
                            else
                            {
                                // 나중에 골드가 부족합니다 띄워줄 UI 제작 해야함
                                Debug.Log("3번 병사 골드가 부족합니다.");
                            }
                            break;
                    }



                }
            }
        }
    }

    private void Init_Solider(Unit_Information unit)
    {

        // 나중에 인트로 씬에서 컬러셋 스크립트에서 컬러번호 넘겨 받은거로 소환할 때 컬러 적용 시켜야함

        //if(spawnPoint == null)
        //{
        //    FindSpawnPoint();
        //}
        //스폰포인트영역 들어가면 spawnPoint 참조받고 스폰위치 받아서 그위치로 소환.
        GameObject newUnit = Instantiate(unit.unitObject, spawnPoint.SpawnPoint[spawnIndex].position, Quaternion.identity);
        UnitAttack2 unitAttack2 = newUnit.GetComponent<UnitAttack2>();
        ColorManager.instance.RecursiveSearchAndSetUnit(newUnit.transform, GameManager.instance.Color_Index);
        unitAttack2.data = unit;
        unitAttack2.Setunit();

        spawnPoint.SetLayerRecursively(newUnit, gameObject.layer);

        GameManager.instance.Gold -= unit.cost;
        //Minion.transform.SetParent(transform);
        UnitList_List.Add(newUnit);
        GameManager.instance.Current_MinionCount++;




      
     
        spawnIndex++;

   
        //스폰위치를 차례대로 나오게하기위한 메소드 
        if (spawnIndex > 2)
        {
            spawnIndex = 0;
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("SpawnPoint") && other.gameObject.layer == gameObject.layer)
        {
            spawnPoint = other.GetComponent<EnemySpawn>();
        }
    }
    private void FindSpawnPoint()
    {
        GameObject[] spawns;
        spawns = GameObject.FindGameObjectsWithTag("SpawnPoint");
        float mindistance = int.MaxValue;
        foreach (GameObject ob in spawns)
        {
            if (gameObject.layer == ob.gameObject.layer)
            {

                float distance = Vector3.Distance(gameObject.transform.position, ob.transform.position);
                if (distance < mindistance)
                {
                    mindistance = distance;
                    spawnPoint = ob.GetComponent<EnemySpawn>();
                }
            }
        }
    }
}