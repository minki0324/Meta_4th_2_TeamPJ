using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
using Pathfinding.Util;
using System.Linq;

public class Position
{
   Vector3 position;
    bool dwq = false;

} 
public class Formation_enemy : MonoBehaviour
{

    private LeaderAI leaderAI;
    [SerializeField] private Transform[] Parents_Pos;
    [SerializeField] private Formation_Count[] Count;
    public  List<(Transform transform, float weighted)> weightedParents = new List<(Transform, float)>();
    private bool isUnitsMoving = true;
    private int Parents_index;
    private int succeCount;
    private Transform currentPosGroup;
    private void Awake()
    {
        leaderAI = GetComponent<LeaderAI>();
        currentPosGroup = Parents_Pos[Parents_index];
    }

    private void Update()
    {
        //����׷� 
        //�׷��� �ڸ��� �� ���� ���� �׷����� �̵�
        if(currentPosGroup.childCount == succeCount)
        {
            Parents_index++;
            succeCount = 0;
            currentPosGroup = Parents_Pos[Parents_index];
        }
        List<Position> positions = new List<Position>();
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

    #region ���� ����
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

            // ������ �θ� ������ ã��
            foreach (Transform parent in sortedParents)
            {
                if (parent.childCount != parent.GetComponent<Formation_Count>().Count)
                {
                    selectedParent = parent;
                    break;
                }
            }

            // ������ Ÿ�� ����
            if (selectedParent != null)
            {
                unit.GetComponent<AIDestinationSetter>().target = selectedParent.GetChild(selectedParent.GetComponent<Formation_Count>().Count).transform;
                selectedParent.GetComponent<Formation_Count>().Count++;
            }
            else
            {
                Debug.LogError("No available position for unit: " + unit.name);
                // ��� �θ� ��ġ�� �� á�� ����� ó��
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
        // �÷��̾��� ���� �̵� ����� �ӵ�
       
        //Quaternion rotation = leaderAI.gameObject.transform.rotation;
       
        for (int i = 0; i < leaderAI.UnitList.Count; i++)
        {
            GameObject unit = leaderAI.UnitList[i];

            // ������ ��ġ�� ȸ���� �÷��̾�� ����ȭ
            //Vector3 newPosition = unit.transform.position + playerDirection * speed * Time.deltaTime;

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
    #region ��ĵ �� ��ƿ �޼ҵ��
    // ����� ���ֵ� ��ƿ��� �޼ҵ�
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

    // ������ ��ƿ��� �޼ҵ�
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
   
    // ����ġ ��� ������ ���� �޼ҵ�
    private Transform[] GetSortedParentsByWeight(GameObject unit)
    {
        Vector3 currentPosition = unit.transform.position;
  
        float weightvalue = 0.1f;
        float weight = 1f;
        foreach (Transform parent in Parents_Pos)
        {

            for (int i = 0; i < parent.childCount; i++)
            {
                Transform child = parent.GetChild(i);

                weight += weightvalue;

                weightedParents.Add((child, weight));  // ������ �κ�
            }

        }

        // ����ġ�� �Ÿ��� ���� ����
        weightedParents.Sort((a, b) => a.weightedDistance.CompareTo(b.weightedDistance));

        // ���ĵ� Ʈ������ �迭 ����
        return weightedParents.Select(item => item.transform).ToArray();
    }

    // ����ġ ���� �޼ҵ�
    private float GetWeightForParent(Transform parent)
    {
        int index = Array.IndexOf(Parents_Pos, parent);
        float baseWeight = 1.0f; // �⺻ ����ġ

        if (parent.childCount == parent.GetComponent<Formation_Count>().Count)
        {
            return float.MaxValue; // Ǯ�� ��� �ſ� ���� ����ġ�� �����Ͽ� �켱������ ����
        }
        else if (index >= 0 && index <= 3)
        {
            return baseWeight / 2f; // 1���� 4�� �ε����� ���� ���� ����ġ
        }
        else if (index == 4)
        {
            return baseWeight / 1.2f; // 5�� �ε����� ���� ���� ����ġ
        }
        else if (index >= 5 && index <= 6)
        {
            return baseWeight / 1.5f; // 6���� 7�� �ε����� ���� ���� ����ġ
        }

        return baseWeight; // �������� �⺻ ����ġ
    }

    // ����� ������ ��ƿ��� �޼ҵ�
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
