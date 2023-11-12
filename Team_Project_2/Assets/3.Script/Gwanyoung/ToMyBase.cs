using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToMyBase : LeaderAction
{
    private Flag[] flags;
    public override Transform TargetSetting()
    {
        flags = FindObjectsOfType<Flag>();
        for (int i = 0; i < flags.Length; i++)
        {
            if (flags[i].transform.root.gameObject.CompareTag("Base") && flags[i].transform.root.gameObject.layer.Equals(this.gameObject.layer))
            {
                return null;
            }
        }
        return null;

    }
}
