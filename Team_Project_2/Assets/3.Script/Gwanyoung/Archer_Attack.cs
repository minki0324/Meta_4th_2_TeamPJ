using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Archer_Attack : MonoBehaviour
{
    // ��ó ���� ����
    // 1. �ִϸ��̼� ����
    // 2. Ȱ���� ȭ�� ����

    public Transform Target;
    [SerializeField] private GameObject Arrow; // ȭ�� ������
    private Animator ani; // ��ó ���� �ִϸ��̼� �����ϱ�����
    public Transform Bow; // ȭ�� Transform

    private WaitForSeconds Cooltime = new WaitForSeconds(1.5f); // ��Ÿ�� 1.5��

    private bool isAttack = false;

    private void Start()
    {
        ani = GetComponent<Animator>();
        Bow.transform.position += new Vector3(0, 0.2f, 0); // Ȱ���� ������ ������ ����
       
    }
    private void Update()
    {
        if (Target != null && !isAttack) // Ÿ���� �ְ� �������� �ƴ� �� ȭ��߻�
        {
            StartCoroutine(Archer_atc());
        }
    }
    IEnumerator Archer_atc()
    {
        isAttack = true;
        
        ani.SetTrigger("Attack");  // ���� �ִϸ��̼�
        
        GameObject Arrow_ob = Instantiate(Arrow, Bow.position, Quaternion.identity);  // ȭ�� ����
        Arrow_ob.transform.parent = this.transform;   // Arrow �������� ������ ��������� ��� ���..

        yield return Cooltime;   // ��Ÿ�� 
        isAttack = false;  

        yield return null;
    }
}
