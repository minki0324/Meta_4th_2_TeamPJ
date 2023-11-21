using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
using Pathfinding.Util;
[System.Serializable]
public struct Data
{
    public float currentHP;
    public float maxHP;
    public float damage;
    public bool isDie;
}
public abstract class Unit : MonoBehaviour 
{
    public Data data;
    protected WaitForSeconds attackDelay;
    protected WaitForSeconds hitDelay = new WaitForSeconds(0.2f);
    protected Ply_Controller player;
    public AIDestinationSetter target;
    public AIPath aipath;
    //팀의 리더가 누군지

    //공격중인가?
    protected bool isHitting;
    protected bool isAttacking;
    protected bool isdetecting;
    protected bool holdingShield;
    protected bool isMove;
    public Animator ani;
    protected Coroutine attackCoroutine;
    public LeaderState leaderState;
    public GameObject leader;
    protected Soilder_Controller enemy;
   [SerializeField] protected float scanRange = 30;
    [SerializeField] protected float AttackRange = 1.5f;
    protected float formationRange = 8;
    protected int myLayer;
    protected int combinedMask;
    // 공격 대상 레이어
    protected LayerMask TeamLayer;
    protected bool isSuccessAtk = true;
    //죽었을때 박스콜라이더 Enable하기위해 직접참조 
    [SerializeField] protected Collider HitBox_col;
    [SerializeField] protected Collider Ob_Weapon_col;
    protected float defaultSpeed;

    protected float speed;

    [Header("현재타겟 Transform")]
    [SerializeField] protected Transform nearestTarget;


    //힐러용
    Healer healer;

    [SerializeField]
    Vector3 HealerPos = new Vector3();

    float healRange = 5f;

    public Transform GetNearestTarget()
    {
        return nearestTarget;
       
    }


    protected virtual void Awake()
    {
        ani = GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponentInChildren<Ply_Controller>();
        aipath = GetComponent<AIPath>();
        defaultSpeed = aipath.maxSpeed;
    }

    protected virtual void Start()
    {
        //자신의 레이어를 제외한 적팀레이어를 담은 배열 계산하는 메소드

        myLayer = gameObject.layer;
        TeamLayer = LayerMask.NameToLayer("Team");
        combinedMask = TargetLayers();
        leaderState = GetComponent<LeaderState>();
        target = GetComponent<AIDestinationSetter>();

        
        //



    }
    protected void AttackOrder()
    {
        RaycastHit[] allHits = Physics.SphereCastAll(transform.position, scanRange, Vector3.forward, 0, combinedMask);
        nearestTarget = GetNearestTarget(allHits);

        if (nearestTarget != null) //탐지된 적이 있을때
        {
            LookatTarget(nearestTarget);
            target.target = nearestTarget;

            if (gameObject.GetComponent<Soilder_Controller>().isHealer)    //힐러일 때
            {
                if (Vector3.Distance(transform.position, nearestTarget.transform.position) < 5f)
                {
                    target.target = transform;
                }
            }

            if (gameObject.GetComponent<Soilder_Controller>().isHealer)    //힐러일 때
            {
                float healDistance = Vector3.Distance(transform.position, nearestTarget.position);
                if (healDistance <= healRange)
                {
                    isdetecting = true;
                }
                else
                {
                    isdetecting = false;
                }
            }
            else
            {
                //힐러가 아닐 떄
                float attackDistance = Vector3.Distance(transform.position, nearestTarget.position); //목표 타겟과 자기자신 거리 비교
                if (attackDistance <= AttackRange)
                {
                    isdetecting = true;     //공격범위
                }
                else
                {
                    isdetecting = false;
                }
            }
           

            if (!isdetecting) //탐지된적이 멀리있으면 적한테 이동
            {
                isMove = true;
                //ani.SetBool("Move", true);
                aipath.isStopped = false;
                aipath.canMove = true;
                aipath.canSearch = true;

            }
            else // 탐지된 적이 접근하면 이동을 멈추고 공격
            {

                isMove = false;
                aipath.isStopped = true;
                aipath.canMove = false;
                aipath.canSearch = false;
                if (!isAttacking)
                {
                    attackCoroutine = StartCoroutine(Attack_co());
                    //StartCoroutine(Attack_co());
                }

                //������ ��
            }

        }
        else//탐지된 적이 없을때,
        {
            Lostleader();



        }
    }

    
   
    //레이어 감지후 가까운 타겟 설정하는메소드
    protected Transform GetNearestTarget(RaycastHit[] hits)
    {
        Transform nearest = null;
        float closestDistance = float.MaxValue;

        foreach (RaycastHit hit in hits)
        {
            if (hit.transform.CompareTag("SpawnPoint") || (hit.transform.CompareTag("Flag")))
            {
                continue;
            }
            float distance = Vector3.Distance(transform.position, hit.transform.position);


            if (distance < closestDistance && !hit.transform.CompareTag("SpawnPoint"))
            {
                closestDistance = distance;
                nearest = hit.transform;
            }
        }

        return nearest;
    }
    //적을감지했을때 적을바라보는 메소드
    protected void LookatTarget(Transform target)
    {

        Vector3 AttackDir = target.position - transform.position;
        AttackDir.y = 0; // Y 축 이동을 무시하여 기울이지 않음
        Quaternion rotation = Quaternion.LookRotation(AttackDir);
        transform.rotation = rotation;
    }
    //적을감지했을때 공격하기위해 적에게 이동하는메소드


    //공격코루틴메소드

    //히트 코루틴메소드
    protected GameObject FindParentGameObject(GameObject child)
    {
        Transform parentTransform = child.transform.parent;

        // 부모가 더 이상 없으면 현재 child 반환
        if (parentTransform != null)
        {
            return FindParentGameObject(parentTransform.gameObject);
        }
        else
        {
            // 부모가 더 이상 없으면 현재 child 반환
            return child;
        }
    }
    protected Soilder_Controller FindParentComponent(GameObject child)
    {
        Transform parentTransform = child.transform.parent;

        // 부모가 더 이상 없으면 null 반환
        if (parentTransform == null)
        {
            return null;
        }

        // 부모 객체에서 원하는 컴포넌트 가져오기
        Soilder_Controller parentComponent = parentTransform.GetComponent<Soilder_Controller>();

        // 부모 객체에 해당 컴포넌트가 있으면 반환, 없으면 부모의 부모로 재귀 호출
        return parentComponent != null ? parentComponent : FindParentComponent(parentTransform.gameObject);
    }

    //이벤트에서 무기 껏다키는 메소드
    public void WeaponActive()
    {
        isSuccessAtk = true;
        Ob_Weapon_col.enabled = true;
        Invoke("WeaponFalse", 0.1f);

    }
    private void WeaponFalse()
    {
        Ob_Weapon_col.enabled = false;
    }
    //죽을때 메소드



    protected int TargetLayers()
    {
        int[] combinedLayerMask;
        int myLayer = gameObject.layer;
        //총 4개팀의 레이어 
        int[] layerArray = new int[] { LayerMask.NameToLayer("Team"), LayerMask.NameToLayer("Enemy1"), LayerMask.NameToLayer("Enemy2"), LayerMask.NameToLayer("Enemy3") };
        //우리팀의 레이어를 제외한 나머지 레이어를 담을 배열
        combinedLayerMask = new int[3];
        int combinedIndex = 0;


        for (int i = 0; i < layerArray.Length; i++)
        {
            if (myLayer != layerArray[i])
            {
                combinedLayerMask[combinedIndex] = layerArray[i];
                combinedIndex++;
            }

        }
        int layerMask0 = 1 << combinedLayerMask[0];
        int layerMask1 = 1 << combinedLayerMask[1];
        int layerMask2 = 1 << combinedLayerMask[2];
        combinedMask = layerMask0 | layerMask1 | layerMask2;
        return combinedMask;
    }
    //자신의 리더가 오더를내렸을때 말을듣게하기위한메소드

    //히트 코루틴메소드
    protected IEnumerator Hit_co(float damage)
    {
        isHitting = true;
        //히트시 대미지달기

        //데미지 다는거 각 스크립트마다 구현해주세요 todo
        HitDamage(damage);


        //공격도중 캔슬시 공격쿨타임 초기화
        if (!isSuccessAtk)
        {


            StopCoroutine(attackCoroutine);
            isAttacking = false;
        }
        ani.SetTrigger("Hit");
        yield return hitDelay;
        isHitting = false;


    }
    protected IEnumerator Attack_co()
   
    {
        if(!gameObject.GetComponent<Soilder_Controller>().isHealer) //힐러가 아니면 공격
        {   
            //공격쿨타임
            float d = Random.Range(2f, 2.1f);
            attackDelay = new WaitForSeconds(d);

            //상태 공격중으로 변경
            isAttacking = true;

            isSuccessAtk = false;
            ani.SetTrigger("Attack");
            yield return attackDelay;


            isAttacking = false;
        }
        else
        {
            healer = GetComponent<Healer>();
            healer.GetHeal_Target();
        }
     
    }
    //protected void FollowOrder()
    //{
    //    ani.SetBool("Move", true);
    //    target.target = leader.transform;
    //}
    
    public abstract void Lostleader();
    public abstract void Die();
    public abstract void HitDamage(float damage);
    
}
