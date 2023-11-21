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
        FollowShield,
        Stop    //멈춰라~~
    }



    public Mode CurrentMode;

    //다른 클래스 객체=====================

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
    private EnemySpawn RespawnPoint;
    public Ply_Movement playerMovement;
    //보병 & 궁수 뽑을 수 있는가 판단하는 변수 -> 추후에 업그레이드 기능과 할때 사용해주세여
    private List<Position> positions = new List<Position>();
    private Formation_enemy formation_enemy;
    private int spawnIndex;
    private int nextIndex;
    public LayerMask TargetLayer;

    private UnityEngine.AI.NavMeshAgent[] agents;

   

    //변수추가 이서영

    public bool isOperateStop = false;
    public bool setFormation;
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

    [SerializeField] private Upgrade upgrade;
    public bool isUpgrade_SolDam = false;
    public bool isUpgrade_SolHP = false;
    

    private void Awake()
    {
        CurrentMode = Mode.Follow;

    }
    private void Start()
    {
        playerMovement = GetComponent<Ply_Movement>();
        formation_enemy = GetComponent<Formation_enemy>();

        positions = formation_enemy.positions;
    }
    private void Update()
    {
        if (GameManager.instance.isDead)
        {
            return;
        }   

        if (GameManager.instance.inRange)
        {
            Spawn_Solider();
        }



        //if(playerMovement.speed > 0.9)
        //{
        //    CurrentMode = Mode.Follow;
        //}
        //else
        //{
        //    CurrentMode = Mode.FollowShield;
        //}
        //상태 변경
        if (Input.GetKeyDown(KeyCode.Q))
        {
            Debug.Log("나를 따르라~~");
            if (!isPlay_FollowOrder)
            {
              
                animator.SetTrigger("FollowOrder");
                isPlay_FollowOrder = true;
             


            }


            isOperateFollow = true;
            CurrentMode = Mode.Follow;

           




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

         switch (CurrentMode)
        {
            case Mode.Follow:
                if (playerMovement.isPlayerMove == false || playerMovement.holdingShield)
                {
       
                    FormationOrder(8);


                    //setFormation = true;
                }
                else
                {
                    ResetPosition();    
                    nextIndex = 0;
                }
                break;

            case Mode.Attack:
                break;
            case Mode.FollowShield:
                break;
            case Mode.Stop:
                break;
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
                            if (GameManager.instance.Gold > GameManager.instance.unit0.cost)
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
                            if (GameManager.instance.Gold > GameManager.instance.unit1.cost && GameManager.instance.isPossible_Upgrade_1)
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
                            if (GameManager.instance.Gold > GameManager.instance.unit2.cost)
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
        Soilder_Controller Soilder_Con = newUnit.GetComponent<Soilder_Controller>();
        ColorManager.instance.RecursiveSearchAndSetUnit(newUnit.transform, GameManager.instance.Color_Index);
        Soilder_Con.infodata = unit;
        Soilder_Con.Setunit();
        UpgradeSet(Soilder_Con);

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
    
    private void UpgradeSet(Soilder_Controller soilder_Controller)
    {
        if(isUpgrade_SolDam)
        {
            soilder_Controller.data.damage = soilder_Controller.infodata.damage + 5;
        }

        if(isUpgrade_SolHP)
        {
            soilder_Controller.data.currentHP = soilder_Controller.infodata.currentHP + 50;
            soilder_Controller.data.maxHP = soilder_Controller.infodata.maxHP + 50;
        }
    }
    private Transform GetSoilder(RaycastHit[] hits)
    {
        Transform nearest = null;
        float closestDistance = float.MaxValue;
        //같은팀 병사담기
        foreach (RaycastHit hit in hits)
        {
            if (hit.transform.gameObject.CompareTag("Soldier"))
            {
                Soilder_Controller hit_con = hit.transform.gameObject.GetComponent<Soilder_Controller>();
                if (hit_con.formationState == Soilder_Controller.FormationState.Formation && !hit_con.isSetPosition) // 리더에게 도착했고 , 포지션이 설정안된친구 고르기 ( 포메이션 준비완료된 병사)
                {
                    float distance = Vector3.Distance(transform.position, hit.transform.position);


                    if (distance < closestDistance)
                    {
                        closestDistance = distance;
                        nearest = hit.transform;
                    }
                }
                else
                {
                    continue;
                }


            }

        }

        return nearest;
    }
    private void FormationOrder(float scanRange)
    {

        //같은팀 레이어들 모두 담기
        RaycastHit[] allHits = Physics.SphereCastAll(transform.position, scanRange, Vector3.forward, 0, 1 << gameObject.layer);

        Transform Soilder = null;
        Soilder = GetSoilder(allHits); //포메이션위치 와 가장가까운 병사 리턴
        if (Soilder != null)
        {
            Soilder_Controller soilder_con = Soilder.GetComponent<Soilder_Controller>();
            soilder_con.isSetPosition = true;
            soilder_con.formationTransmform = CarculateScore(Soilder); //제일가깝고 도착한 병사들 위치로 설정
            nextIndex++;
        }



    }
    private Transform CarculateScore(Transform Soilder)
    {

        float maxScore = 0;
        Position destination = null;
        for (int i = 0; i < positions.Count; i++)
        {
            if (positions[i].isSuccess)
            {
                continue;
            }


            float currentScore;
            float positionSocre = positions[i].weight / Vector3.Distance(transform.position, positions[i].position.position); // 포지션과 리더와 거리비례 점수 (멀면멀수록 weight점수 감소)
            //float distanceScore = Vector3.Distance(Soilder.position, positions[i].position.position); // 병사가 포지션까지 가는 거리 (
            currentScore = positionSocre /** distanceScore*/;   //포지션점수 / 병사가 포지션까지 가는 거리 ( 병사가 거리가멀면 점수가 더낮아짐)  ----> 최종 병사입장에서의 그자리의 점수

            if (currentScore > maxScore)
            {
                maxScore = currentScore;
                destination = positions[i];
                Soilder.GetComponent<Soilder_Controller>().currentScore = currentScore;
            }
            else
            {
                continue;
            }
                        

        }
        destination.isSuccess = true;
        return destination.position;
    }
    private void ResetPosition()
    {
        for (int i = 0; i < positions.Count; i++)
        {
            positions[i].isSuccess = false;
        }
    }
}