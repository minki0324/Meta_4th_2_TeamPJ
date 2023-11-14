using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
using Pathfinding.Util;

public class Test2 : MonoBehaviour
{
    private Ply_Controller player_Con;
    [SerializeField] private Transform[] Formation_Pos;

    private void Awake()
    {
        player_Con = GetComponent<Ply_Controller>();
    }

    private void Update()
    {
        
        SetFormation();
    }

    private void SetFormation()
    {
        for (int i = 0; i < player_Con.UnitList_List.Count; i++)
        {
            GameObject unit = player_Con.UnitList_List[i];
            Animator anim = unit.GetComponent<Animator>();
            unit.GetComponent<AIDestinationSetter>().target = Formation_Pos[i];
        }
    }

    private void Scan(float scanRange)
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
