using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetMyBase : Targetsetting
{
    public override Transform Target(Transform StartPos)
    {
        ListTemp = new List<GameObject>();
        for (int i = 0; i < MapInfo.baseCampPositions.Count; i++)
        {
            if (MapInfo.baseCampPositions[i].layer.Equals(gameObject.layer))
            {
                ListTemp.Add(MapInfo.baseCampPositions[i]);
            }
        }

        if (ListTemp.Count > 0)
        {
            TargetBase = ListTemp[0];

            if (ListTemp.Count > 1)
            {
                float Distance = Vector3.SqrMagnitude(StartPos.position - ListTemp[0].transform.position);

                for (int i = 1; i < ListTemp.Count; i++)
                {
                    float DistanceTemp = Vector3.SqrMagnitude(StartPos.position - ListTemp[i].transform.position);
                    if (DistanceTemp < Distance) // 거리가 더 짧을 때
                    {
                        TargetBase = MapInfo.baseCampPositions[i];
                        Distance = DistanceTemp;
                    }

                }
            }
            return TargetBase.transform;
        }
        return null;
    }

}
