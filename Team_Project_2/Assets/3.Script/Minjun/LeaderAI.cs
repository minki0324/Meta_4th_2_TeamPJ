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
    private Animator ani;
    //public Transform nearestTarget;
    private float AttackRange = 5f;


    public bool isEnermyChecked = false;


    private void Awake()
    {
        ani = GetComponent<Animator>();
        combinedMask = TargetLayers();
        navMesh = GetComponent<NavMeshAgent>();
        bat_State = BattleState.Follow;
    }
    private void Update()
    {
        // �׻� �ֺ��� �����ִ��� Ž��
        EnemyDitect();
        switch (bat_State)
        {
            case BattleState.Follow:
                //navMesh.isStopped = true;
                break;
            case BattleState.Attack:
              
                break;
        }

        switch (jud_State)
        {
            case JudgmentState.Ready:
                ani_State = AniState.Idle;
                break;
            case JudgmentState.wait:
                break;
            case JudgmentState.Ditect:
                //�ִϸ��̼� ���е��
                ani_State = AniState.shild;
                //õõ�� ������ ����
                //Debug.Log()
                ani.SetBool("Move" , true);
                navMesh.SetDestination(nearestTarget.position);

                //float originalSpeed = navMeshAgent.speed; // ���� �ӵ� ����
                //navMeshAgent.speed = originalSpeed / 4; // 1/4�� ���� �ӵ� ����


                break;
        }



    }

    private void EnemyDitect()
    {
        RaycastHit[] hits = Physics.SphereCastAll(transform.position, scanRange, Vector3.forward, 0, combinedMask);
      
        nearestTarget = GetNearestTarget(hits);

        if(nearestTarget)
        {          
            float attackDistance = Vector3.Distance(transform.position, nearestTarget.position);
            jud_State = JudgmentState.Ditect;
           
            //DItect �����϶� ���и� ��� õõ�� ����
            if (attackDistance <= AttackRange)
            {
                bat_State = BattleState.Attack;
            }

           
            isEnermyChecked = true;
        }
        else
        {         
            jud_State = JudgmentState.Ready;
            bat_State = BattleState.Follow;


            isEnermyChecked = false;   
        }

       
        //�������� ���ݶ��̴��� ������ Ditect ���·� ����
    }
  

}   