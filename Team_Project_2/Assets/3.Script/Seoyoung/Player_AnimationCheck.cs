using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_AnimationCheck : MonoBehaviour
{
    private Animator animator;

    private Ply_Movement pm;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        pm = FindObjectOfType<Ply_Movement>();
    }


    public void CheckAttacking()
    {
        if (pm.isAttacking_1)
        {
            pm.isAttacking_1 = false;
        }
    }
}
