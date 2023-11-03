using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class LeaderAI : Unit
{
    private float scanRange = 10f;
    private LayerMask targetLayer;
    private int invertedLayerMask;
    private NavMeshAgent navMesh;
    private Animator ani;

    //public Transform nearestTarget;
    private float AttackRange = 5f;
    private void Awake()
    {
        ani = GetComponent<Animator>();
        combinedMask = TargetLayers();
        navMesh = GetComponent<NavMeshAgent>();
        bat_State = BattleState.Follow;
    }
    private void Update()
    {
        // 항상 주변에 적이있는지 탐지
        EnemyDitect();
        switch (bat_State)
        {
            case BattleState.Follow:
                Debug.Log("AI Follow상태");
                //navMesh.isStopped = true;
                break;
            case BattleState.Attack:
                Debug.Log("AI Attack상태");
              
                break;
        }

        switch (jud_State)
        {
            case JudgmentState.Ready:
                ani_State = AniState.Idle;
                break;
            case JudgmentState.wait:
                break;
            case JudgmentState.Detect:
                Debug.Log("AI 적감지후 접근중");
                //애니메이션 방패들기
                ani_State = AniState.shild;
                //천천히 적에게 접근
                //Debug.Log()
                ani.SetBool("Move" , true);
                navMesh.SetDestination(nearestTarget.position);

                //float originalSpeed = navMeshAgent.speed; // 현재 속도 저장
                //navMeshAgent.speed = originalSpeed / 4; // 1/4로 줄인 속도 설정


                break;
        }



    }

    private void EnemyDitect()
    {
        RaycastHit[] hits = Physics.SphereCastAll(transform.position, scanRange, Vector3.forward, 0, combinedMask);
        nearestTarget = GetNearestTarget(hits);
        if(nearestTarget != null) { 
        float attackDistance = Vector3.Distance(transform.position, nearestTarget.position);
            jud_State = JudgmentState.Detect;
           
            //DItect 상태일때 방패를 들며 천천히 접근
            if (attackDistance <= AttackRange)
            {
                bat_State = BattleState.Attack;
            }
        }
        else
        {
            jud_State = JudgmentState.Ready;
            bat_State = BattleState.Follow;
        }

       
        //범위내에 적콜라이더가 있을시 Ditect 상태로 변경
    }
  

}   