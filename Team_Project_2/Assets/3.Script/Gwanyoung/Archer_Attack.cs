using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Archer_Attack : MonoBehaviour
{
    // 아처 공격 구현
    // 1. 애니메이션 구현
    // 2. 활에서 화살 생성

    public Transform Target;
    [SerializeField] private GameObject Arrow; // 화살 프리팹
    private Animator ani; // 아처 공격 애니메이션 구현하기위함
    public Transform Bow; // 화살 Transform

    private WaitForSeconds Cooltime = new WaitForSeconds(1.5f); // 쿨타임 1.5초

    private bool isAttack = false;

    private void Start()
    {
        ani = GetComponent<Animator>();
        Bow.transform.position += new Vector3(0, 0.2f, 0); // 활에서 나가는 포지션 보정
       
    }
    private void Update()
    {
        if (Target != null && !isAttack) // 타겟이 있고 공격중이 아닐 때 화살발사
        {
            StartCoroutine(Archer_atc());
        }
    }
    IEnumerator Archer_atc()
    {
        isAttack = true;
        
        ani.SetTrigger("Attack");  // 공격 애니메이션
        
        GameObject Arrow_ob = Instantiate(Arrow, Bow.position, Quaternion.identity);  // 화살 생성
        Arrow_ob.transform.parent = this.transform;   // Arrow 프리팹이 정보좀 가져가라고 잠시 상속..

        yield return Cooltime;   // 쿨타임 
        isAttack = false;  

        yield return null;
    }
}
