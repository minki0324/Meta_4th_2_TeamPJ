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


    public void CheckAttack1()  //모션 1 끝난후
    {
        Debug.Log("모션 1 끝");
        //pm.GetComponent<Animator>().SetBool("Attack1", false);
        pm.isAttacking_1 = false;
        pm.isPossible_Attack_2 = false;
        pm.isPossible_Attack_1 = true;
        
    }

    public void Check_PossibleAttack2() //모션 1 공격 중반
    {
        pm.isPossible_Attack_2 = true;
    }


    public void CheckAttack2()  //모션 2 끝난 후 
    {
        if (pm.isAttacking_2)
        {
            pm.isAttacking_2 = false;
            pm.isAttacking_1 = false;

        }
      
    }

    public void Check_PossibleAttack1() //모션 2 공격 중반
    {
        pm.isPossible_Attack_1 = true;
       
    }


    public void Attack_Order()
    {
        Debug.Log("모션 attack 끝");
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
