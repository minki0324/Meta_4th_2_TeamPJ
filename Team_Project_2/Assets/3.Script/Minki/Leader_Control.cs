using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class Leader_Control : MonoBehaviour
{
    public enum AIState
    {
        NonTarget,
        Target_Gate,
        Target_Flag,
        Target_Base,
        Target_Enemy
    }

    public AIState state;
    private LeaderAI ai;
    private AIPath aiPath;
    public AIDestinationSetter Main_target;
    public Targetsetting targetsetting;
    public Transform Target;
    public Transform NextTarget;

    public bool NearGate = false;
    public bool NearFlag = false;
    public bool NearBase = true;
    public bool isStart = false;

    private float scanRange = 2f;
    private float State_CoolTime;

    private void Start()
    {
        ai = GetComponent<LeaderAI>();
        aiPath = GetComponent<AIPath>();
        Main_target = GetComponent<AIDestinationSetter>();
    }

    private void Update()
    {
        if(!GameManager.instance.isLive)
        {
            return;
        }

        Scan_targetPos();
        Change_State();
        State_CoolTime += Time.deltaTime;

        Debug.Log($"{gameObject.name} : {state} : {ai.bat_State} ");

        if (State_CoolTime > 0.2f)
        {
            switch (state)
            {
                case AIState.NonTarget:
                    Search_Target();
                    break;
                case AIState.Target_Gate:
                    IfTarget_Gate();
                    break;
                case AIState.Target_Base:
                    IfTarget_Base();
                    break;
                case AIState.Target_Flag:
                    IfTarget_Flag();
                    break;
                case AIState.Target_Enemy:
                    break;
                default:
                    break;
            }
            State_CoolTime = 0;
        }
    }

    // 논타겟일때 타겟을 설정해주는 메소드
    public void Search_Target()
    {
        // ai 타겟이 없을 때
        switch(ai.bat_State)
        {
            case LeaderState.BattleState.Attack:
                ai.bat_State = LeaderState.BattleState.Move;
                break;
            case LeaderState.BattleState.Detect:
                ai.bat_State = LeaderState.BattleState.Move;
                break;
            case LeaderState.BattleState.Move:
                /*
                    1. 현재 본인의 병사 숫자 체크해서 병사 숫자가 적으면 가까운 스폰포인트로 이동해서 병사 채움
                    2. 병사 숫자가 15명 이상이라면 가장 가까운 깃발로 이동해서 점령 준비
                    3. 깃발에 도착했다면 Wait 상태로 전환해서 점령이 끝날때까지 대기
                */     
                if(!Check_Soldier())
                {
                    Return_SpawnPoint(transform, ref Target);
                }
                else if(Check_Soldier())
                {
                    if(!NearFlag)
                    {
                        if(NearBase)
                        {

                        }
                        else
                        {
                            Move_Flag(transform, ref Target);
                        }
                    }
                    else if(NearFlag && !NearBase)
                    {
                        Debug.Log("들어옴?");
                        Move_Flag(transform, ref Target);
                    }
                    else if(NearFlag && NearBase)
                    {
                        Move_Gate(transform, ref Target);
                    }
                }
                break;
            case LeaderState.BattleState.Wait:
                if (!Check_Soldier())
                {
                    Return_SpawnPoint(transform, ref Target);
                }
                else if (Check_Soldier())
                {
                    if (NearFlag)
                    {
                        Move_Flag(transform, ref Target);
                        ai.bat_State = LeaderState.BattleState.Move;
                    }
                }
                break;
        }
        
    }
    // 타겟이 베이스일때 다음 행동을 설정해주는 메소드
    public void IfTarget_Base()
    {
        switch(ai.bat_State)
        {
            case LeaderState.BattleState.Attack:
                break;
            case LeaderState.BattleState.Detect:
                break;
            case LeaderState.BattleState.Move:
                /*
                    1. 현재 본인의 병사 수를 체크하여 병사 숫자가 15마리 이하라면 본인 주변의 스폰 포인트로 이동
                    2. isStart가 false이면 아직 본진에서 빠져나온게 아니라 isStart를 켜주면서 Gate로 이동
                    3. 현재 병사 숫자가 15마리 이상이라면 그대로 목적지로 이동
                */
                if(!Check_Soldier() && !NearBase)
                {
                    Return_SpawnPoint(transform, ref Target);
                }
                else if(!Check_Soldier() && NearBase)
                {
                    ai.bat_State = LeaderState.BattleState.Wait;
                }
                else if(Check_Soldier())
                {
                    if(NearBase)
                    {
                        Move_Gate(transform, ref Target);
                    }
                    else
                    {
                        break;
                    }
                }
                break;
            case LeaderState.BattleState.Wait:
                /*
                    1. 현재 본인의 병사 수를 체크하여 병사 숫자가 15마리 이하라면 대기
                    2. 병사 숫자가 충분히 채워졌으면 게이트로 이동
                */
                Debug.Log($"{gameObject.name} : {Check_Soldier()}");
                if(!Check_Soldier())
                {
                    break;
                }
                else if(Check_Soldier())
                {
                    Move_Gate(transform, ref Target);
                    ai.bat_State = LeaderState.BattleState.Move;
                }
                break;
        }
    }
    // 타겟이 게이트일때 다음 행동을 설정해주는 메소드
    public void IfTarget_Gate()
    {
        switch(ai.bat_State)
        {
            case LeaderState.BattleState.Attack:
                break;
            case LeaderState.BattleState.Detect:
                break;
            case LeaderState.BattleState.Move:
                /*
                    1. 현재 병사수가 15마리 이하라면 가까운 베이스로 이동
                    2. 다음 목적지가 플래그인데 일단 게이트 주변에 와야 길 진행이 가능하기 때문에 neargate 불값에 따라서 움직임을 설정
                    3. neargate값이 true면 move_flag 메소드 호출
                    4. neargate값이 false면 break로 가까이 올때까지 대기
                */
                if(!Check_Soldier())
                {
                    Return_SpawnPoint(transform, ref Target);
                }
                else if(Check_Soldier())
                {
                    if(NearGate)
                    {
                        Move_Flag(transform, ref Target);
                        ai.bat_State = LeaderState.BattleState.Move;
                    }
                    else
                    {
                        break;
                    }
                }
                break;
            case LeaderState.BattleState.Wait:
                /*
                    1. 게이트가 목적지라면 wait는 필요없기때문에 버그 
                */
                ai.bat_State = LeaderState.BattleState.Move;
                break;
        }
    }
    // 타겟이 플래그일때 다음 행동을 설정해주는 메소드
    public void IfTarget_Flag()
    {
        switch(ai.bat_State)
        {
            case LeaderState.BattleState.Attack:
                break;
            case LeaderState.BattleState.Detect:
                break;
            case LeaderState.BattleState.Move:
                /*
                    1. 현재 병사수 체크해서 15마리 이하라면 가까운 스폰 포인트로 이동
                    2. nearFlag 불값으로 플래그에 가까워졌는지 아닌지 확인
                    3. nearFlag 트루면 점령중이므로 Wait상태로 전환
                    4. nearFlag 펄스면 이동중이므로 break;
                */
                if(!Check_Soldier())
                {
                    Return_SpawnPoint(transform, ref Target);
                }
                else if(Check_Soldier())
                {
                    if(NearFlag)
                    {
                        if(!isOccuDone())
                        {
                            Debug.Log("불리니?");
                            ai.bat_State = LeaderState.BattleState.Wait;
                        }
                        else if(isOccuDone())
                        {
                            Debug.Log("다음거 안감 ?");
                            Move_Flag(transform, ref Target);
                        }
                    }
                    else if(!NearFlag)
                    {
                        break;
                    }
                }
                break;
            case LeaderState.BattleState.Wait:
                /*
                    1. 현재 병사수 체크해서 15마리 이하라면 가까운 스폰 포인트로 이동
                    2. nearFlag가 펄스라면 배틀 스테이트 Move로 바꿔줌
                    3. 점령을 완료했다면 배틀 스테이트를 move로 바꿔줌
                    4. 아직 점령중이라면 break;
                */
                if(!Check_Soldier())
                {
                    Return_SpawnPoint(transform, ref Target);
                }
                else if(Check_Soldier())
                {
                    if(!NearFlag)
                    {
                        ai.bat_State = LeaderState.BattleState.Move;
                    }
                    else if(NearFlag)
                    {
                        if(isOccuDone())
                        {
                            Move_Flag(transform, ref Target);
                            ai.bat_State = LeaderState.BattleState.Move;
                        }
                        else if(!isOccuDone())
                        {
                            break;
                        }
                    }
                }

                break;
        }
    }
    public bool Check_Soldier()
    {
        if(ai.currentUnitCount >= 15)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private bool isOccuDone()
    {
        if(Target.gameObject.layer.Equals(gameObject.layer))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private void Return_SpawnPoint(Transform StartPos, ref Transform Target)
    {
        aiPath.canMove = true;
        aiPath.canSearch = true;
        targetsetting = GetComponent<TargetMyBase>();
        Target = targetsetting.Target(StartPos);
        ToTarget(Target);
    }

    private void Move_Flag(Transform StartPos, ref Transform Target)
    {
        aiPath.canMove = true;
        aiPath.canSearch = true;
        targetsetting = GetComponent<TargetFlag>();
        Target = targetsetting.Target(StartPos);
        ToTarget(Target);
    }

    private void Move_Gate(Transform StartPos, ref Transform Target)
    {
        aiPath.canMove = true;
        aiPath.canSearch = true;
        targetsetting = GetComponent<TargetGate>();
        Target = targetsetting.Target(StartPos);
        ToTarget(Target);
    }

    private void ToTarget(Transform Target)
    {
        Main_target.target = Target;
    }

    private AIState Change_State()
    {
        if (Main_target.target == null)
        {
            state = AIState.NonTarget;
        }
        else
        {
            if (Main_target.target.parent != null && Main_target.target.parent.gameObject.CompareTag("Gate"))
            {
                state = AIState.Target_Gate;
            }
            else if (Main_target.target.CompareTag("Base"))
            {
                state = AIState.Target_Base;
            }
            else if (Main_target.target.CompareTag("Flag"))
            {
                state = AIState.Target_Flag;
            }
            else if (Main_target.target.CompareTag("Leader") && Main_target.target.CompareTag("Soldier"))
            {

            }
        }
        return state;
    }

    public bool isArrive(Transform EndPos)
    {
        return (Vector3.Magnitude(transform.position - EndPos.position) < 5) ? true : false;
    }

    private void Scan_targetPos()
    {
        int layerMask = ~LayerMask.GetMask("IgnoreCollision");

        RaycastHit[] allHits = Physics.SphereCastAll(transform.position, scanRange, Vector3.forward, 0, layerMask);

        List<GameObject> validObject = new List<GameObject>();

        NearGate = false;
        NearFlag = false;
        NearBase = false;

        for(int i = 0; i < allHits.Length; i++)
        {
            GameObject hitObject = allHits[i].transform.gameObject;

            if (hitObject.CompareTag("SpawnPoint"))
            {
                // "SpawnPoint" 태그인 경우에는 처리하지 않습니다.
                continue;
            }

            validObject.Add(hitObject);
        }

        foreach (GameObject hit in validObject)
        {
            if(hit.transform.CompareTag("SpawnPoint"))
            {
                continue;
            }

            if (hit.transform.CompareTag("Gate"))
            {
                NearGate = isArrive(hit.transform);
            }
            else if (hit.transform.CompareTag("Flag"))
            {
                NearFlag = isArrive(hit.transform);
            }
            else if (hit.transform.CompareTag("Base"))
            {
                NearBase = isArrive(hit.transform);
            }

        }
    }
       
}
