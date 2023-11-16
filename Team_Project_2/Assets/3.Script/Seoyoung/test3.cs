using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class test3 : MonoBehaviour
{
    [SerializeField] private Ply_Controller player_con;
    [SerializeField] private Ply_Movement player_move;
    [SerializeField] private LeaderState Leader;
    [SerializeField] private GameObject Forward;
    [SerializeField] private List<GameObject> Stop_List;
    private bool isStop = true;

    private int playernum = 0;
    WaitForSeconds ForSeconds = new WaitForSeconds(0.1f);

    private void Awake()
    {
        player_con = GetComponent<Ply_Controller>();
        player_move = GetComponent<Ply_Movement>();
        Leader = GetComponent<LeaderState>();
    }

    private void Update()
    {
        if (!GameManager.instance.isLive)
        {
            return;
        }

        if (Input.GetMouseButton(1))
        {
            Following_Shield(2.5f);
        }
        else
        {
            StartCoroutine(Following(4.5f));
        }
    }

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
    private bool isTarget
    {

        get
        {
            if (GameManager.instance.isLive)
            {
                return true;
            }
            return false;
        }

    }

    public IEnumerator Following(float speed)
    {
        if(player_move.isPlayerMove)
        {

            #region 자연스러운 이동
          
            if (isTarget)
            {
                player_con.gameObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotation;

                for (int i = 0; i < player_con.UnitList_List.Count; i++)
                {
                    Animator anim = player_con.UnitList_List[i].GetComponent<Animator>();
                    player_con.UnitList_List[i].GetComponent<UnitAttack2>().isClose = false;
                    player_con.UnitList_List[i].GetComponent<NavMeshAgent>().isStopped = false;
                    anim.SetBool("Move", true);
                }


                player_con.UnitList_List.Sort(delegate (GameObject a, GameObject b)
                {
                    return Compare2(a, b, player_con.gameObject);
                });


                for (int i = 0; i < player_con.UnitList_List.Count; i++)
                {
                    player_con.UnitList_List[i].GetComponent<UnityEngine.AI.NavMeshAgent>().obstacleAvoidanceType = ObstacleAvoidanceType.HighQualityObstacleAvoidance;
                    if (i == 0)
                    {
                        player_con.UnitList_List[i].GetComponent<UnityEngine.AI.NavMeshAgent>().SetDestination(player_con.transform.position + Vector3.back);
                    }
                    else
                    {

                        player_con.UnitList_List[i].GetComponent<UnityEngine.AI.NavMeshAgent>().SetDestination(player_con.transform.position + Vector3.back);
                    }

                }

            }
            #endregion
        }
        else
        {
            Vector3 playerDirection = player_move.MoveDir.normalized;

            float offset = 1f;
            float stoppingDistance = 2f; // 병사가 멈추기 시작할 거리

            Vector3 right = player_con.transform.right;
            Vector3 left = player_con.transform.right * -1;


            Vector3 playerPos = player_con.transform.position + right * offset;
            Vector3 playerPos2 = player_con.transform.position + left * offset;
            Vector3 playerPos3 = player_con.transform.position + right * (offset * 2);
            Vector3 playerPos4 = player_con.transform.position + left * (offset * 2);

            //player_con.UnitList_List.Sort(delegate (GameObject a, GameObject b)
            //{
            //    return Compare2(a, b, Forward);
            //});

            for (int i = 0; i < player_con.UnitList_List.Count; i++)
            {

                GameObject unit = player_con.UnitList_List[i];
                Animator anim = unit.GetComponent<Animator>();
                NavMeshAgent agent = unit.GetComponent<NavMeshAgent>();

                float distanceToDestination = Vector3.Distance(unit.transform.position, playerPos);

                if (distanceToDestination > stoppingDistance)
                {

                    anim.SetBool("Move", true);
                    agent.obstacleAvoidanceType = ObstacleAvoidanceType.HighQualityObstacleAvoidance;
                    if (i < 5)
                    {
                        if (i % 4 == 0)
                        {
                            agent.SetDestination(player_con.transform.position + right * offset);
                        }
                        else if (i % 3 == 0)
                        {
                            agent.SetDestination(player_con.transform.position + right * offset);
                        }
                        else if (i % 2 == 0)
                        {
                            agent.SetDestination(player_con.transform.position + left * (offset * 2));
                        }
                        else
                        {
                            agent.SetDestination(player_con.transform.position + right * (offset * 2));
                        }
                    }
                    else
                    {
                        Vector3 back = player_con.UnitList_List[i - 5].transform.forward * -1f;
                        Vector3 Pos5 = player_con.UnitList_List[i - 5].transform.position + back * (offset * 2);
                        agent.SetDestination(Pos5);
                    }

                    agent.speed = speed;
                }
                else
                {

                    anim.SetBool("Move", false);
                    agent.velocity = Vector3.zero; // 병사가 도착했으므로 속도를 0으로 설정
                    agent.obstacleAvoidanceType = ObstacleAvoidanceType.NoObstacleAvoidance;
                    if (playerDirection != Vector3.zero)
                    {
                        Quaternion newRotation = Quaternion.LookRotation(playerDirection);
                        unit.transform.rotation = newRotation;
                    }


                }


            }
        }

        yield return null;
    }
    Transform GetNearestTarget(RaycastHit[] hits)
    {
        Debug.Log(hits);
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
    public int Compare(Vector3 a, Vector3 b, Vector3 dest)
    {
        float lengthA_Dest = Vector3.Distance(a, dest);
        float lengthB_Dest = Vector3.Distance(a, dest);

        return lengthA_Dest < lengthB_Dest ? -1 : 1;
    }

    public int Compare2(GameObject a, GameObject b, GameObject dest)
    {
        float lengthA_Dest = Vector3.Distance(a.transform.position, dest.transform.position);
        float lengthB_Dest = Vector3.Distance(b.transform.position, dest.transform.position);

        return lengthA_Dest < lengthB_Dest ? -1 : 1;
    }
}