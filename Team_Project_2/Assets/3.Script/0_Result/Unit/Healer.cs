using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Healer : MonoBehaviour
{
    Ply_Controller pc;

    [SerializeField]
    private float healRange = 5f;

    [SerializeField]
    private Animator ani;

    [SerializeField]
    private ParticleSystem HealEffect;

    [SerializeField]
    private ParticleSystem HealCircle;

    GameObject healTarget;

    float lessHP;

    public float healCoolTime = 3f;

    public bool isHealTargetExist = false;

   // public bool testbool = false;



    public bool isCanHeal;

    private void Start()
    {

        isCanHeal = true;
        ani = GetComponent<Animator>();
    }

    public void GetHeal_Target()
    {
        healTarget = pc.UnitList_List[0];

        for (int i = 0; i < pc.UnitList_List.Count; i++)
        {
            if (pc.UnitList_List[i].GetComponent<Soilder_Controller>().data.currentHP <= lessHP && Vector3.Distance(healTarget.transform.position, transform.position) > healRange)
            {
                healTarget = pc.UnitList_List[i];
            }
        }

        if (healTarget.GetComponent<Soilder_Controller>().data.currentHP >= GameManager.instance.Current_HP)
        {
            healTarget = pc.gameObject;
        }

        lessHP = healTarget.GetComponent<Soilder_Controller>().data.currentHP;


        if(healTarget.GetComponent<Soilder_Controller>().data.currentHP >= healTarget.GetComponent<Soilder_Controller>().data.maxHP || GameManager.instance.Current_HP >= GameManager.instance.Max_Hp )
        {
            isHealTargetExist = false;
        }
        else
        {
            isHealTargetExist = true;
        }
        


}

public void Heal()
    {
        if (isCanHeal && isHealTargetExist)
        {
            ani.SetTrigger("Heal");

            GetHeal_Target();

            float healAmount = Random.Range(healTarget.GetComponent<Soilder_Controller>().data.maxHP / 10, healTarget.GetComponent<Soilder_Controller>().data.maxHP / 5);
            //�ִ�ü���� 10~20�۸�ŭ ��~

            healTarget.GetComponent<Soilder_Controller>().data.currentHP += healAmount;

            HealEffect.transform.position = healTarget.transform.position;
            HealEffect.Play();

            HealCircle.transform.position = gameObject.transform.position;
            HealCircle.Play();

            isCanHeal = false;
            StartCoroutine(HealCoolTime());
        }


    }

    public IEnumerator HealCoolTime()
    {
        yield return new WaitForSeconds(healCoolTime);
        isCanHeal = true;
    }


    //public int Compare_HP(GameObject a, GameObject b)
    //{
    //    int a_HP = a.GetComponent<Soilder_Controller>().HP;
    //    int b_HP = b.GetComponent<Soilder_Controller>().HP;


    //    return a_HP <= b_HP ? -1 : 1;
    //}
}
