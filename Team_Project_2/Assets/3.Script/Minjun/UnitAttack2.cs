using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System.Linq;

public class UnitAttack2 : Unit
{
    //[SerializeField] private Ply_Controller player;
    /*
     미니언을 중심으로한 레이어를 감지하는 원 생성
    레이어중 가장가까운적을 타겟으로 지정함
    타겟감지후 미니언이 적을 바라보고 (Lookat메소드) 적에게 이동  -> 현재는 Lerp로 이동 추후 네비게이션으로 변경
    이동중 미니언의 공격범위 콜라이더(미니언앞에 작은 박스콜라이더 (좀비서바이벌처럼))에 닿으면 정지후 공격

     
     
     
     */
    //임시 미니언체력
    //public float currentHP;
    //public float maxHP;
    //public float Damage;
    //팀의 리더가 누군지
    
    //적컴포넌트
    private GameObject ob;
    public GameObject GetLeader()
    {
        return leader;
    }
    // 유닛 공격감지범위


    //이동중 적군유닛이 공격범위콜라이더에 닿았는가?
    //공격중인가?

    // 공격 대상 레이어
    private LayerMask EnemyLayer;
    //�׾����� �ڽ��ݶ��̴� Enable�ϱ����� �������� 
    
    //어택, 히트 딜레이
  
    //네비게이션
    public bool isClose;
    public Unit_Information infodata;
    public bool isHealer = false;


    public bool isNear = false;

    private Following following;
    protected override void Awake()
    {
        base.Awake();
        
        ani = GetComponent<Animator>();
        
    }

    protected override void Start()
    {

        base.Start();
        //자신의 레이어를 제외한 적팀레이어를 담은 배열 계산하는 메소드
        myLayer = gameObject.layer;
        TeamLayer = LayerMask.NameToLayer("Team");
        combinedMask = TargetLayers();


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
        if(!GameManager.instance.isLive)
        {
            return;
        }
    }

    private void Update()
    {
        if(!GameManager.instance.isLive || data.isDie )
        {
            return;
        }
        
        //병사의 리더가 적리더일때
        if ( leader != player.gameObject)
        {
            //적리더가 죽지않았을때
            if (leaderState.data.isDie)
            {
                switch (leaderState.bat_State)
                {
                    case LeaderState.BattleState.Attack:
                        if (gameObject != leader)
                        {
                            if (!infodata.ishealer)
                            {
                                AttackOrder();
                            }
                            else
                            {
                                //힐러 메소드 넣기
                            }
                        }
                        else
                        {
                            AttackOrder();
                        }
                      
                        break;
                    default:
                        FollowOrder();
                        break;
                }
            }
            else
            {

                AttackOrder();


            }
        }
      

        if(!GameManager.instance.isDead && leader == player.gameObject)
        {
            switch (player.CurrentMode)
            {
                case Ply_Controller.Mode.Follow:
                    
                    if (player.CurrentMode == Ply_Controller.Mode.Follow)
                    {
                        if (isClose == true)
                        {
                            ani.SetBool("Move", false);
                        }
                        else
                        {
                            ani.SetBool("Move", true);
                        }
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


     
        if (data.currentHP <= 0 )
        {
            Debug.Log("죽는조건");
            //공격정지 ,이동정지 
            if (!data.isDie)
            {
                data.isDie = true;
                Die();
            }
            
        }
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
                            if (enemy.leader == player.gameObject )
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
        aipath.canMove = false;
        aipath.canSearch = false;
        gameObject.layer = 12;   // 레이어 DIe로 변경해서 타겟으로 안되게
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
        isHealer = infodata.ishealer;
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
            ani.SetBool("Move", false);
            return;
        }
        else // 리더가 있으면 리더한테 이동
        {
            FollowOrder();
        }
    }
    private void FollowOrder()
    {
        ani.SetBool("Move", true);
        target.target = leader.transform;
    }
}