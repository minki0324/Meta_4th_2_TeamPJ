using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    // ȭ�� 
    // 1. ����� ���� ���������� ���ư�.
    // 2. ���� �ð��� ������ ���������.

    private Archer_Attack archer_attack;   
    private float ArrowSpeed = 12f;  // ȭ�� �ӵ�
    private float destroyTime = 10f; // �߻� �� ����� �ð�
    private bool isHit = false;  // �¾Ҵ���


    private void Start()
    {
        
        StartCoroutine(Arrow_rot());
    }


    IEnumerator Arrow_rot()
    {
        archer_attack = GetComponentInParent<Archer_Attack>(); 
        Vector3 targetp = archer_attack.Target.position - archer_attack.Bow.position;  // ���ư� ���� ���ϱ�

        // Ÿ�� �������� ���ؼ� ��� �ٴ����� ���� ������ ���� y�� �÷�����ϴ�..
        transform.forward = targetp + new Vector3(0, 0.7f, 0); 

        gameObject.layer = archer_attack.gameObject.layer;
        gameObject.transform.SetParent(null);  // ����
  
        Destroy(gameObject, destroyTime); // ��� 10�ʰ� ������ ����            
        while (!isHit) // �±� ������
        {
            transform.position += transform.forward * ArrowSpeed * Time.deltaTime;
            
            yield return null;
        }

    }
    private void OnTriggerEnter(Collider other)
    {
        // 1. ȭ�� �߻� Ÿ�ֿ̹� ����� �� �� �浹�� ���� ����ó��
        // 2. ȭ�� �߻� �� ������ �´� ��� ����ó�� 
        if (other.gameObject.CompareTag("Soldier") || other.gameObject.CompareTag("Player") || other.gameObject.CompareTag("Leader"))
        {
            if (!other.gameObject.layer.Equals(gameObject.layer))
            {
                isHit = true;
                Debug.Log("����");

                Destroy(gameObject);
            }
        }
    }
    
}
