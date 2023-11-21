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

    private WaitForSeconds Cooltime = new WaitForSeconds(3f); // ��Ÿ�� 1.5��
    private WaitForSeconds Sink = new WaitForSeconds(0.8f);
    [SerializeField] private AudioSource BowSound;

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

        BowSound.PlayOneShot(AudioManager.instance.clip_SFX[(int)SFXList.Arrow_Shoot]);
        yield return Sink;


        GameObject Arrow_ob = Instantiate(Arrow, Bow.position, Quaternion.identity);  // ȭ�� ����
        Arrow_ob.transform.parent = this.transform;   // Arrow �������� ������ ��������� ��� ���..

        yield return Cooltime;   // ��Ÿ�� 
        isAttack = false;  

        yield return null;
    }
}
