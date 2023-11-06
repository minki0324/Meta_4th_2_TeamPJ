using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System.Linq;
public class UnitAttack1 : MonoBehaviour
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
   private Ply_Controller player;
    //팀의 리더가 누군지
    private LeaderState leaderState;
    private GameObject leader;
    // 유닛 공격감지범위
    [SerializeField] private float scanRange = 13f;
    [SerializeField] private float AttackRange = 1.5f;

    //이동중 적군유닛이 공격범위콜라이더에 닿았는가?
    [SerializeField] private bool isdetecting = false;
    //공격중인가?
    private bool isAttacking = false;
    private bool isHitting = false;
    private bool isSuccessAtk = true;
    private Animator ani;
    private Coroutine attackCoroutine;
    private int myLayer;
    private int combinedMask;
    // 공격 대상 레이어
    private LayerMask TeamLayer;
    private LayerMask EnemyLayer;
    //죽었을때 박스콜라이더 Enable하기위해 직접참조 
    [SerializeField] private BoxCollider HitBox_col;
    [SerializeField] private BoxCollider Ob_Weapon_col;
    
    //어택, 히트 딜레이
    private WaitForSeconds attackDelay;
    private WaitForSeconds hitDelay = new WaitForSeconds(0.2f);
    //네비게이션
    private NavMeshAgent navMeshAgent;

    [Header("현재타겟 Transform")]
    [SerializeField] public Transform nearestTarget;
    [Header("현재타겟 Layer")]
    [SerializeField] LayerMask target;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Ply_Controller>();
        navMeshAgent = GetComponent<NavMeshAgent>();
        ani = GetComponent<Animator>();
    }

    private void Start()
    {
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

    private void Update()
    {

        //오브젝트 레이어에 맞는 리더가 명령내렸을때 어택메소드 실행

        if (myLayer == TeamLayer)
        {

            if (player.CurrentMode == Ply_Controller.Mode.Attack)
            {
                MinionAttack();
            }

        }
        else
        {
            if (leaderState != null)
            {
                if (leaderState.bat_State == LeaderState.BattleState.Attack)
                {
                    MinionAttack();
                }
            }
            else
            {
                Debug.Log( $" {myLayer}번레이어 : 리더찾지못함");
                //리더없으면 그냥 어택상태
                MinionAttack();
            }
        }



        // 미니언컨트롤러로 옮길필요성있음.
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
    //레이어 감지후 가까운 타겟 설정하는메소드
    Transform GetNearestTarget(RaycastHit[] hits)
    {
        Transform nearest = null;
        float closestDistance = float.MaxValue;

        foreach (RaycastHit hit in hits)
        {
            if (hit.transform.CompareTag("SpawnPoint")) {
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
    private void LookatTarget(Transform target)
    {

        Vector3 AttackDir = target.position - transform.position;
        transform.rotation = Quaternion.LookRotation(AttackDir);
    }
    //적을감지했을때 공격하기위해 적에게 이동하는메소드
    private void AttackMoving(Transform target)
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

        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, AttackRange);
    }


    //닿은 콜라이더와 오브젝트가 레이어가 다르고
    //맞고있는중이 아니며
    //닿은 콜라이더가 검일때
    // 즉, 닿은 무기의 웨폰의 레이어가 자신과 다를때 히트
    private void OnTriggerEnter(Collider other)
    {

        if (other.CompareTag("Weapon") && (other.gameObject.layer != gameObject.layer) && !isHitting)
        {
            StartCoroutine(Hit_co());
        }

    }
    //공격코루틴메소드
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
    //히트 코루틴메소드
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
    public void Die()
    {
        ani.SetTrigger("Dead");  // 죽는모션재생
        gameObject.layer = 9;   // 레이어 DIe로 변경해서 타겟으로 안되게
        HitBox_col.enabled = false;    //부딪히지않게 콜라이더 false
        //StopCoroutine(attackCoroutine);   //공격도중이라면 공격도 중지
        if(gameObject.layer == TeamLayer) { 
        player.UnitList_List.Remove(gameObject);
        }
        else
        {
            leaderState.UnitList.Remove(gameObject);
        }
        
        leaderState.currentUnitCount--;
        Destroy(gameObject, 3f);  // 죽고나서 3초후 디스트로이




       
        
       

    }
    public void MinionAttack()
    {

 
        RaycastHit[] allHits = Physics.SphereCastAll(transform.position, scanRange, Vector3.forward, 0, combinedMask);

        
        nearestTarget = GetNearestTarget(allHits);
        //target =1 << nearestTarget.gameObject.layer;
        if (nearestTarget != null && !isDie)
        {
            //leaderState.
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
        else if (nearestTarget == null)
        {
            if(leaderState != null) { 
            LeaderAI leaderAI = leaderState.GetComponent<LeaderAI>();
            nearestTarget = leaderAI.GetNearestTarget();
            }
            else
            {
                return;
            }
            if (!isdetecting)
            {
                AttackMoving(nearestTarget);
            }
        }
        
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
    private int TargetLayers()
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
  
}