using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_AnimationCheck : MonoBehaviour
{
    private Animator animator;

    private Ply_Movement pm;

    private Ply_Controller pc;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        pm = FindObjectOfType<Ply_Movement>();
        pc = FindObjectOfType<Ply_Controller>();
    }


    public void CheckAttack1()  //��� 1 ������
    {
        //pm.GetComponent<Animator>().SetBool("Attack1", false);
        pm.isAttacking_1 = false;
        pm.isPossible_Attack_2 = false;
        pm.isPossible_Attack_1 = true;
        
    }

    public void Check_PossibleAttack2() //��� 1 ���� �߹�
    {
        pm.isPossible_Attack_2 = true;
    }


    public void CheckAttack2()  //��� 2 ���� �� 
    {
        if (pm.isAttacking_2)
        {
            pm.isAttacking_2 = false;
            pm.isAttacking_1 = false;

        }
      
    }

    public void Check_PossibleAttack1() //��� 2 ���� �߹�
    {
        pm.isPossible_Attack_1 = true;
       
    }


    public void Attack_Order()
    {
        Debug.Log("��� attack ��");
        pc.isPlay_AttackOrder = false;
    }


    public void Follow_Order()
    {
        pc.isPlay_FollowOrder = false;
    }

    public void Stop_Order()
    {
        pc.isPlay_StopOrder = false;
    }
}
