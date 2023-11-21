using Pathfinding;
using UnityEngine;


public class LeaderController : MonoBehaviour
{
    // 하 살려주세요 제발

    public Targetsetting Targetset;

    public Transform Target;
    public Transform NextTarget;
    private LeaderAI AI;
    private ObjPosInfo MapData;

    private bool isStart = false;

    public Transform CurrentPos;

    public AIDestinationSetter Target_;



    // 하.. 하..

    private void Start()
    {
        MapData = FindObjectOfType<ObjPosInfo>();
        Target_ = GetComponent<AIDestinationSetter>();
        AI = GetComponent<LeaderAI>();
    }

    private void Update()
    {

        if (!GameManager.instance.isLive) return;

        if (isStart &&                          // isStart가 true 일 때
            AI.GetNearestTarget() == null &&    // 타겟이 null이 아닐 때
            Target.CompareTag("Base") &&        // 타겟의 태그가 Base일 때
            isArrive(Target))                   // 본인이 타겟의 주변에 있을 때
        {
            if (Target.gameObject.layer.Equals(gameObject.layer))
            {
                // 현재 병사 수가 15명 이상일 때
                if (AI.currentUnitCount >= 15)
                {
                    AI.bat_State = LeaderState.BattleState.Move;
                    Targetset = GetComponent<TargetFlag>();
                    NextTarget = Targetset.Target(transform);

                    // 현재 중앙 지역 깃발을 내가 다 먹고있을 때
                    // 그럼 돈 모일 때까지 기다리기..
                    if (NextTarget.Equals(null))
                    {

                        AI.bat_State = LeaderState.BattleState.Wait;

                        // 현재 병사 수가 15명 이상일 때
                        if (AI.currentUnitCount >= 15)
                        {
                            AI.bat_State = LeaderState.BattleState.Move;
                            Targetset = GetComponent<TargetFlag>();
                            NextTarget = Targetset.Target(transform);

                            // 현재 중앙 지역 깃발을 내가 다 먹고있을 때
                            // 그럼 돈 모일 때까지 기다리기..
                            if (NextTarget.Equals(null))
                            {
                                Targetset = GetComponent<TargetEnemyBase>();
                                NextTarget = Targetset.Target(transform);
                            }
                        }
                        else
                        {
                            if (AI.currentUnitCount >= 23)
                            {
                                Targetset = GetComponent<TargetEnemyBase>();
                                NextTarget = Targetset.Target(transform);
                            }

                        }
                        return;
                    }
                    GoGate(transform, ref Target);

                }
                else
                {
                    return;
                }
            }

            // 현재 위치가 상대 진영일 때
            else
            {
                // 깃발점령
                // 어차피 깃발 점령하면 베이스가 본인 진영으로 바뀌어서 위 if문으로 이동
                if (!Target.GetComponentInChildren<Flag>().transform.parent.gameObject.layer.Equals(gameObject.layer))
                {
                    Target = Target.gameObject.GetComponentInChildren<Flag>().transform.parent.transform;
                    ToTarget(Target);
                }
            }
            #region Flag일 때
            if (Target.CompareTag("Flag") && isArrive(Target))
            {
                // 내 깃발이 아닐 때
                // 점령하려면 가만히 있어야지..
                if (!Target.gameObject.layer.Equals(gameObject.layer))
                {
                    return;
                }
                // 내 깃발일 때
                else
                {
                    if (AI.currentUnitCount > 13)
                    {
                        GoFlag(transform, ref Target);
                    }
                    else
                    {
                        return;
                    }
                }
            }
            #endregion


            #region Gate일 때
            // 게이트에 서있을 때
            if (Target.transform.parent != null)
            {
                if (Target.transform.parent.gameObject.CompareTag("Gate") && isArrive(Target))
                {
                    if (NextTarget == null)
                    {
                        return;
                    }
                    else
                    {
                        Target = NextTarget;
                        ToTarget(Target);
                        NextTarget = null;
                    }

                }
            }
            #endregion

            if (AI.data.isDie)
            {
                GoGate(transform, ref Target);
            }

            



        }



        // --------------------------------------------------------------------------------------------
        else  // 처음 한 번만 실행
        {
            GoMyBase(transform, ref Target);
            AI.bat_State = LeaderState.BattleState.Wait;
            isStart = true;
        }


    }


    public bool isArrive(Transform EndPos)
    {
        return (Vector3.Magnitude(transform.position - EndPos.position) < 5) ? true : false;
    }

    private void GoFlag(Transform StartPos, ref Transform Target)
    {
        Targetset = GetComponent<TargetFlag>();
        Target = Targetset.Target(StartPos);
        ToTarget(Target);

    }
    private void GoMyBase(Transform StartPos, ref Transform Target)
    {     
        Targetset = GetComponent<TargetMyBase>();
        Target = Targetset.Target(StartPos);
        ToTarget(Target);

    }
    private void GoEnemyBase(Transform StartPos, ref Transform Target)
    {    
        Targetset = GetComponent<TargetEnemyBase>();
        Target = Targetset.Target(StartPos);
        ToTarget(Target);

    }
    private void GoGate(Transform StartPos, ref Transform Target)
    {
        Targetset = GetComponent<TargetGate>();
        Target = Targetset.Target(StartPos);
        ToTarget(Target);
    }
    private void ToTarget(Transform Target)
    {
        Target_.target = Target;
    }

    private void Wait()
    {

    }
}


