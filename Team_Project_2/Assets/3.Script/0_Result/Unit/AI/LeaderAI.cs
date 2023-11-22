using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Pathfinding;
public class LeaderAI : LeaderState
{
    private LayerMask targetLayer;
    private GameObject[] flag;
    SphereCollider col;
    bool isStart = false;
    private Formation_enemy formation;
    private float attackOrederRange = 8;

    private LeaderController leaderController;
    public Vector3 leaderAIDirection;
    public List<Position> positions = new List<Position>();
    private int nextIndex = 0;
    protected override void Awake()
    {
        base.Awake();
        combinedMask = TargetLayers();
        flag = GameObject.FindGameObjectsWithTag("Flag");
        leaderController = GetComponent<LeaderController>();
        col = GetComponent<SphereCollider>();
        col.enabled = false;

    }
    protected override void Start()
    {
        base.Start();
        GameManager.instance.leaders.Add(gameObject.GetComponent<LeaderState>());
        formation = GetComponent<Formation_enemy>();
        positions = formation.positions; // 포메이션 포지션들 우선순위로 담아둔 리스트 초기화
        SetLeaderState();

    }
    private void Update()
    {

        if (!GameManager.instance.isLive || data.isDie)
        {
            return;
        }

        if (!isStart)
        {
            col.enabled = true;
            isStart = true;
        }
        total_Gold += Time.deltaTime * Magnifi * has_Flag * Upgrade_GoldValue;
        Gold += Time.deltaTime * Magnifi * has_Flag * Upgrade_GoldValue; // 골드수급 = 분당 120 * 점령한 지역 개수

        // 항상 주변에 적이있는지 탐지
        if (!data.isDie)
        {
            EnemyDitect();


            if (holdingShield )
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
            aipath.maxSpeed = 5 * speed;
            ani.SetBool("Shield", holdingShield);
            ani.SetFloat("Speed", speed);
            ani.SetBool("Move", isMove);

            #region 리더상태에 따른 행동
            switch (bat_State)
            {

                case BattleState.Move:
                    ResetPosition();
                 
                    holdingShield = false;
                    isMove = true;
                    aipath.isStopped = false;
                    nextIndex = 0;
                    for (int i = 0; i < UnitList.Count; i++)
                    {
                        UnitList[i].GetComponent<Soilder_Controller>().isSetPosition = false;

                    }

                    break;
                case BattleState.Attack:
                    ResetPosition();
                    holdingShield = false;
                    isMove = false;
                    aipath.isStopped = true;
                    //느려졌던 이동속도 초기화
                    aipath.maxSpeed = Mathf.Lerp(aipath.maxSpeed, defaultSpeed, Time.deltaTime * 1);
                    AttackOrder();
                    break;
                case BattleState.Detect:

                    target.target = nearestTarget;

                    holdingShield = true;
                    aipath.maxSpeed = 1.5f; // 이동속도줄이기
                    leaderAIDirection = transform.TransformDirection(Vector3.forward);
                    FormationOrder(formationRange);
                    //formation.Following_Shield(aipath.maxSpeed, leaderAIDirection);
                    break;
                case BattleState.Wait:
                    //리더가 타겟에게 가는 이동속도 줄이기 , 방패들기

                    holdingShield = true;
                    aipath.maxSpeed = 1.5f; // 이동속도줄이기
                    leaderAIDirection = transform.TransformDirection(Vector3.forward);
                    FormationOrder(formationRange);
                    //formation.Following_Shield(aipath.maxSpeed, leaderAIDirection);
                    break;
                case BattleState.Defense:
                    break;

                default:
                    holdingShield = false;
                    break;


            }
            #endregion
        }



    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, scanRange);
    }
    public override void Die(int teamLayer, int enemyLayer)
    {
        //죽는애니메이션
        //레이어변하기
        //콜라이더 끄기
        //리스폰위치 저장하기
        //isDead true하기
        ani.SetLayerWeight(1, 0);
        ani.SetTrigger("Dead"); // 죽는모션재생
        col.enabled = false;
        aipath.isStopped = true;
        SetRespawnPoint();
        gameObject.layer = 12;   // 레이어 DIe로 변경해서 타겟으로 안되게
        //HitBox_col.enabled = false;
        data.isDie = true;
        target.target = null;
        KillCount_Set(teamLayer, enemyLayer);
    }
    private void SetRespawnPoint()
    {
        EnemySpawn ES = GameManager.instance.FindSpawnPoint(gameObject);
        respawnPoint = ES.transform.GetChild(0);
    }
    private void SetLeaderState()
    {
        data.maxHP = 150;
        data.currentHP = data.maxHP;
        data.damage = 20;
        data.isDie = false;


    }


    private void OnTriggerEnter(Collider other)
    {
        //적병사가 -> Soilder_Controller
        //적리더가 -> LeaderAI
        //플레이어가 ->GameManager

        if (other.CompareTag("Weapon") && !isHitting)
        {
            GameObject ob = FindParentGameObject(other.gameObject);   //칼을들고있는 오브젝트

            if (gameObject.layer != ob.layer) //공격하는 오브젝트가 적일때 
            {

                enemy = FindParentComponent(other.gameObject);


                //병사들
                if (enemy != null)
                {
                    if (enemy.gameObject.layer != gameObject.layer)
                    {
                        StartCoroutine(Hit_co(enemy.data.damage));


                    }
                }
                else//null : 리더들 , 플레이어 
                {
                    LeaderAI _leader = other.GetComponent<LeaderAI>();

                    if (_leader != null && _leader.gameObject.layer != gameObject.layer)
                    {
                        StartCoroutine(Hit_co(_leader.data.damage));
                    }
                    else if (gameObject.layer != player.gameObject.layer)
                    {
                        StartCoroutine(Hit_co(GameManager.instance.Damage));
                    }
                }



                if (data.currentHP <= 0 && !data.isDie)
                {
                    //적이 나를 죽였을때 -> 플레이어 컨트롤 다이에서 따로 설정
                    //enemy ==null  -> 플레이어

                    //우리팀을 적팀이 죽였을때 

                    //공격정지 ,이동정지 
                    data.isDie = true;
                    Die(gameObject.layer, ob.layer);


                }
            }
        }

    }

    private void EnemyDitect()
    {
        RaycastHit[] hits = Physics.SphereCastAll(transform.position, scanRange, Vector3.forward, 0, combinedMask);
        nearestTarget = GetNearestTarget(hits);

        if (nearestTarget != null)
        {
            float attackDistance = Vector3.Distance(transform.position, nearestTarget.position);
            bat_State = BattleState.Detect;

            //DItect 상태일때 방패를 들며 천천히 접근
            if (attackDistance <= attackOrederRange)
            {
                bat_State = BattleState.Attack;
            }

        }


        //범위내에 적콜라이더가 있을시 Ditect 상태로 변경
    }

   
    public override void HitDamage(float damage)
    {
        data.currentHP -= damage;
    }
    public override void Lostleader()
    {
        target.target = null;
        leaderState.bat_State = LeaderState.BattleState.Move;
    }

    public void Respawn(GameObject leader, int layer)
    {
        //애니메이션초기화
        //HP , 콜라이더 , isDead ,레이어 다시설정
        //저장한 리스폰 위치로 이동
        data.currentHP = data.maxHP;
        data.isDie = false;
        aipath.isStopped = false;
        aipath.canMove = true;
        leader.layer = layer;
        leader.transform.position = respawnPoint.position;
        ani.SetTrigger("Reset");
        ani.SetLayerWeight(1, 1);
        col.enabled = true;
        target.target = null;
        bat_State = BattleState.Move;


    }


    //리더에게 가장가까운 병사 추출
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


                    if (distance  < closestDistance)
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
            if (positions[i].isSuccess) {
                continue;
            }


            float currentScore;
            float positionSocre = positions[i].weight / Vector3.Distance(transform.position, positions[i].position.position); // 포지션과 리더와 거리비례 점수 (멀면멀수록 weight점수 감소)
            float distanceScore = Vector3.Distance(Soilder.position, positions[i].position.position); // 병사가 포지션까지 가는 거리 (
            currentScore = positionSocre / distanceScore;   //포지션점수 / 병사가 포지션까지 가는 거리 ( 병사가 거리가멀면 점수가 더낮아짐)  ----> 최종 병사입장에서의 그자리의 점수
            
            if(currentScore > maxScore)
            {
                maxScore = currentScore;
                destination = positions[i];
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