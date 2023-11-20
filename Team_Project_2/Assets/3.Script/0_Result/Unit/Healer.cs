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

    GameObject healTarget = new GameObject();

    float lessHP;

    public float healCoolTime = 3f;

    public bool isHealTargetExist = false;

    // public bool testbool = false;



    public bool isCanHeal;

    private void Start()
    {

        isCanHeal = true;
       // ani = GetComponent<Animator>();
    }

    public void GetHeal_Target()
    {
        Debug.Log("枇枇枇枇枇枇枇枇枇枇枇枇枇枇枇枇枇枇枇枇枇枇枇枇枇枇枇枇枇枇枇枇枇枇枇枇枇");

        //枇 展為 UnitList0腰生稽 績税 走舛
       // healTarget = pc.UnitList_List[0].gameObject;    //null
        if(healTarget == null)
        {
            Debug.Log("獣降繋焼 訊 暁 蒸壱 走偶昔汽ばばばばばばばばばばばばばばばば");
        }
        else
        {
            Debug.Log("戚杏 眼壕亜..?");
        }
       
        lessHP = healTarget.GetComponent<Soilder_Controller>().data.currentHP;
        //Debug.Log("HealTarget : " + healTarget.name.ToString());
        // && Vector3.Distance(healTarget.transform.position, transform.position) > healRange
        for (int i = 0; i < pc.UnitList_List.Count; i++)
        {
            if (pc.UnitList_List[i].GetComponent<Soilder_Controller>().data.currentHP <= lessHP)
            {
                healTarget = pc.UnitList_List[i];
                lessHP = healTarget.GetComponent<Soilder_Controller>().data.currentHP;

                
            }
        }

        if (healTarget.GetComponent<Soilder_Controller>().data.currentHP >= GameManager.instance.Current_HP)
        {
            healTarget = pc.gameObject;

        }


        Debug.Log("HealTarget : " + healTarget.name.ToString());

        if (healTarget.GetComponent<Soilder_Controller>().data.currentHP >= healTarget.GetComponent<Soilder_Controller>().data.maxHP || GameManager.instance.Current_HP >= GameManager.instance.Max_Hp)
        {
            isHealTargetExist = false;
        }
        else
        {
            isHealTargetExist = true;
        }


        Heal();
    }

    public void Heal()
    {
       
        if (isCanHeal && isHealTargetExist)
        {
            Debug.Log("匙人枇@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@");

            //ani.SetTrigger("Heal");



            float healAmount = Random.Range(healTarget.GetComponent<Soilder_Controller>().data.maxHP / 10, healTarget.GetComponent<Soilder_Controller>().data.maxHP / 5);
            //置企端径税 10~20遁幻鏑 枇~

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
        Debug.Log("枇悌悌悌悌悌悌悌悌悌悌悌悌悌悌悌悌悌悌悌悌悌悌悌悌悌悌");
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