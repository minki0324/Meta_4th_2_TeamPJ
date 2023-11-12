using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToFlag : LeaderAction
{
    private Flag[] flags;

    public override Transform TargetSetting()
    {
        flags = FindObjectsOfType<Flag>();

        while (true)
        {
            int Temp = Random.Range(0, 7);
            if (flags[Temp].transform.root.gameObject.layer.Equals(0))
            {
                return flags[Temp].transform.parent.gameObject.transform;
            }
        }
    }
}
