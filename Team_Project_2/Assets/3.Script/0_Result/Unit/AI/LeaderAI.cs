using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class LeaderAI : LeaderState 
{
    private LayerMask targetLayer;
    private GameObject[] flag;
    SphereCollider col;
    bool isStart = false;
    [SerializeField] private GameObject targetFlag;

    public bool isEnermyChecked = false;

    //public Transform nearestTarget;
    protected override void Awake()
    {
        base.Awake();
        combinedMask = TargetLayers();
        flag = GameObject.FindGameObjectsWithTag("Flag");
        targetFlag = TargetFlag();
        bat_State = BattleState.Move;
        col = GetComponent<SphereCollider>();
        col.enabled = false;
        
    }
    protected override void Start()
    {
        base.Start();
        GameManager.instance.leaders.Add(gameObject.GetComponent<LeaderState>());
        SetLeaderState();
        
    }
    private void Update()
    {
        if (!GameManager.instance.isLive)
        {
            return;
        }
        if(!isStart)
        {
            col.enabled = true;
            isStart = true;
        }

        if (data.currentHP <= 0)
        {
            if (!data.isDie)
            {
                Die();
            }
        }
        // 항상 주변에 적이있는지 탐지
        EnemyDitect();


        switch (bat_State)
        {

            case BattleState.Attack:
                AttackOrder();
                break;
            case BattleState.Search:
                break;
            case BattleState.Move:
                if (targetFlag.transform.position != null)
                {
                    ani.SetBool("Move", true);
                }
                else
                {
                    bat_State = BattleState.Search;
                }
                break;
            case BattleState.Defense:
                break;

            case BattleState.Detect:
                //ani.SetBool("Move", true);
                //navMesh.SetDestination(NearestTarget.position);
                break;

        }

        if (data.currentHP <= 0)
        {
            
            //공격정지 ,이동정지 
            if (!data.isDie)
            {
                data.isDie = true;
                Die();
            }

        }

    }
    public override void Die()
    {
        //죽는애니메이션
        //레이어변하기
        //콜라이더 끄기
        //리스폰위치 저장하기
        //isDead true하기
        ani.SetLayerWeight(1, 0);
        ani.SetTrigger("Dead"); // 죽는모션재생
        col.enabled = false;
        aipath.canMove = false;
        aipath.canSearch = false;
        SetRespawnPoint();
        gameObject.layer = 12;   // 레이어 DIe로 변경해서 타겟으로 안되게
        //HitBox_col.enabled = false;
        data.isDie = true;

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



                if (data.currentHP <= 0)
                {
                    //적이 나를 죽였을때 -> 플레이어 컨트롤 다이에서 따로 설정
                    //enemy ==null  -> 플레이어

                    //우리팀을 적팀이 죽였을때 
                    
                        //병사끼리의 킬계산
                        if (enemy != null)
                        {
                            //적팀을 우리팀이 죽였을때
                            if (enemy.leader == player.gameObject)
                            {
                                GameManager.instance.killCount++;
                                leaderState.deathCount++;
                            }
                            //적팀끼리 죽였을때
                            else
                            {
                                enemy.leaderState.killCount++;
                                deathCount++;
                            }
                        }
                        else//리더가죽였을때 킬계산
                        {  //적팀을 내가 죽였을때 (enemy ==null)
                            if (other.gameObject == player.gameObject)
                            {
                                GameManager.instance.killCount++;
                                deathCount++;
                            }
                            else
                            {
                                LeaderAI _leader = ob.GetComponent<LeaderAI>();
                                _leader.killCount++;
                                leaderState.deathCount++;
                            }

                        }

                    
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
            if (attackDistance <= AttackRange)
            {
                bat_State = BattleState.Attack;
            }

            isEnermyChecked = true;
        }
        else
        {
            if (bat_State == BattleState.Attack)
            {
                if (targetFlag == null)
                {
                    bat_State = BattleState.Search;
                }
                else
                {
                    bat_State = BattleState.Move;
                }


            }
            bat_State = BattleState.Move;
            isEnermyChecked = false;
        }


        //범위내에 적콜라이더가 있을시 Ditect 상태로 변경
    }

    private GameObject TargetFlag()
    {
        GameObject[] defaultFlags = flag.Where(_flag => _flag.layer != gameObject.layer).ToArray();
        if (defaultFlags.Length > 0)
        {
            int randomIndex = Random.Range(0, defaultFlags.Length);
            GameObject selected1Flag = defaultFlags[randomIndex];

            return selected1Flag;
            // 선택된 객체(selectedFlag)를 사용하세요.
        }

        GameObject selectedFlag = null;
        int minTouchingCount = int.MaxValue;
        int layerMask = (1 << LayerMask.NameToLayer("Team")) | (1 << LayerMask.NameToLayer("Enemy1")) | (1 << LayerMask.NameToLayer("Enemy2")) | (1 << LayerMask.NameToLayer("Enemy3"));
        int radius = 10;
        foreach (GameObject _flag in flag)
        {
            // _flag 주변에서 trigger에 닿아 있는 객체 검출
            Collider[] colliders = Physics.OverlapSphere(_flag.transform.position, radius, layerMask, QueryTriggerInteraction.Collide);
            

            // 최소 카운트 갱신
            if (colliders.Length < minTouchingCount)
            {
                minTouchingCount = colliders.Length;
                selectedFlag = _flag;
            }
        }

        if (selectedFlag != null)
        {
            return selectedFlag;
        }
        else
        { 
            Debug.Log("깃발못찾음");
            return null;

        }


    }
    public override void HitDamage(float damage)
    {
        data.currentHP -= damage;
    }
    public override void Lostleader()
    {
        leaderState.bat_State = LeaderState.BattleState.Move;
    }

    
  
}