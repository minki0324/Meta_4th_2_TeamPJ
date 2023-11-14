using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Test3 : MonoBehaviour
{
    enum State
    {
        For_Following,
        For_CloseOrder,
        For_Wait
    }

    [SerializeField] private Ply_Controller player_con;
    [SerializeField] private Ply_Movement player_move;
    [SerializeField] private LeaderState Leader;
    [SerializeField] private GameObject Forward;
    [SerializeField] private List<GameObject> Stop_List;

    private Coroutine slowDownCoroutine;



    private void Awake()
    {
        player_con = GetComponent<Ply_Controller>();
        player_move = GetComponent<Ply_Movement>();
        Leader = GetComponent<LeaderState>();
    }

    private void Update()
    {
        if(!GameManager.instance.isLive)
        {
            return;
        }

        if(Input.GetMouseButton(1))
        {
            Following_Shield(2f);
        }
        else
        {
            Following(5f);
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

    public void Following(float speed)
    {
        Vector3 playerDirection = player_move.MoveDir.normalized;

        float offset = 1f;
        float someThreshold = 2.5f;
        Vector3 right = player_con.transform.right;
        Vector3 left = player_con.transform.right * -1;

        Vector3 playerPos = player_con.transform.position + right * offset;
        Vector3 playerPos2 = player_con.transform.position + left * offset;
        Vector3 playerPos3 = player_con.transform.position + right * (offset*3);
        Vector3 playerPos4 = player_con.transform.position + left * (offset*3);
        Vector3 playerPos5 = player_con.transform.position + right * (offset*5);
        Vector3 playerPos6 = player_con.transform.position + left * (offset*5);

        Vector3[] positions = { playerPos, playerPos2, playerPos3, playerPos4, playerPos5, playerPos6 };
        GameObject nearestUnit = Scan(4f);

        if (nearestUnit != null)
        {
            for (int i = 0; i < player_con.UnitList_List.Count; i++)
            {
                GameObject unit = player_con.UnitList_List[i];
                Animator anim = unit.GetComponent<Animator>();
                NavMeshAgent agent = unit.GetComponent<NavMeshAgent>();

                Vector3 destination = positions[i % positions.Length];
                Debug.DrawLine(unit.transform.position, destination, Color.red, 0.1f, false);

                float distanceToDestination = Vector3.Distance(unit.transform.position, destination);

                if (distanceToDestination < someThreshold) // someThreshold는 유닛이 멈춰야 할 거리
                {
                    if (unit.GetComponent<CorutineHolder>().currentCoroutine == null)
                    {
                        Coroutine coroutine = StartCoroutine(SlowDown(agent, 1.0f, anim));
                        agent.obstacleAvoidanceType = ObstacleAvoidanceType.LowQualityObstacleAvoidance;
                        unit.GetComponent<CorutineHolder>().currentCoroutine = coroutine;
                    }
                }
                else
                {
                    if (unit.GetComponent<CorutineHolder>().currentCoroutine != null)
                    {
                        StopCoroutine(unit.GetComponent<CorutineHolder>().currentCoroutine);
                        unit.GetComponent<CorutineHolder>().currentCoroutine = null;
                    }
                    anim.SetBool("Move", true);
                    agent.SetDestination(destination);
                    agent.obstacleAvoidanceType = ObstacleAvoidanceType.HighQualityObstacleAvoidance;
                }
            }
        }
        
    }

    private IEnumerator SlowDown(NavMeshAgent agent, float duration, Animator anim)
    {
        float time = 0;
        Vector3 startVelocity = agent.velocity;

        while (time < duration)
        {
            agent.velocity = Vector3.Lerp(startVelocity, Vector3.zero, time / duration);
            time += Time.deltaTime;
            yield return null;
        }

        agent.velocity = Vector3.zero;
        anim.SetBool("Move", false);
    }

    public int Compare2(GameObject a, GameObject b, GameObject dest)
    {
        float lengthA_Dest = Vector3.Distance(a.transform.position, dest.transform.position);
        float lengthB_Dest = Vector3.Distance(b.transform.position, dest.transform.position);

        return lengthA_Dest < lengthB_Dest ? -1 : 1;
    }

    private GameObject Scan(float scanRange)
    {
        RaycastHit[] allHits = Physics.SphereCastAll(transform.position, scanRange, Vector3.forward, 0);
        Transform nearestTarget = null;
        if (allHits != null)
        {
            foreach (RaycastHit hit in allHits)
            {
                if (hit.transform.gameObject.layer == gameObject.layer)
                {
                    nearestTarget = GetNearestTarget(allHits);
                }
            }
        }
        return nearestTarget.gameObject;
    }

    private Transform GetNearestTarget(RaycastHit[] hits)
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
}
