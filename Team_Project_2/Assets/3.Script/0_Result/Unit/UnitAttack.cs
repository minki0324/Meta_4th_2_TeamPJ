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
    private int HP = 3;
    private bool isDie;
    [SerializeField] private Ply_Controller player;


    // 유닛 공격감지범위
    public float scanRange = 13f;
    public float AttackRange = 1.5f;

    //이동중 적군유닛이 공격범위콜라이더에 닿았는가?
    [SerializeField] private bool isdetecting = false;
    //공격중인가?
    private bool isAttacking = false;
    private bool isHitting = false;
    private bool isSuccessAtk = true;
    private Animator ani;
    private Coroutine attackCoroutine;
    // 공격 대상 레이어
    [SerializeField] private LayerMask targetLayer;
    //죽었을때 박스콜라이더 Enable하기위해 직접참조 
    [SerializeField] private BoxCollider HitBox_col;
    [SerializeField] private BoxCollider Ob_Weapon_col;
    //타겟레이어
    private int targetLayer_Index;
    //최종타겟
    public Transform nearestTarget;
    //어택, 히트 딜레이
    private WaitForSeconds attackDelay;
    private WaitForSeconds hitDelay = new WaitForSeconds(0.2f);
    //네비게이션
    private NavMeshAgent navMeshAgent;
    private void Start()
    {
        //공격딜레이 랜덤
        TryGetComponent(out player);

        navMeshAgent = GetComponent<NavMeshAgent>();
        ani = GetComponent<Animator>();



        //GameObject.FindGameObjectWithTag("Player").TryGetComponent<Ply_Controller>(out player);
        if (gameObject.layer == LayerMask.NameToLayer("Team"))
        {
            targetLayer_Index = LayerMask.NameToLayer("Enemy");
        }
        else if (gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            targetLayer_Index = LayerMask.NameToLayer("Team");
        }
        targetLayer = 1 << targetLayer_Index;

    }


    private void Update()
    {

        //MinionAttack();




        if (HP <= 0)
        {
            //공격정지 ,이동정지 
            if (!isDie)
            {
                Die();
            }
            isDie = true;
        }
    }
    //레이어 감지후
    public Transform GetNearestTarget(RaycastHit[] hits) //리턴함수
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
    //private void OnDrawGizmos()
    //{
    //    Gizmos.color = Color.red;
    //    Gizmos.DrawWireSphere(transform.position, scanRange);

    //    Gizmos.color = Color.green;
    //    Gizmos.DrawWireSphere(transform.position, AttackRange);
    //}

    private void OnTriggerEnter(Collider other)
    {

        if (other.CompareTag("Weapon") && other.gameObject.layer == targetLayer_Index && !isHitting)
        {
            StartCoroutine(Hit_co());
        }

    }

    private IEnumerator Attack_co()
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

    private IEnumerator Hit_co()
    {
        isHitting = true;
        //히트시 대미지달기
        HP -= 1;


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
    public void Die()
    {
        ani.SetTrigger("Dead");  // 죽는모션재생
        gameObject.layer = 9;   // 레이어 DIe로 변경해서 타겟으로 안되게
        HitBox_col.enabled = false;    //부딪히지않게 콜라이더 false
        //StopCoroutine(attackCoroutine);   //공격도중이라면 공격도 중지
        Destroy(gameObject, 3f);  // 죽고나서 3초후 디스트로이
    }
    public void MinionAttack()
    {
        RaycastHit[] hits = Physics.SphereCastAll(transform.position, scanRange, Vector3.forward, 0, targetLayer);
        nearestTarget = GetNearestTarget(hits);


        if (nearestTarget != null && !isDie)
        {
            float attackDistance = Vector3.Distance(transform.position, nearestTarget.position);
            if (attackDistance <= AttackRange)
            {
                isdetecting = true;
            }
            else
            {
                isdetecting = false;
            }
            //타겟감지시 타겟쪽으로 바라보기
            LookatTarget(nearestTarget);
            // 유닛의 공격범위에 들어갈때까지 타겟에게 이동
            if (!isdetecting)
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
    }
}