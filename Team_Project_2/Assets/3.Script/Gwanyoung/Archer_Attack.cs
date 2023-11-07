using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Archer_attack : MonoBehaviour
{
    public Transform Target;
    [SerializeField] GameObject Arrow; // È­»ì ÇÁ¸®ÆÕ
    private Animator ani;
    public Transform Bow;

    WaitForSeconds Cooltime = new WaitForSeconds(1.5f);

    LeaderAI leaderai;

    private bool isAttack = false;


    private void Start()
    {
        leaderai = GetComponent<LeaderAI>();
        ani = GetComponent<Animator>();
        Bow.transform.position += new Vector3(0, 0.2f, 0);
       
    }
    private void Update()
    {
        //Target = leaderai.nearestTarget;
        if (Target!=null && !isAttack) 
        {
            StartCoroutine(Archer_atc());
        }
    }
    IEnumerator Archer_atc()
    {
        isAttack = true;
        
        ani.SetTrigger("Attack");
        GameObject Arrow_ob = Instantiate(Arrow, Bow.position, transform.rotation);
        Arrow_ob.transform.parent = this.transform;
        yield return Cooltime;
        isAttack = false;
        yield return null;
    }
}
