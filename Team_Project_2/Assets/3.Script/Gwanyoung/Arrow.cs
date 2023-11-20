using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    // 화살 
    // 1. 대상을 향해 일직선으로 날아감.
    // 2. 일정 시간이 지나면 사라지도록.

    private Archer_Attack archer_attack;   
    private float ArrowSpeed = 12f;  // 화살 속도
    private float destroyTime = 10f; // 발사 후 사라질 시간
    private bool isHit = false;  // 맞았는지


    private void Start()
    {
        
        StartCoroutine(Arrow_rot());
    }


    IEnumerator Arrow_rot()
    {
        archer_attack = GetComponentInParent<Archer_Attack>(); 
        Vector3 targetp = archer_attack.Target.position - archer_attack.Bow.position;  // 날아갈 방향 구하기

        // 타겟 포지션을 향해서 쏘면 바닥으로 쏴서 보정값 으로 y좀 올려줬습니당..
        transform.forward = targetp + new Vector3(0, 0.7f, 0); 

        gameObject.layer = archer_attack.gameObject.layer;
        gameObject.transform.SetParent(null);  // 독립
  
        Destroy(gameObject, destroyTime); // 쏘고 10초가 지나면 삭제            
        while (!isHit) // 맞기 전까지
        {
            transform.position += transform.forward * ArrowSpeed * Time.deltaTime;
            
            yield return null;
        }

    }
    private void OnTriggerEnter(Collider other)
    {
        // 1. 화살 발사 타이밍에 무기와 한 번 충돌이 나서 예외처리
        // 2. 화살 발사 후 본인이 맞는 경우 예외처리 
        if (other.gameObject.CompareTag("Soldier") || other.gameObject.CompareTag("Player") || other.gameObject.CompareTag("Leader"))
        {
            if (!other.gameObject.layer.Equals(gameObject.layer))
            {
                isHit = true;
                Debug.Log("맞음");

                Destroy(gameObject);
            }
        }
    }
    
}
