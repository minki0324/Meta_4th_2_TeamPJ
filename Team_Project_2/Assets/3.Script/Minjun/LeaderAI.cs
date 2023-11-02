using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class LeaderAI : LeaderState
{
    private float scanRange = 10f;
    private LayerMask targetLayer;
    private int invertedLayerMask;
    private NavMeshAgent navMesh;
    public Transform nearestTarget;
    private float AttackRange = 5f;
    private void Awake()
    {
        targetLayer = 1 << gameObject.layer;
        Debug.Log(targetLayer); 
        invertedLayerMask = ~targetLayer.value; // targetLayer를 제외한 나머지 레이어들을 검출
     
        navMesh = GetComponent<NavMeshAgent>();
    }
    private void Update()
    {
        // 항상 주변에 적이있는지 탐지
        EnemyDitect();



        switch (jud_State)
        {
            case JudgmentState.Ready:
                ani_State = AniState.Idle;
                break;
            case JudgmentState.wait:
                break;
            case JudgmentState.Ditect:
                //애니메이션 방패들기
                ani_State = AniState.shild;

                //천천히 적에게 접근


                break;
        }

    }

    private void EnemyDitect()
    {
        RaycastHit[] hits = Physics.SphereCastAll(transform.position, scanRange, Vector3.forward, 0, invertedLayerMask);
        nearestTarget = GetNearestTarget(hits);

        float attackDistance = Vector3.Distance(transform.position, nearestTarget.position);
        if (attackDistance <= AttackRange)
        {
            bat_State = BattleState.Attack;
        }
       
        //범위내에 적콜라이더가 있을시 Ditect 상태로 변경
        if (hits.Length > 0)
        {
            jud_State = JudgmentState.Ditect;
            navMesh.SetDestination(nearestTarget.position);
            //DItect 상태일때 방패를 들며 천천히 접근
            if (attackDistance <= AttackRange)
            {
                bat_State = BattleState.Attack;
            }
        }
        else
        {
            bat_State = BattleState.Attack;
        }
    }
    Transform GetNearestTarget(RaycastHit[] hits)
    {
        Transform nearest = null;
        float closestDistance = float.MaxValue;

        foreach (RaycastHit hit in hits)
        {
            float distance = Vector3.Distance(transform.position, hit.transform.position);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                nearest = hit.transform;
            }
        }

        return nearest;
    }
}