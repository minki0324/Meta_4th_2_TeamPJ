using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
public class AIpathoverride : AIPath
{
    private Soilder_Controller soilder_con;
    protected  override void Start()
    {
        soilder_con = GetComponent<Soilder_Controller>();
    }       
    public override void OnTargetReached()
    {
        if (soilder_con.formationState == Soilder_Controller.FormationState.GoingFormation)
        {
            soilder_con.formationState = Soilder_Controller.FormationState.Shield;
            
        }
    }
}
