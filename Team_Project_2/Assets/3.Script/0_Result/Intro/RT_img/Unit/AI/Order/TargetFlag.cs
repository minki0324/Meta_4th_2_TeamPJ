using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetFlag : Targetsetting
{
    public override Transform Target(Transform StartPos)
    {
        ListTemp = new List<GameObject>();
        for (int i = 0; i < MapInfo.flagPositions_List.Count; i++)
        {
            if (!MapInfo.flagPositions_List[i].layer.Equals(gameObject.layer))
            {
                ListTemp.Add(MapInfo.flagPositions_List[i]);
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
                    float DistanceListTemp = Vector3.SqrMagnitude(StartPos.position - ListTemp[i].transform.position);
                    if (DistanceListTemp < Distance) // 거리가 더 짧을 때
                    {
                        TargetBase = ListTemp[i];
                        Distance = DistanceListTemp;
                    }

                }
            }
            return TargetBase.transform;

        }
        return null;


    }


}
