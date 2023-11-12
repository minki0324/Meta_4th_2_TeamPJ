using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToGate : LeaderAction
{
    private Gate[] gates;

    public override Transform TargetSetting()
    {
        gates = FindObjectsOfType<Gate>();
        for (int i = 0; i < gates.Length; i++)
        {
            if (gates[i].transform.root.gameObject.layer.Equals(this.gameObject.layer) && !transform.root.gameObject.CompareTag("Base"))
            {
                return gates[i].transform;
            }
        }
        return null;
    }

}
