using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Healer : MonoBehaviour
{
    Ply_Controller pc;

    [SerializeField]
    private float healRange = 10f;

    [SerializeField]
    private Animator ani;

    [SerializeField]
    private ParticleSystem HealEffect;

    [SerializeField]
    private ParticleSystem HealCircle;

    GameObject healTarget = new GameObject();

    float lessHP;

    public float healCoolTime;

    public bool isHealTargetExist = false;

    // public bool testbool = false;



    public bool isCanHeal;

    private void Start()
    {
        pc = FindObjectOfType<Ply_Controller>();
        isCanHeal = true;
        ani = GetComponent<Animator>();
    }

    public void GetHeal_Target()
    {
        Debug.Log("ÈúÈúÈúÈúÈúÈúÈúÈúÈúÈúÈúÈúÈúÈúÈúÈúÈúÈúÈúÈúÈúÈúÈúÈúÈúÈúÈúÈúÈúÈúÈúÈúÈúÈúÈúÈúÈú");

        //Èú Å¸°Ù UnitList0¹øÀ¸·Î ÀÓÀÇ ÁöÁ¤
        healTarget = pc.UnitList_List[0].gameObject;    //null
        if(healTarget == null)
        {
            Debug.Log("½Ã¹ß·Ò¾Æ ¿Ö ¶Ç ¾ø°í Áö¶öÀÎµ¥¤Ğ¤Ğ¤Ğ¤Ğ¤Ğ¤Ğ¤Ğ¤Ğ¤Ğ¤Ğ¤Ğ¤Ğ¤Ğ¤Ğ¤Ğ¤Ğ");
        }
    
        lessHP = healTarget.GetComponent<Soilder_Controller>().data.currentHP;
        //Debug.Log("HealTarget : " + healTarget.name.ToString());
        
        for (int i = 0; i < pc.UnitList_List.Count; i++)
        {
            if (pc.UnitList_List[i].GetComponent<Soilder_Controller>().data.currentHP <= lessHP && Vector3.Distance(healTarget.transform.position, transform.position) < healRange)
            {
                //ÃÖ¼ÒHP¸¦ °¡Áø ¾Öº¸´Ù ÇÇ°¡ ÀÛ°í Èú°¡´É ¹üÀ§ ³»(10f)¾È¿¡ ÀÖÀ¸¸é
                healTarget = pc.UnitList_List[i];
                lessHP = healTarget.GetComponent<Soilder_Controller>().data.currentHP;

                
            }
        }

        if (healTarget.GetComponent<Soilder_Controller>().data.currentHP >= GameManager.instance.Current_HP)
        {
            healTarget = pc.gameObject;

        }


      

        if (healTarget.GetComponent<Soilder_Controller>().data.currentHP >= healTarget.GetComponent<Soilder_Controller>().data.maxHP)
        {
            //ÈúÅ¸°ÙÀÇ ÇöÀç HP°¡ maxphpÀÎ °æ¿ì 
            isHealTargetExist = false;
            Debug.Log("ÈúÅ¸°Ù Á¸ÀçÇÏÁö ¾ÊÀ½");
        }
        else
        {
            isHealTargetExist = true;
            Debug.Log("HealTarget : " + healTarget.name.ToString());
            Heal();
        }
     

      
    }

    public void Heal()
    {
       
        if (isCanHeal)
        {
            Debug.Log("ºü¿ÍÈú@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@");

            ani.SetTrigger("Heal");



            float healAmount = Random.Range(healTarget.GetComponent<Soilder_Controller>().data.maxHP / 10, healTarget.GetComponent<Soilder_Controller>().data.maxHP / 5);
            //ÃÖ´ëÃ¼·ÂÀÇ 10~20ÆÛ¸¸Å­ Èú~

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
        healCoolTime = Random.Range(3, 7);
        Debug.Log("ÈúÄğÄğÄğÄğÄğÄğÄğÄğÄğÄğÄğÄğÄğÄğÄğÄğÄğÄğÄğÄğÄğÄğÄğÄğÄğÄğ");
        yield return new WaitForSeconds(healCoolTime);
        isCanHeal = true;
        yield break;
    }


    //public int Compare_HP(GameObject a, GameObject b)
    //{
    //    int a_HP = a.GetComponent<Soilder_Controller>().HP;
    //    int b_HP = b.GetComponent<Soilder_Controller>().HP;


    //    return a_HP <= b_HP ? -1 : 1;
    //}
}