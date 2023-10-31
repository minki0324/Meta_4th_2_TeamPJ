using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class UnitAttack : Minion_Controller
{
    //[SerializeField] private Ply_Controller player;
    /*
     미니언을 중심으로한 레이어를 감지하는 원 생성
    레이어중 가장가까운적을 타겟으로 지정함
    타겟감지후 미니언이 적을 바라보고 (Lookat메소드) 적에게 이동  -> 현재는 Lerp로 이동 추후 네비게이션으로 변경
    이동중 미니언의 공격범위 콜라이더(미니언앞에 작은 박스콜라이더 (좀비서바이벌처럼))에 닿으면 정지후 공격

     
     
     
     */
    //임시 미니언체력
    private int HP = 10;
    private bool isDie;



    // 유닛 공격감지범위
    public float scanRange = 13f;
    //감지후 유닛 이동속도 -> 플레이어 이동속도랑 맞추기
    private float moveSpeed = 1f;
    //이동중 적군유닛이 공격범위콜라이더에 닿았는가?
    private bool isdetecting = false;
    //공격중인가?
    private bool isAttacking = false;
    private bool isHitting = false;
    private bool isSuccessAtk = true;
    private Animator ani;
    private Coroutine attackCoroutine;
    // 공격 대상 레이어
    [SerializeField] private LayerMask targetLayer;
    [SerializeField] private GameObject weapon;
    [SerializeField] private BoxCollider boxCollider;
    [SerializeField] private BoxCollider boxCollider1;
    private BoxCollider weapon_col;
    private int layerIndex;
    public Transform nearestTarget;

    private WaitForSeconds attackDelay;
    private WaitForSeconds hitDelay = new WaitForSeconds(0.5f);

    private NavMeshAgent navMeshAgent;
    private void Start()
    {
        //공격딜레이 랜덤


        navMeshAgent = GetComponent<NavMeshAgent>();
        ani = GetComponent<Animator>();
        weapon_col = weapon.GetComponent<BoxCollider>();



        //GameObject.FindGameObjectWithTag("Player").TryGetComponent<Ply_Controller>(out player);
        if (gameObject.layer == LayerMask.NameToLayer("Team"))
        {
            layerIndex = LayerMask.NameToLayer("Enemy");
        }
        else if (gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            layerIndex = LayerMask.NameToLayer("Team");
        }
        targetLayer = 1 << layerIndex;

    }

    private void Update()
    {

        //if(player.CurrentMode == Ply_Controller.Mode.Attack) { 
        RaycastHit[] hits = Physics.SphereCastAll(transform.position, scanRange, Vector3.forward, 0, targetLayer);
        nearestTarget = GetNearestTarget(hits);



        if (nearestTarget != null && !isDie)
        {
            //타겟감지시 타겟쪽으로 바라보기
            LookatTarget(nearestTarget);
            // 유닛의 공격범위에 들어갈때까지 타겟에게 이동
            Debug.Log(isdetecting);
            if (!isdetecting && !isAttacking)
            {
                AttackMoving(nearestTarget);
            }
            // 타겟이 공격범위에 들어왔을때 공격
            else
            {
                navMeshAgent.isStopped = true;
                ani.SetBool("Move", false);

                // 
                if (!isAttacking)
                {
                    attackCoroutine = StartCoroutine(Attack_co());
                    //StartCoroutine(Attack_co());
                }

            }
        }
        //}

        if (HP <= 0)
        {
            //공격정지 ,이동정지 
            if (!isDie)
            {
                ani.SetTrigger("Dead");
                gameObject.layer = 9;
                boxCollider.enabled = false;
                boxCollider1.enabled = false;
                Destroy(gameObject, 3f);
            }
            isDie = true;
        }
    }
    //레이어 감지후
    Transform GetNearestTarget(RaycastHit[] hits)
    {
        Transform nearest = null;
        float closestDistance = float.MaxValue;

        foreach (RaycastHit hit in hits)
        {
            float distance = Vector3.Distance(transform.position, hit.transform.position);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                nearest = hit.transform;
            }
        }

        return nearest;
    }
    private void LookatTarget(Transform target)
    {

        Vector3 AttackDir = target.position - transform.position;
        transform.rotation = Quaternion.LookRotation(AttackDir);
    }
    void AttackMoving(Transform target)
    {
        //추후 미니언컨트롤러에서 제어할예정.(애니메이션)
        // 공격 로직을 구현
        //Debug.Log("공격타겟 : " + target.name);
        ani.SetBool("Move", true);
        navMeshAgent.isStopped = false;
        //추후 네비게이션으로 이동변경 
        navMeshAgent.SetDestination(target.transform.position);
        //transform.position = Vector3.Lerp(transform.position, target.position, moveSpeed * Time.deltaTime);



    }
    //감지범위 그리는메소드
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, scanRange);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (gameObject.layer == LayerMask.NameToLayer("Team"))
        {
            if (other.CompareTag("Weapon") && other.gameObject.layer == LayerMask.NameToLayer("Enemy") && !isHitting)
            {
                StartCoroutine(Hit_co());
            }

        }
        if (gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            if (other.CompareTag("Weapon") && other.gameObject.layer == LayerMask.NameToLayer("Team") && !isHitting)
            {
                StartCoroutine(Hit_co());
            }
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if (gameObject.layer == LayerMask.NameToLayer("Team"))
        {
            if (other.gameObject.layer == LayerMask.NameToLayer("Enemy"))
            {

                isdetecting = true;
                //적이 감지콜라이더에 닿았을때 이동을 멈추고 공격 애니메이션 진행
            }
            else
            {
                isdetecting = false;
            }

        }
        else if (gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            if (other.gameObject.layer == LayerMask.NameToLayer("Team"))
            {

                isdetecting = true;
                //적이 감지콜라이더에 닿았을때 이동을 멈추고 공격 애니메이션 진행
            }
            else
            {
                isdetecting = false;
            }

        }


    }
    //private void OnTriggerExit(Collider other)
    //{
    //    if (gameObject.layer == LayerMask.NameToLayer("Team"))
    //    {
    //        if (other.gameObject.layer == LayerMask.NameToLayer("Enemy"))
    //        {
    //            Debug.Log("1111");
    //            isdetecting = false;
    //            //적이 감지콜라이더에 닿았을때 이동을 멈추고 공격 애니메이션 진행
    //        }

    //    }
    //    else if (gameObject.layer == LayerMask.NameToLayer("Enemy"))
    //    {
    //        if (other.gameObject.layer == LayerMask.NameToLayer("Team"))
    //        {
    //            Debug.Log("2222");
    //            isdetecting = false;
    //            //적이 감지콜라이더에 닿았을때 이동을 멈추고 공격 애니메이션 진행
    //        }
    //    }
    //}
    private IEnumerator Attack_co()
    {
        float d = Random.Range(2f, 2.1f);
        attackDelay = new WaitForSeconds(d);
        //공격시간동안 이동불가
        //
        Debug.Log("몇번들어오냐");
        isAttacking = true;
        isSuccessAtk = false;
        ani.SetTrigger("Attack");
        yield return attackDelay;

        Debug.Log("여긴들어오냐");
        isAttacking = false;
    }

    private IEnumerator Hit_co()
    {
        isHitting = true;
        //히트시 대미지달기
        HP -= 1;


        //공격도중 캔슬시 공격쿨타임 초기화
        if (!isSuccessAtk)
        {

            Debug.Log("공격도중 맞았음 쿨초기화");
            StopCoroutine(attackCoroutine);
            isAttacking = false;
        }

        ani.SetTrigger("Hit");
        yield return hitDelay;
        isHitting = false;


    }

    public void WeaponActive()
    {
        isSuccessAtk = true;
        weapon_col.enabled = true;
        Invoke("WeaponFalse", 0.1f);

    }
    private void WeaponFalse()
    {
        weapon_col.enabled = false;
    }
}