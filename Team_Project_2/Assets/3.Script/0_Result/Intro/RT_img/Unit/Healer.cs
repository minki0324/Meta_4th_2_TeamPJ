/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Healer : MonoBehaviour
{
    Ply_Controller pc;

    [SerializeField]
    private Animator ani;

    [SerializeField]
    private ParticleSystem HealEffect;

    [SerializeField]
    private ParticleSystem HealCircle;

    GameObject lessHPMinion;
    float lessHP;
    public float healCoolTime = 3f;



    public bool isCanHeal;
    private void Start()
    {
        
        isCanHeal = true;
        ani = GetComponent<Animator>();
    }


    public void Heal()
    {
        if(isCanHeal)
        {
            ani.SetTrigger("Heal");

            lessHPMinion = pc.UnitList_List[0];
            lessHP = lessHPMinion.GetComponent<                >().data.currentHP;
            for (int i = 0; i < pc.UnitList_List.Count; i++)
            {
                if (pc.UnitList_List[i].GetComponent<Soilder_Controller>().data.currentHP <= lessHP)
                {
                    lessHPMinion = pc.UnitList_List[i];

                }
                lessHP = lessHPMinion.GetComponent<Soilder_Controller>().data.currentHP;
            }

            lessHPMinion.GetComponent<Soilder_Controller>().data.currentHP += 1;
            HealEffect.transform.position = lessHPMinion.transform.position;
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
*/