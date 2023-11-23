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
    private AIpathoverride aiPath1;
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
        aiPath1 = GetComponent<AIpathoverride>();
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

    // ��Ÿ���϶� Ÿ���� �������ִ� �޼ҵ�
    public void Search_Target()
    {
        // ai Ÿ���� ���� ��
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
                    1. ���� ������ ���� ���� üũ�ؼ� ���� ���ڰ� ������ ����� ��������Ʈ�� �̵��ؼ� ���� ä��
                    2. ���� ���ڰ� 15�� �̻��̶�� ���� ����� ��߷� �̵��ؼ� ���� �غ�
                    3. ��߿� �����ߴٸ� Wait ���·� ��ȯ�ؼ� ������ ���������� ���
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
                        Debug.Log("����?");
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
    // Ÿ���� ���̽��϶� ���� �ൿ�� �������ִ� �޼ҵ�
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
                    1. ���� ������ ���� ���� üũ�Ͽ� ���� ���ڰ� 15���� ���϶�� ���� �ֺ��� ���� ����Ʈ�� �̵�
                    2. isStart�� false�̸� ���� �������� �������°� �ƴ϶� isStart�� ���ָ鼭 Gate�� �̵�
                    3. ���� ���� ���ڰ� 15���� �̻��̶�� �״�� �������� �̵�
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
                    1. ���� ������ ���� ���� üũ�Ͽ� ���� ���ڰ� 15���� ���϶�� ���
                    2. ���� ���ڰ� ����� ä�������� ����Ʈ�� �̵�
                */
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
    // Ÿ���� ����Ʈ�϶� ���� �ൿ�� �������ִ� �޼ҵ�
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
                    1. ���� ������� 15���� ���϶�� ����� ���̽��� �̵�
                    2. ���� �������� �÷����ε� �ϴ� ����Ʈ �ֺ��� �;� �� ������ �����ϱ� ������ neargate �Ұ��� ���� �������� ����
                    3. neargate���� true�� move_flag �޼ҵ� ȣ��
                    4. neargate���� false�� break�� ������ �ö����� ���
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
                    1. ����Ʈ�� ��������� wait�� �ʿ���⶧���� ���� 
                */
                ai.bat_State = LeaderState.BattleState.Move;
                break;
        }
    }
    // Ÿ���� �÷����϶� ���� �ൿ�� �������ִ� �޼ҵ�
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
                    1. ���� ����� üũ�ؼ� 15���� ���϶�� ����� ���� ����Ʈ�� �̵�
                    2. nearFlag �Ұ����� �÷��׿� ����������� �ƴ��� Ȯ��
                    3. nearFlag Ʈ��� �������̹Ƿ� Wait���·� ��ȯ
                    4. nearFlag �޽��� �̵����̹Ƿ� break;
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
                            ai.bat_State = LeaderState.BattleState.Wait;
                        }
                        else if(isOccuDone())
                        {
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
                    1. ���� ����� üũ�ؼ� 15���� ���϶�� ����� ���� ����Ʈ�� �̵�
                    2. nearFlag�� �޽���� ��Ʋ ������Ʈ Move�� �ٲ���
                    3. ������ �Ϸ��ߴٸ� ��Ʋ ������Ʈ�� move�� �ٲ���
                    4. ���� �������̶�� break;
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
        aiPath1.canMove = true;
        aiPath.canSearch = true;  
        aiPath1.canSearch = true;  
        targetsetting = GetComponent<TargetMyBase>();
        Target = targetsetting.Target(StartPos);
        ToTarget(Target);
    }

    private void Move_Flag(Transform StartPos, ref Transform Target)
    {
        aiPath.canMove = true;
        aiPath1.canMove = true;
        aiPath.canSearch = true;  
        aiPath1.canSearch = true;  
        targetsetting = GetComponent<TargetFlag>();
        Target = targetsetting.Target(StartPos);
        ToTarget(Target);
    }

    private void Move_Gate(Transform StartPos, ref Transform Target)
    {
        aiPath.canMove = true;
        aiPath1.canMove = true;
        aiPath.canSearch = true; 
        aiPath1.canSearch = true; 
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
                // "SpawnPoint" �±��� ��쿡�� ó������ �ʽ��ϴ�.
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
