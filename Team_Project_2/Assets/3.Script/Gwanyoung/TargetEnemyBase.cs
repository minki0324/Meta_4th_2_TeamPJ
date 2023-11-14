using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetEnemyBase : Targetsetting
{
    public override Transform Target(Transform StartPos)
    {
        ListTemp = new List<GameObject>();
        for (int i = 0; i < MapInfo.baseCampPositions.Count; i++)
        {
            if (!MapInfo.baseCampPositions[i].layer.Equals(gameObject.layer))
            {
                ListTemp.Add(MapInfo.baseCampPositions[i]);
            }
        }
        TargetBase = ListTemp[Random.Range(0, ListTemp.Count)];
        return TargetBase.transform;
    }

}
