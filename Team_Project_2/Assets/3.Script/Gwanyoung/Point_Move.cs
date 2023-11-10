using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Point_Move : MonoBehaviour
{
    // 포인트지점까지 이동

    public GameObject[] FlagPoint; // 플래그포인트
    private NavMeshAgent navMesh;
    private Vector3 currentPos;
    private int Flag_Num;

    private bool isMove = false;  // 이동 중인지

    private void Start()
    {
        FlagPoint = GameObject.FindGameObjectsWithTag("Flag");
        navMesh = GetComponent<NavMeshAgent>();
        
    }
    private void Update()
    {

        if(!GameManager.instance.isLive)
        {
            return;
        }

        if (!isMove)
        {
            Flag_Num = Random.Range(0, FlagPoint.Length);

            if (FlagPoint[Flag_Num].layer != gameObject.layer) 
            {
                isMove = true;
                navMesh.SetDestination(FlagPoint[Flag_Num].transform.position);   // 랜덤으로 Flag를 향해 이동
            }
        }
        else
        {
            currentPos = FlagPoint[Flag_Num].transform.position - transform.position;  // 두 포지션의 차이
            // Flag 좌표와 리더 좌표가 1.5씩 차이날 때
            if (Mathf.Abs(currentPos.x) <= 1.5f && Mathf.Abs(currentPos.z) <= 1.5f && FlagPoint[Flag_Num].layer == gameObject.layer) 
            {
                isMove = false;               
            }
        }

    }
}
