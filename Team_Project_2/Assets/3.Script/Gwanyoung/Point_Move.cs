using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Point_Move : MonoBehaviour
{
    // 포인트지점까지 이동

    public GameObject FlagPoint; // 플래그포인트
    public NavMeshAgent navMesh;
    private Vector3 currentPos;
    private int Flag_Num;

    private bool isMove = false;  // 이동 중인지

    private void Start()
    {

        navMesh = GetComponent<NavMeshAgent>();
       


    }
    private void Update()
    {

        navMesh.SetDestination(FlagPoint.transform.position);
    }
}
