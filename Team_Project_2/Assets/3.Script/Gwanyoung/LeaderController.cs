using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class LeaderController : MonoBehaviour
{
    private NavMeshAgent Nav;

    public StateManager stateManager;

    private enum LeaderAIState
    {
        Charge = 0, // 돌격 (적과 필드조우)
        Retreat,    // 후퇴 (필드에서 단순 물러남)
        Improve,    // 정비
        Occu_Flag,  // 깃발 점령  
        Occu_Enemy  // 상대 진영 점령
    }

    private Dictionary<LeaderAIState, IState> dicState = new Dictionary<LeaderAIState, IState>();

    private void Start()
    {
        TryGetComponent<NavMeshAgent>(out Nav);

        IState charge = new StateCharge();
        IState retreat = new StateRetreat();
        IState improve = new StateImprove();
        IState occu_Flag = new StateOccuFlag();
        IState occu_Enemy = new StateOccuEnemy();

        // Dictionary 에 저장
        dicState.Add(LeaderAIState.Charge, charge);
        dicState.Add(LeaderAIState.Retreat, retreat);
        dicState.Add(LeaderAIState.Improve, improve);
        dicState.Add(LeaderAIState.Occu_Flag, occu_Flag);
        dicState.Add(LeaderAIState.Occu_Enemy, occu_Enemy);

        stateManager = new StateManager(occu_Flag);

    }

    private void Update()
    {
        stateManager.DoOperStay();

        if (Input.GetKeyDown(KeyCode.F1))
        {
            stateManager.SetState(dicState[LeaderAIState.Charge]);
        }
        if (Input.GetKeyDown(KeyCode.F2))
        {
            stateManager.SetState(dicState[LeaderAIState.Retreat]);
        }
        if (Input.GetKeyDown(KeyCode.F3))
        {
            stateManager.SetState(dicState[LeaderAIState.Improve]);
        }
        if (Input.GetKeyDown(KeyCode.F4))
        {
            stateManager.SetState(dicState[LeaderAIState.Occu_Flag]);
        }
        if (Input.GetKeyDown(KeyCode.F5))
        {
            stateManager.SetState(dicState[LeaderAIState.Occu_Enemy]);
        }
    }
}


