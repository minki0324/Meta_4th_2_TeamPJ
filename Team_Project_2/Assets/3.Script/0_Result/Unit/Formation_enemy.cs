using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
using Pathfinding.Util;
using System.Linq;

public class Formation_enemy : MonoBehaviour
{

    private LeaderAI leaderAI;
    [SerializeField] private Transform[] Parents_Pos;
    [SerializeField] private Formation_Count[] Count;
    private bool isUnitsMoving = true;

    
    private void Awake()
    {
        leaderAI = GetComponent<LeaderAI>();
     
    }

    private void Update()
    {
        //if(Input.GetKeyDown(KeyCode.U))
        //{
        //    Following(100);
        //}
        //else if(Input.GetKeyDown(KeyCode.I))
        //{
        //    SetFormation(100);
        //}
        //else if(Input.GetMouseButton(1))
        //{
        //    Following_Shield(2 );
        //}
        //else if(Input.GetMouseButtonUp(1))
        //{
        //    for (int i = 0; i < leaderAI.UnitList.Count; i++)
        //    {
        //        GameObject unit = leaderAI.UnitList[i];
        //        unit.GetComponent<AIPath>().canSearch = true;
        //    }
        //}
        //else if(Input.GetKeyDown(KeyCode.O))
        //{
        //    isUnitsMoving = !isUnitsMoving;

        //    for (int i = 0; i < leaderAI.UnitList.Count; i++)
        //    {
        //        GameObject unit = leaderAI.UnitList[i];
        //        unit.GetComponent<AIPath>().canSearch = isUnitsMoving;
        //        unit.GetComponent<AIPath>().canMove = isUnitsMoving;
        //    }
        //}
    }

    #region 명령 로직
    public void Following(int scanRange)
    {
        for (int i = 0; i < leaderAI.UnitList.Count; i++)
        {
            GameObject unit = leaderAI.UnitList[i];
            unit.GetComponent<AIDestinationSetter>().target = leaderAI.transform.gameObject.transform;
        }
    }

    public void SetFormation(int scanRange)
    {
        for (int i = 0; i < Count.Length; i++)
        {
            Count[i].Count = 0;
        }

        List<GameObject> unitList = Scan_Pos(scanRange);

        foreach (GameObject unit in unitList)
        {
            Transform[] sortedParents = GetSortedParentsByWeight(unit);
            Transform selectedParent = null;

            // 적절한 부모 포지션 찾기
            foreach (Transform parent in sortedParents)
            {
                if (parent.childCount != parent.GetComponent<Formation_Count>().Count)
                {
                    selectedParent = parent;
                    break;
                }
            }

            // 유닛의 타겟 설정
            if (selectedParent != null)
            {
                unit.GetComponent<AIDestinationSetter>().target = selectedParent.GetChild(selectedParent.GetComponent<Formation_Count>().Count).transform;
                selectedParent.GetComponent<Formation_Count>().Count++;
            }
            else
            {
                Debug.LogError("No available position for unit: " + unit.name);
                // 모든 부모 위치가 꽉 찼을 경우의 처리
            }
        }
    }

    public void Following_Shield(float speed , Vector3 leaderAIDirection)
    {
        for (int i = 0; i < leaderAI.UnitList.Count; i++)
        {
            GameObject unit = leaderAI.UnitList[i];
            //unit.GetComponent<Soilder_Controller>();
            unit.GetComponent<AIDestinationSetter>().target = leaderAI.transform;
            unit.GetComponent<AIPath>().canSearch = false;
            unit.GetComponent<AIPath>().canMove = false;
        }
        // 플레이어의 현재 이동 방향과 속도
       
        //Quaternion rotation = leaderAI.gameObject.transform.rotation;
       
        for (int i = 0; i < leaderAI.UnitList.Count; i++)
        {
            GameObject unit = leaderAI.UnitList[i];

            // 유닛의 위치와 회전을 플레이어와 동기화
            //Vector3 newPosition = unit.transform.position + playerDirection * speed * Time.deltaTime;
            Debug.Log(leaderAIDirection);

            Vector3 newPosition = unit.transform.position + leaderAIDirection * speed * Time.deltaTime;
            unit.transform.position = newPosition;

            //if (playerDirection != Vector3.zero)
            //{
            //    Quaternion newRotation = rotation;
            //    unit.transform.rotation = newRotation;
            //}
        }
    }
    #endregion
    #region 스캔 및 유틸 메소드들
    // 가까운 유닛들 담아오는 메소드
    private List<GameObject> Scan_Unit(float scanRange)
    {
        RaycastHit[] allHits = Physics.SphereCastAll(transform.position, scanRange, Vector3.forward, 0);

        List<GameObject> unitList = new List<GameObject>();

        foreach (RaycastHit hit in allHits)
        {
            GameObject hitObject = GetNearestTarget(allHits).gameObject;
            unitList.Add(hitObject);
        }

        return unitList;
    }

    // 포지션 담아오는 메소드
    private List<GameObject> Scan_Pos(float scanRange)
    {
        RaycastHit[] allHits = Physics.SphereCastAll(transform.position, scanRange, Vector3.forward, 0);
        HashSet<GameObject> uniqueTargets = new HashSet<GameObject>();

        foreach (RaycastHit hit in allHits)
        {
            GameObject hitObject = hit.transform.gameObject;
            if (hitObject.layer == gameObject.layer && hitObject.CompareTag("Soilder"))
              
            {
                if (!uniqueTargets.Contains(hitObject))
                {
                    uniqueTargets.Add(hitObject);
                }
            }
        }

        List<GameObject> unitList = new List<GameObject>(uniqueTargets);
        unitList.Sort((a, b) => Vector3.Distance(transform.position, a.transform.position).CompareTo(Vector3.Distance(transform.position, b.transform.position)));

        return unitList;
    }
   
    // 가중치 기반 포지션 정렬 메소드
    private Transform[] GetSortedParentsByWeight(GameObject unit)
    {
        Vector3 currentPosition = unit.transform.position;
        List<(Transform transform, float weightedDistance)> weightedParents = new List<(Transform, float)>();

        foreach (Transform parent in Parents_Pos)
        {
            float distance = Vector3.Distance(currentPosition, parent.position);
            float weight = GetWeightForParent(parent);

            weightedParents.Add((parent, distance * weight));
        }

        // 가중치된 거리에 따라 정렬
        weightedParents.Sort((a, b) => a.weightedDistance.CompareTo(b.weightedDistance));

        // 정렬된 트랜스폼 배열 추출
        return weightedParents.Select(item => item.transform).ToArray();
    }

    // 가중치 설정 메소드
    private float GetWeightForParent(Transform parent)
    {
        int index = Array.IndexOf(Parents_Pos, parent);
        float baseWeight = 1.0f; // 기본 가중치

        if (parent.childCount == parent.GetComponent<Formation_Count>().Count)
        {
            return float.MaxValue; // 풀일 경우 매우 높은 가중치를 설정하여 우선순위를 낮춤
        }
        else if (index >= 0 && index <= 3)
        {
            return baseWeight / 2f; // 1부터 4번 인덱스에 대한 낮은 가중치
        }
        else if (index == 4)
        {
            return baseWeight / 1.2f; // 5번 인덱스에 대한 낮은 가중치
        }
        else if (index >= 5 && index <= 6)
        {
            return baseWeight / 1.5f; // 6에서 7번 인덱스에 대한 낮은 가중치
        }
        return baseWeight; // 나머지는 기본 가중치
    }

    // 가까운 포지션 담아오는 메소드
    private Transform GetNearestTarget(RaycastHit[] hits)
    {
        Transform nearest = null;
        float closestDistance = float.MaxValue;

        foreach (RaycastHit hit in hits)
        {
            if (hit.transform.CompareTag("SpawnPoint") &&
                !hit.transform.CompareTag("Player") && 
                !hit.transform.CompareTag("Leader") &&
                !hit.transform.CompareTag("Base"))
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
    #endregion

    
}
