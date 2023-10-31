using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class test : MonoBehaviour
{
    private NavMeshAgent agent;
    private Player_Controller pc;
    void Start()
    {
        pc = FindObjectOfType<Player_Controller>();
        agent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        agent.SetDestination(pc.transform.position);
    }
}
