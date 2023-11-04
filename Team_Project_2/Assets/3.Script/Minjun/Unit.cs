using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Unit : LeaderState
{
   
    private bool isDie;
    private Ply_Controller player;
    //팀의 리더가 누군지

    //공격중인가?

  
   

    private int myLayer;
    protected int combinedMask;
    // 공격 대상 레이어
    private LayerMask TeamLayer;
    private bool isSuccessAtk = true;
    //죽었을때 박스콜라이더 Enable하기위해 직접참조 
    [SerializeField] private BoxCollider HitBox_col;
    [SerializeField] private BoxCollider Ob_Weapon_col;

  
    //네비게이션
    protected NavMeshAgent navMeshAgent;

    [Header("현재타겟 Transform")]
    [SerializeField] protected Transform NearestTarget;
    public Transform GetNearestTarget()
    {
        return NearestTarget;
    }

    [Header("현재타겟 Layer")]
    [SerializeField] LayerMask target;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponentInChildren<Ply_Controller>();
        navMeshAgent = GetComponent<NavMeshAgent>();
   
    }

    private void Start()
    {
        //자신의 레이어를 제외한 적팀레이어를 담은 배열 계산하는 메소드

        myLayer = gameObject.layer;
        TeamLayer = LayerMask.NameToLayer("Team");
        combinedMask = TargetLayers();




        //
      


    }

    //레이어 감지후 가까운 타겟 설정하는메소드
    public  Transform GetNearestTarget(RaycastHit[] hits)
    {
        Transform nearest = null;
        float closestDistance = float.MaxValue;

        
        foreach (RaycastHit hit in hits)
        {
            if (hit.transform.CompareTag("SpawnPoint"))
            {
                continue;
            }
            float distance = Vector3.Distance(transform.position, hit.transform.position);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                nearest = hit.transform;
            }
        }

        return nearest;
    }
    //적을감지했을때 적을바라보는 메소드
    public  void LookatTarget(Transform target)
    {

        Vector3 AttackDir = target.position - transform.position;
        transform.rotation = Quaternion.LookRotation(AttackDir);
    }
    //적을감지했을때 공격하기위해 적에게 이동하는메소드


    //공격코루틴메소드
 
    //히트 코루틴메소드


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
   
    
   
    public int TargetLayers()
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
