using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateManager : MonoBehaviour
{

    public IState CurrentState { get; private set; }

    public StateManager(IState defaultState)
    {
        CurrentState = defaultState;
    }

    // 상태 바뀔 때 세팅
    public void SetState(IState state)
    {
        CurrentState.OperExit();

        CurrentState = state;

        CurrentState.OperEnter();
    }

    // 업데이트 될 메서드
    public void DoOperStay()
    {
        CurrentState.OperStay(); 
    }
}

public interface IState
{
    void OperEnter();    // 상태가 시작될 때
    void OperStay();     // 상태 지속 중일 때
    void OperExit();     // 상태가 끝날 때
}