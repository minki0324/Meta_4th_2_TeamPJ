using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAttack : UnitAttack2
{
    [SerializeField] private float scanRange = 13f;
    private int combinedMask;
    //[SerializeField] public Transform nearestTarget;
    private NavMeshAgent navMeshAgent;
    private Animator ani;
    [SerializeField]private bool isdetecting;

    private float AttackRange = 1.5f;
    private void Awake()
    {
         ani = GetComponent<Animator>();
        navMeshAgent = GetComponent<NavMeshAgent>();
        combinedMask = TargetLayers();
    }
    private void Update()
    {
        switch (leaderState.bat_State)
        {
            case LeaderState.BattleState.Attack:
                AttackOrder();
                break;
            default:
                FollowOrder();
                break;
        }




    }

    private int TargetLayers()
    {
        int[] combinedLayerMask;
        int myLayer = gameObject.layer;
        //총 4개팀의 레이어 
        int[] layerArray = new int[] { LayerMask.NameToLayer("Team"), LayerMask.NameToLayer("Enemy1"), LayerMask.NameToLayer("Enemy2"), LayerMask.NameToLayer("Enemy3") };
        //우리팀의 레이어를 제외한 나머지 레이어를 담을 배열
        combinedLayerMask = new int[3];
        int combinedIndex = 0;


        for (int i = 0; i < layerArray.Length; i++)
        {
            if (myLayer != layerArray[i])
            {
                combinedLayerMask[combinedIndex] = layerArray[i];
                combinedIndex++;
            }

        }
        int layerMask0 = 1 << combinedLayerMask[0];
        int layerMask1 = 1 << combinedLayerMask[1];
        int layerMask2 = 1 << combinedLayerMask[2];
        combinedMask = layerMask0 | layerMask1 | layerMask2;
        return combinedMask;
    }
    Transform GetNearestTarget(RaycastHit[] hits)
    {
        Transform nearest = null;
        float closestDistance = float.MaxValue;

        foreach (RaycastHit hit in hits)
        {
            if (hit.transform.CompareTag("SpawnPoint"))
            {
                continue;
            }
            float distance = Vector3.Distance(transform.position, hit.transform.position);


            if (distance < closestDistance && !hit.transform.CompareTag("SpawnPoint"))
            {
                closestDistance = distance;
                nearest = hit.transform;
            }
        }

        return nearest;
    }

    private void AttackOrder()
    {
        RaycastHit[] allHits = Physics.SphereCastAll(transform.position, scanRange, Vector3.forward, 0, combinedMask);
        nearestTarget = GetNearestTarget(allHits);

        if (nearestTarget != null)
        {

            float attackDistance = Vector3.Distance(transform.position, nearestTarget.position);
            if (attackDistance <= AttackRange)
            {
                isdetecting = true;
            }
            else
            {
                isdetecting = false;
            }


            if (!isdetecting)
            {
                navMeshAgent.isStopped = false;
                ani.SetBool("Move", true);
                navMeshAgent.SetDestination(nearestTarget.transform.position);



            }
            else
            {
                ani.SetBool("Move", false);
                navMeshAgent.isStopped = true;
                if (!isAttacking)
                {
                    attackCoroutine = StartCoroutine(Attack_co());
                    //StartCoroutine(Attack_co());
                }
            }
        }
    }
    private void FollowOrder()
    {
        ani.SetBool("Move", true);
        navMeshAgent.SetDestination(leader.transform.position);
    }
}
