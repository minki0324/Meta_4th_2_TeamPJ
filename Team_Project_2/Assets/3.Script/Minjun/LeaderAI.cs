using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System.Linq;
public class LeaderAI : Unit
{
    private float scanRange = 10f;
    private LayerMask targetLayer;
    private int invertedLayerMask;
    private NavMeshAgent navMesh;
    private Animator ani;
    private GameObject[] flag;
    [SerializeField] private GameObject targetFlag;
   
    //public Transform nearestTarget;
    private float AttackRange = 5f;
    private void Awake()
    {
        ani = GetComponent<Animator>();
        combinedMask = TargetLayers();
        navMesh = GetComponent<NavMeshAgent>();
        bat_State = BattleState.Follow;
        flag = GameObject.FindGameObjectsWithTag("Flag");
    }
    private void Update()
    {
        for (int i = 0; i < flag.Length; i++)
        {
            Debug.Log(flag[i]);
        }

        // 항상 주변에 적이있는지 탐지
        EnemyDitect();
        switch (bat_State)
        {
            case BattleState.Follow:
                //if(targetFlag.transform.position != null) { 
                //navMesh.SetDestination(targetFlag.transform.position);
                //}
                //else
                //{
                //    return;
                //}
                //navMesh.isStopped = true;
                break;
            case BattleState.Attack:
                break;
            case BattleState.Detect:

                //애니메이션 방패들기
                ani_State = AniState.shild;
                //천천히 적에게 접근
                //Debug.Log()
                ani.SetBool("Move", true);
                navMesh.SetDestination(NearestTarget.position);
                break;

        }

        switch (jud_State)
        {
            case JudgmentState.Ready:
                ani_State = AniState.Idle;
                Debug.Log("타겟깃발정함");
                targetFlag= TargetFlag();
                navMesh.SetDestination(targetFlag.transform.position);
                jud_State = JudgmentState.Going;
                //navMesh.SetDestination()
                break;
            case JudgmentState.Wait:
                break;
            case JudgmentState.Going:
                //죽거나 
                break;
           

                //float originalSpeed = navMeshAgent.speed; // 현재 속도 저장
                //navMeshAgent.speed = originalSpeed / 4; // 1/4로 줄인 속도 설정



        }



    }

    private void EnemyDitect()
    {
        RaycastHit[] hits = Physics.SphereCastAll(transform.position, scanRange, Vector3.forward, 0, combinedMask);
        NearestTarget = GetNearestTarget(hits);
        if (NearestTarget != null)
        {
            float attackDistance = Vector3.Distance(transform.position, NearestTarget.position);
            bat_State = BattleState.Detect;

            //DItect 상태일때 방패를 들며 천천히 접근
            if (attackDistance <= AttackRange)
            {
                bat_State = BattleState.Attack;
            }
        }
        else
        {
            if(bat_State == BattleState.Attack) { 
            if(targetFlag == null)
            {
                jud_State = JudgmentState.Ready;
            }
            else
            {
                jud_State = JudgmentState.Going;
            }
           
            bat_State = BattleState.Follow;
            }
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
}