using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetManager : MonoBehaviour
{
    public TargetSetting targetset;

    public void Move(Transform StartPos)
    {
        targetset.Move(StartPos);
    }
    public void SetTarget(TargetSetting targetset)
    {
        this.targetset = targetset;
    }
}
