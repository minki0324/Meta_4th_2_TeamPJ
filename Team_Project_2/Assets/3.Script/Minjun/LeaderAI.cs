using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System.Linq;
public class LeaderAI : LeaderState 
{
    private LayerMask targetLayer;
    private NavMeshAgent navMesh;
    private GameObject[] flag;
    [SerializeField] private GameObject targetFlag;

    public bool isEnermyChecked = false;

    //public Transform nearestTarget;
    protected override void Awake()
    {
        base.Awake();
        combinedMask = TargetLayers();
        navMesh = GetComponent<NavMeshAgent>();
        flag = GameObject.FindGameObjectsWithTag("Flag");
        targetFlag = TargetFlag();
        bat_State = BattleState.Move;
        
    }
    protected override void Start()
    {
        base.Start();
        GameManager.instance.leaders.Add(gameObject.GetComponent<LeaderState>());
    }
    private void Update()
    {

        if (!GameManager.instance.isLive)
        {
            return;
        }

        if (data.currentHP <= 0)
        {
            if (!isDead)
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

                targetFlag = TargetFlag();
                if (targetFlag != null)
                {
                    bat_State = BattleState.Move;
                }


                break;
            case BattleState.Move:
                if (targetFlag.transform.position != null)
                {
                    ani.SetBool("Move", true);
                    Debug.Log("깃발이동");
                    navMesh.SetDestination(targetFlag.transform.position);
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


    }
    public override void Die()
    {
        //죽는애니메이션
        //레이어변하기
        //콜라이더 끄기
        //리스폰위치 저장하기
        //isDead true하기
        //ani.SetTrigger("Dead"); // 죽는모션재생
        ani.SetBool("Die", true); // 죽는모션재생
        gameObject.layer = 12;   // 레이어 DIe로 변경해서 타겟으로 안되게
        HitBox_col.enabled = false;
        EnemySpawn ES = GameManager.instance.FindSpawnPoint(gameObject);
        respawnPoint = ES.transform.GetChild(0);
        isDead = true;

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