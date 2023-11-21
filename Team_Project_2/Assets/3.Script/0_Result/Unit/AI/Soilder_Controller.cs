using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System.Linq;

public class Soilder_Controller : Unit
{

    //Detect 상태일때만 적용되는 스테이트
    public enum FormationState
    {
        Following, // 리더와 가까워질때까지 따라다님
        Formation, // 리더한테 도착하면 포메이션 세팅
        GoingFormation, //포메이션으로이동
        Shield, //포메이션 이동완료시 실드들고 리더와 발맞추기

    }

    //적컴포넌트
    private GameObject ob;

    public FormationState formationState;
    public Transform formationTransmform;
    //네비게이션
    public Unit_Information infodata;
    public bool isArrive = false;
    public bool isSetPosition = false;
    public bool isNear = false;

    Healer healer;



    float tempspeed;
    public float currentScore;

    protected override void Awake()
    {
        base.Awake();

    }

    protected override void Start()
    {

        base.Start();
        //자신의 레이어를 제외한 적팀레이어를 담은 배열 계산하는 메소드
        myLayer = gameObject.layer;
        TeamLayer = LayerMask.NameToLayer("Team");
        combinedMask = TargetLayers();

        tempspeed = speed;


        //
        if (myLayer != TeamLayer)
        {

            leaderState = FindLeader();
            if (leaderState != null)
            {
                leader = leaderState.gameObject;

            }
        }
        else
        {
            leader = player.gameObject;

        }

    }

    private void OnEnable()
    {
        if (!GameManager.instance.isLive)
        {
            return;
        }
    }

    private void Update()
    {
        if (!GameManager.instance.isLive || data.isDie)
        {
            return;
        }

        if (Vector3.Distance(leader.transform.position, transform.position) < formationRange)
        {
            isArrive = true;
        }
        else
        {
            isArrive = false;
        }

        if (!isArrive) //도착하지않음  -> 팔로잉
        {
            formationState = FormationState.Following;
        }
        else if (isArrive && !isSetPosition) //도착함, 포지션배정 안받음 -> 포메이션대기
        {
           
            formationState = FormationState.Formation;   // 도착시 포메이션 준비완료
        }
        else if (isArrive && isSetPosition) // 도착했고 포지션배정받음
        {
            if (formationState != FormationState.Shield)
            {
                formationState = FormationState.GoingFormation;


            }
        }






        // 방패 들때안들때 이속,방패들기 메소드
        if (holdingShield)
        {
            speed -= 1f * Time.deltaTime;
            speed = Mathf.Clamp(speed, 0.3f, 1f);
        }
        else
        {
            if (isMove)
            {
                speed += 1f * Time.deltaTime;

            }
            else
            {
                speed -= 1f * Time.deltaTime;

            }
            speed = Mathf.Clamp01(speed);
        }
        aipath.maxSpeed = 5 * speed;


        #region  플레이어,AI 명령로직
        //병사의 리더가 적리더일때
        if (leader != player.gameObject)
        {
            //적리더가 죽지않았을때
            if (!leaderState.data.isDie)
            {
                switch (leaderState.bat_State)
                {
                    case LeaderState.BattleState.Detect:
                    case LeaderState.BattleState.Wait:
                        Formation_Process();
                        break;
                    case LeaderState.BattleState.Attack:
                        holdingShield = false;
                        //aipath.canSearch = true;
                        //aipath.canMove = true;
                        //aipath.maxSpeed = Mathf.Lerp(aipath.maxSpeed, defaultSpeed, Time.deltaTime * 1);


                        if (!data.ishealer)
                        {
                            //느려졌던 이동속도 초기화
                            //Debug.Log("attack~~~" + gameObject.name.ToString());
                            AttackOrder();
                        }
                        //else
                        //{
                        //    //힐러 메소드 넣기                         
                        //    healer = GetComponent<Healer>();
                        //    healer.GetHeal_Target();
                        //}



                        break;
                    case LeaderState.BattleState.Move:
                        FollowOrder();
                        holdingShield = false;
                        aipath.isStopped = false;
                        break;


                    default:
                        //holdingShield = false;
                        //isMove = true;

                        //FollowOrder();
                        break;

        
                }
            }
            else
            {

                AttackOrder();


            }
        }
        else
        {
            if (!GameManager.instance.isDead)
            {

                switch (player.CurrentMode)
                {
                    case Ply_Controller.Mode.Follow:
                        Formation_Process();
                        if (player.playerMovement.isPlayerMove && player.playerMovement.speed >0.7f && !    player.playerMovement.holdingShield)
                        {
                            isSetPosition = false; //배치받은 위치 초기화
                            FollowOrder();
                        }

                        break;
                    case Ply_Controller.Mode.Attack:
                        AttackOrder();
                        break;
                    case Ply_Controller.Mode.Stop:
                        break;

                }
            }
            else
            {

                AttackOrder();

            }
        }
        ani.SetBool("Shield", holdingShield);
        ani.SetFloat("Speed", speed);
        ani.SetBool("Move", isMove);
        #endregion
        #region 다이 메소드
        if (data.currentHP <= 0)
        {
            //공격정지 ,이동정지 
            if (!data.isDie)
            {
                data.isDie = true;
                Die();
            }

        }
        #endregion
    }
    //레이어 감지후 가까운 타겟 설정하는메소드

    //적을감지했을때 적을바라보는 메소드
    //적을감지했을때 공격하기위해 적에게 이동하는메소드

    //감지범위 그리는메소드
    //private void OnDrawGizmos()
    //{
    //    Gizmos.color = Color.red;
    //    Gizmos.DrawWireSphere(transform.position, scanRange);

    //    Gizmos.color = Color.green;
    //    Gizmos.DrawWireSphere(transform.position, AttackRange);
    //}


    private void OnTriggerEnter(Collider other)
    {


        if (other.CompareTag("Weapon") && !isHitting)
        {
            ob = FindParentGameObject(other.gameObject);   //칼을들고있는 오브젝트

            if (gameObject.layer != ob.layer) //공격하는 오브젝트가 적일때 
            {

                enemy = FindParentComponent(other.gameObject);


                //병사들
                if (enemy != null)
                {
                    if (enemy.gameObject.layer != gameObject.layer)
                    {
                        StartCoroutine(Hit_co(enemy.data.damage));


                    }
                }
                else//null : 리더들 , 플레이어 
                {
                    LeaderAI _leader = other.GetComponent<LeaderAI>();

                    if (_leader != null && _leader.gameObject.layer != gameObject.layer)
                    {
                        StartCoroutine(Hit_co(_leader.data.damage));
                    }
                    else if (gameObject.layer != player.gameObject.layer)
                    {
                        StartCoroutine(Hit_co(GameManager.instance.Damage));
                    }
                }



                if (data.currentHP <= 0)
                {
                    //적이 나를 죽였을때 -> 플레이어 컨트롤 다이에서 따로 설정
                    //enemy ==null  -> 플레이어

                    //우리팀을 적팀이 죽였을때 
                    if (leader == player.gameObject)
                    {
                        GameManager.instance.DeathCount++;
                        enemy.leaderState.killCount++;
                    }
                    else
                    {
                        //병사끼리의 킬계산
                        if (enemy != null)
                        {
                            //적팀을 우리팀이 죽였을때
                            if (enemy.leader == player.gameObject)
                            {
                                GameManager.instance.killCount++;
                                leaderState.deathCount++;
                            }
                            //적팀끼리 죽였을때
                            else
                            {
                                enemy.leaderState.killCount++;
                                leaderState.deathCount++;
                            }
                        }
                        else//리더가죽였을때 킬계산
                        {  //적팀을 내가 죽였을때 (enemy ==null)
                            if (ob.gameObject == player.gameObject)
                            {
                                GameManager.instance.killCount++;
                                leaderState.deathCount++;
                            }
                            else
                            {
                                LeaderAI _leader = ob.GetComponent<LeaderAI>();
                                _leader.killCount++;
                                leaderState.deathCount++;
                            }

                        }

                    }
                }
            }
        }


    }


    //공격코루틴메소드


    //이벤트에서 무기 껏다키는 메소드

    //죽을때 메소드
    public override void Die()
    {

        //ani.SetBool("Die", true);  // 죽는모션재생
        ani.SetLayerWeight(1, 0);
        ani.SetTrigger("Dead"); // 죽는모션재생
                                //HitBox_col.enabled = false;    //부딪히지않게 콜라이더 false
        isMove = false;
        aipath.isStopped = true;
        aipath.canMove = false;

        aipath.canSearch = false;   
        

        if (gameObject.layer == TeamLayer)
        {
            player.UnitList_List.Remove(gameObject);
            GameManager.instance.Current_MinionCount--;
            //following.Stop_List.Remove(gameObject);
        }
        else
        {
            leaderState.UnitList.Remove(gameObject);
            leaderState.currentUnitCount--;
        }
        gameObject.layer = 12;   // 레이어 DIe로 변경해서 타겟으로 안되게
        //StopCoroutine(attackCoroutine);   //공격도중이라면 공격도 중지

        Destroy(gameObject, 3f);  // 죽고나서 3초후 디스트로이


    }



    //자신의 레이어와 같은 리더를 찾는 메소드
    private LeaderState FindLeader()
    {
        GameObject[] objectsWithSameLayer = GameObject.FindGameObjectsWithTag("Leader"); // YourTag에는 LeaderState 컴포넌트가 있는 오브젝트의 태그를 넣습니다.

        // 찾은 오브젝트 중에서 LeaderState 컴포넌트를 가진 첫 번째 오브젝트를 찾습니다.


        foreach (var obj in objectsWithSameLayer)
        {
            if (obj.gameObject.layer == gameObject.layer)
            {
                leaderState = obj.GetComponent<LeaderState>();

                if (leaderState != null)
                {
                    return leaderState;
                    // LeaderState를 찾으면 루프를 종료합니다.
                }
            }
        }

        if (leaderState == null)
        {
            Debug.LogWarning("LeaderState 컴포넌트를 가진 오브젝트를 찾을 수 없습니다.");
        }
        return null;
    }
    //자신의 레이어에따라 공격할 레이어들을 구분시켜주는 메소드
    //예> 자신이 Enemy1 이라면 Team,Enemy2, Enemy3 는 적으로 구분

    //자신의 리더가 오더를내렸을때 말을듣게하기위한메소드





    public void Setunit()
    {

        data.maxHP = infodata.maxHP;
        data.currentHP = data.maxHP;
        data.damage = infodata.damage;
        data.ishealer = infodata.ishealer;
        data.attackRange = infodata.attackRange;

    }
    public override void HitDamage(float damage)
    {
        data.currentHP -= damage;
    }
    public override void Lostleader()
    {
        /*
             1. 리더가 Attack 명령을 내렸지만 너무멀어서 공격할 적이 없을경우
             2. 리더가 없을경우 
             */
        if (leader == null) // 리더가 없으면 제자리에서 대기
        {
            isMove = false;
            aipath.isStopped = true;
            return;
        }
        else // 리더가 있으면 리더한테 이동
        {
            FollowOrder();
           
        }
    }
    private void FollowOrder()
    {
        isMove = true;
        aipath.canMove = true;
        aipath.canSearch = true;
        target.target = leader.transform;
        holdingShield = false;
    }
    private void Formation_Process()
    {
        switch (formationState)
        {
            case FormationState.Following:
                FollowOrder();
                break;
            case FormationState.Formation:
                break;
            case FormationState.GoingFormation:
                ani.SetBool("Shield" , true);
                target.target = formationTransmform;
                break;
            case FormationState.Shield:
                holdingShield = true;
                aipath.canMove = false;
                if (leader.layer == player.gameObject.layer)
                {
                    if (player.playerMovement.isPlayerMove) { 
                    speed = player.playerMovement.speed;
                    }
                    else
                    {
                        speed = 0;
                    }


                    //코드개더럽네 시발 되면 고침
                    Vector3 cameraForward = Camera.main.transform.forward;
                    cameraForward.y = 0f; // 회전을 수평 평면에만 유지
                    Quaternion targetRotation = Quaternion.LookRotation(cameraForward);
                    Quaternion currentRotation = transform.rotation;
                    transform.position += (player.playerMovement.MoveDir.normalized * player.playerMovement.realSpeed); //플레이어와 이동 동기화
                     transform.rotation =  player.transform.rotation; //플레이어와 로테이션 동기화

                    if (player.playerMovement.holdingShield)
                    {
                        
                        float rotationSpeed = 3f; // 회전 속도 조절
                        transform.rotation = Quaternion.Slerp(currentRotation, targetRotation, rotationSpeed * Time.deltaTime);
                    }
                }

            
                //if (leader.layer == player.gameObject.layer)
                //{
                //    transform.position = transform.position + player.playerMovement.MoveDir * player.playerMovement.speed * Time.deltaTime;
                //}
                //else
                //{

                //    transform.position = transform.position + playerDirection * leaderState.speed * Time.deltaTime;
                //}
                //Vector3 leaderDirection = leader.transform.forward * -1;
                //float leaderSpeed = 1.5f;

                // 예시: 솔져에게 리더의 방향과 속도를 전달
                //transform.position = transform.position + leaderDirection * leaderSpeed * Time.deltaTime;
                break;
        }
       
    }


}