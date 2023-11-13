using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Test3 : MonoBehaviour
{
    [SerializeField] private Ply_Controller player_con;
    [SerializeField] private Ply_Movement player_move;
    [SerializeField] private LeaderState Leader;

    private float Min = -210f;
    private float Max = 210f;

    private void Awake()
    {
        player_con = GetComponent<Ply_Controller>();
        player_move = GetComponent<Ply_Movement>();
        Leader = GetComponent<LeaderState>();
    }

    private void Update()
    {
        if(Input.GetMouseButton(1))
        {
            Following(2.5f);
        }
        else
        {
            Following(4.5f);
        }
    }

    /*public void Following()
    {
        float offset = 1f;

        Vector3 playerBack = player.transform.position - player.transform.forward * offset; // offset은 플레이어와 병사 사이의 거리

        for (int i = 0; i < player.UnitList_List.Count; i++)
        {
            player.UnitList_List[i].GetComponent<NavMeshAgent>().SetDestination(playerBack);
        }
    }*/

    public void Following_Shield(float speed)
    {
        // 플레이어의 현재 이동 방향과 속도
        Vector3 playerDirection = player_move.MoveDir.normalized;

        for (int i = 0; i < player_con.UnitList_List.Count; i++)
        {
            GameObject unit = player_con.UnitList_List[i];

            // 유닛의 위치와 회전을 플레이어와 동기화
            Vector3 newPosition = unit.transform.position + playerDirection * speed * Time.deltaTime;
            unit.transform.position = newPosition;

            if (playerDirection != Vector3.zero)
            {
                Quaternion newRotation = Quaternion.LookRotation(playerDirection);
                unit.transform.rotation = newRotation;
            }
        }
    }

    public void Following(float speed)
    {
        float offset = 1f;
        float stoppingDistance = 2.5f; // 병사가 멈추기 시작할 거리

        Vector3 playerBack = player_con.transform.position - player_con.transform.forward * offset;

        for (int i = 0; i < player_con.UnitList_List.Count; i++)
        {
            GameObject unit = player_con.UnitList_List[i];
            NavMeshAgent agent = unit.GetComponent<NavMeshAgent>();

            float distanceToDestination = Vector3.Distance(unit.transform.position, playerBack);

            if (distanceToDestination > stoppingDistance)
            {
                agent.SetDestination(playerBack);
                agent.speed = speed;
            }
            else
            {
                agent.velocity = Vector3.zero; // 병사가 도착했으므로 속도를 0으로 설정
            }
        }
    }
}
