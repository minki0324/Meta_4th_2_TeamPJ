using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class LeaderController : MonoBehaviour
{
    public LeaderAction leaderact;
    public Transform Target;
    private NavMeshAgent Nav;

    void Start()
    {
        Nav = GetComponent<NavMeshAgent>();
        leaderact = GetComponent<ToGate>();
        Target = leaderact.TargetSetting();
    }

    void Update()
    {
        
        if (!GameManager.instance.isLive)
        {
            return;
        }
           Nav.SetDestination(Target.position);
        if(Input.GetKeyDown(KeyCode.P))
        {
            leaderact = GetComponent<ToFlag>();
            Target = leaderact.TargetSetting();
        }
    }
}
