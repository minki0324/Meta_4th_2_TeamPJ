using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateRetreat : MonoBehaviour, IState
{

    TargetSetting targetset;
    private void Start()
    {
        targetset = GetComponent<TargetSetting>();
    }


    public void OperEnter()
    {
        
    }

    public void OperExit()
    {
    }

    public void OperStay()
    {
        

    }

}
