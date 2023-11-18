using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetGate : Targetsetting
{
    public override Transform Target(Transform StartPos)
    {        
        ListTemp = new List<GameObject>();
        TargetGate = gates[0];
        float Distance = Vector3.SqrMagnitude(StartPos.position - gates[0].transform.position); // �񱳿����� 0��° �迭 ����Ʈ �Ÿ����ϱ�

        for (int i = 1; i < gates.Length; i++)
        {
            float DistanceTemp = Vector3.SqrMagnitude(StartPos.position - gates[i].transform.position);
            if (DistanceTemp < Distance) // �Ÿ��� �� ª�� ��
            {
                TargetGate = gates[i];
                Distance = DistanceTemp;
            }
        }
       
        return TargetGate.transform;
    }

}
