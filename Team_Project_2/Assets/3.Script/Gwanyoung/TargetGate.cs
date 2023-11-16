using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetGate : Targetsetting
{
    public override Transform Target(Transform StartPos)
    {        
        ListTemp = new List<GameObject>();
        TargetGate = gates[0];
        float Distance = Vector3.SqrMagnitude(StartPos.position - gates[0].transform.position); // 비교용으로 0번째 배열 게이트 거리구하기

        for (int i = 1; i < gates.Length; i++)
        {
            float DistanceTemp = Vector3.SqrMagnitude(StartPos.position - gates[i].transform.position);
            if (DistanceTemp < Distance) // 거리가 더 짧을 때
            {
                TargetGate = gates[i];
                Distance = DistanceTemp;
            }
        }
       
        return TargetGate.transform;
    }

}
